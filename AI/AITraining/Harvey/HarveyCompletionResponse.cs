using System.Text.Json.Serialization;

namespace AITraining.Harvey;

/// <summary>
/// Response from the Harvey Assistant API completion endpoint.
/// </summary>
public class HarveyCompletionResponse
{
    /// <summary>
    /// The completion text of the input prompt.
    /// </summary>
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// The completion with inline citations (e.g. [1][2]). Only present when include_citations=true.
    /// </summary>
    [JsonPropertyName("response_with_citations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ResponseWithCitations { get; set; }

    /// <summary>
    /// Cited source snippets. Only present when include_citations=true and documents were provided.
    /// </summary>
    [JsonPropertyName("sources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<HarveyCitationSource>? Sources { get; set; }
}

/// <summary>
/// A single cited source in a Harvey completion response.
/// </summary>
public class HarveyCitationSource
{
    /// <summary>
    /// Citation number matching inline references in the response.
    /// </summary>
    [JsonPropertyName("citation_num")]
    public int CitationNum { get; set; }

    /// <summary>
    /// Name of the source document.
    /// </summary>
    [JsonPropertyName("document_name")]
    public string? DocumentName { get; set; }

    /// <summary>
    /// Page number in the source, if applicable.
    /// </summary>
    [JsonPropertyName("page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Page { get; set; }

    /// <summary>
    /// Quoted snippet text (may contain markup).
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
