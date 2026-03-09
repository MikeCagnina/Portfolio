using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AITraining.Harvey;

/// <summary>
/// .NET client for the Harvey AI Assistant API (legal AI). Uses Bearer token authentication and the completion endpoint.
/// See https://developers.harvey.ai/ for API details. Obtain your token from your Harvey Customer Success Manager.
/// </summary>
public class HarveyApiClient
{
    private const string DefaultBaseUrl = "https://api.harvey.ai";
    private const string CompletionPath = "/api/v2/completion";

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Creates a new Harvey API client.
    /// </summary>
    /// <param name="bearerToken">API token from your Harvey Customer Success Manager. Do not commit; use configuration or secrets.</param>
    /// <param name="baseUrl">Optional. Use for EU: https://eu.api.harvey.ai or AU: https://au.api.harvey.ai.</param>
    /// <param name="httpClient">Optional. If null, a new HttpClient is created.</param>
    public HarveyApiClient(string bearerToken, string? baseUrl = null, HttpClient? httpClient = null)
    {
        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            throw new ArgumentException("Bearer token is required.", nameof(bearerToken));
        }

        _baseUrl = baseUrl?.TrimEnd('/') ?? DefaultBaseUrl;
        _httpClient = httpClient ?? new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Sends a completion request (no file attachments). Rate limit: 20 requests per minute.
    /// </summary>
    /// <param name="prompt">Question or request for Harvey (max 20,000 characters; 4,000 if sending files).</param>
    /// <param name="stream">True to stream the response via SSE; default false.</param>
    /// <param name="mode">"draft" or "assist". Default "draft".</param>
    /// <param name="includeCitations">Include citations in the response. Default true.</param>
    /// <param name="clientMatterId">Optional UUID to associate with a client matter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The completion response.</returns>
    public async Task<HarveyCompletionResponse> GetCompletionAsync(
        string prompt,
        bool stream = false,
        string mode = "draft",
        bool includeCitations = true,
        Guid? clientMatterId = null,
        CancellationToken cancellationToken = default)
    {
        return await GetCompletionAsync(prompt, null, stream, mode, includeCitations, clientMatterId, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a completion request with optional file uploads. Rate limit: 20 requests per minute.
    /// </summary>
    /// <param name="prompt">Question or request (max 20,000 chars without files; 4,000 with files).</param>
    /// <param name="filePaths">Optional paths to files to attach for context. Cannot be used with knowledgeSources.</param>
    /// <param name="stream">True to stream the response.</param>
    /// <param name="mode">"draft" or "assist".</param>
    /// <param name="includeCitations">Include citations. Default true.</param>
    /// <param name="clientMatterId">Optional client matter UUID.</param>
    /// <param name="knowledgeSourcesJson">Optional JSON-encoded knowledge sources (e.g. Vault). Cannot be used with filePaths.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<HarveyCompletionResponse> GetCompletionAsync(
        string prompt,
        IReadOnlyList<string>? filePaths,
        bool stream = false,
        string mode = "draft",
        bool includeCitations = true,
        Guid? clientMatterId = null,
        string? knowledgeSourcesJson = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentException("Prompt is required.", nameof(prompt));
        }

        if (filePaths?.Count > 0 && !string.IsNullOrEmpty(knowledgeSourcesJson))
        {
            throw new ArgumentException("Cannot use both file uploads and knowledge_sources.");
        }

        string url = $"{_baseUrl}{CompletionPath}?include_citations={includeCitations.ToString().ToLowerInvariant()}";

        using (MultipartFormDataContent form = new MultipartFormDataContent())
        {
            form.Add(new StringContent(prompt), "prompt");
            form.Add(new StringContent(stream ? "true" : "false"), "stream");
            form.Add(new StringContent(mode), "mode");

            if (clientMatterId.HasValue)
            {
                form.Add(new StringContent(clientMatterId.Value.ToString()), "client_matter_id");
            }

            if (!string.IsNullOrEmpty(knowledgeSourcesJson))
            {
                form.Add(new StringContent(knowledgeSourcesJson), "knowledge_sources");
            }

            if (filePaths != null)
            {
                foreach (string path in filePaths)
                {
                    string fileName = Path.GetFileName(path);
                    byte[] bytes = await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false);
                    ByteArrayContent fileContent = new ByteArrayContent(bytes);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    form.Add(fileContent, "file", fileName);
                }
            }

            HttpResponseMessage response = await _httpClient.PostAsync(url, form, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new HarveyApiException(response.StatusCode, body);
            }

            if (stream)
            {
                return await ReadStreamedResponseAsync(response, cancellationToken).ConfigureAwait(false);
            }

            string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            HarveyCompletionResponse? result = JsonSerializer.Deserialize<HarveyCompletionResponse>(json, _jsonOptions);
            if (result == null)
            {
                throw new InvalidOperationException("Harvey API returned empty or invalid JSON.");
            }

            return result;
        }
    }

    /// <summary>
    /// Streams completion chunks via Server-Sent Events. Each chunk contains a partial "response" string; final event may include response_with_citations and sources.
    /// </summary>
    public async IAsyncEnumerable<HarveyCompletionResponse> StreamCompletionAsync(
        string prompt,
        bool includeCitations = true,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentException("Prompt is required.", nameof(prompt));
        }

        string url = $"{_baseUrl}{CompletionPath}?include_citations={includeCitations.ToString().ToLowerInvariant()}";

        using (MultipartFormDataContent form = new MultipartFormDataContent())
        {
            form.Add(new StringContent(prompt), "prompt");
            form.Add(new StringContent("true"), "stream");
            form.Add(new StringContent("draft"), "mode");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = form;
                using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    await using (Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string? line;
                        while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
                        {
                            if (line.StartsWith("data: ", StringComparison.Ordinal))
                            {
                                string data = line.Substring(6).Trim();
                                if (data == "[DONE]")
                                {
                                    yield break;
                                }

                                HarveyCompletionResponse? chunk = JsonSerializer.Deserialize<HarveyCompletionResponse>(data, _jsonOptions);
                                if (chunk != null)
                                {
                                    yield return chunk;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static async Task<HarveyCompletionResponse> ReadStreamedResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        StringBuilder fullResponse = new StringBuilder();
        string? responseWithCitations = null;
        List<HarveyCitationSource>? sources = null;

        await using (Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            string? line;
            while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
            {
                if (!line.StartsWith("data: ", StringComparison.Ordinal))
                {
                    continue;
                }

                string data = line.Substring(6).Trim();
                if (data == "[DONE]")
                {
                    break;
                }

                HarveyCompletionResponse? chunk = JsonSerializer.Deserialize<HarveyCompletionResponse>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (chunk == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(chunk.Response))
                {
                    fullResponse.Append(chunk.Response);
                }

                if (chunk.ResponseWithCitations != null)
                {
                    responseWithCitations = chunk.ResponseWithCitations;
                }

                if (chunk.Sources != null && chunk.Sources.Count > 0)
                {
                    sources = chunk.Sources;
                }
            }
        }

        return new HarveyCompletionResponse
        {
            Response = fullResponse.ToString(),
            ResponseWithCitations = responseWithCitations,
            Sources = sources
        };
    }
}
