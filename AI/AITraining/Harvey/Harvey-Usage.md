# Harvey AI – Review and .NET Usage

## What is Harvey AI?

**Harvey** (www.harvey.ai) is an AI platform built for **legal and professional services**. It is used by law firms and in-house legal teams for:

- **Legal research** – Complex legal, regulatory, and tax questions
- **Deal management & due diligence** – Contract analysis, review, fund formation
- **Document storage & analysis** – Vault for secure storage and bulk analysis
- **Assistant** – Ask questions, analyze documents, and draft with domain-specific AI

Key product areas: Ecosystem, Workflows, Knowledge, Vault, Assistant. The **Assistant API** is what you call from .NET (completion endpoint). Harvey reports enterprise security (SOC2 II, ISO 27001, GDPR, etc.) and is used by many AmLaw 100 firms.

There is **no official Harvey .NET SDK**. Integration is done via their **REST API** with Bearer token authentication.

---

## API Summary

- **Base URL:** `https://api.harvey.ai` (EU: `https://eu.api.harvey.ai`, AU: `https://au.api.harvey.ai`)
- **Auth:** `Authorization: Bearer YOUR_TOKEN_HERE` (token from your Harvey Customer Success Manager)
- **Main endpoint:** `POST /api/v2/completion` (Assistant API)
- **Rate limit:** 20 requests per minute for the completion endpoint
- **Request body:** `multipart/form-data` with `prompt` (required), optional `stream`, `mode`, `file[]`, `knowledge_sources`, `client_matter_id`
- **Query:** `include_citations` (default true)

Docs: https://developers.harvey.ai/ (Introduction, Authentication, Assistant, Completion API reference).

---

## .NET Client in This Solution

The `AITraining.Harvey` folder contains:

- **HarveyApiClient** – HTTP client for the completion endpoint (non-stream and stream).
- **HarveyCompletionResponse** / **HarveyCitationSource** – Response DTOs.
- **HarveyApiException** – Wraps non-success HTTP responses (400, 401, 429, 500).

Store your Harvey API token in configuration or secrets (e.g. `HARVEY_API_KEY`); never commit it.

---

## Usage Examples

### 1. Simple completion (no files)

```csharp
using AITraining.Harvey;

string apiToken = Environment.GetEnvironmentVariable("HARVEY_API_KEY") ?? "YOUR_TOKEN_HERE";
HarveyApiClient client = new HarveyApiClient(apiToken);

HarveyCompletionResponse result = await client.GetCompletionAsync(
    "In 3 sentences, explain how indemnity clauses typically work in commercial contracts.",
    stream: false,
    mode: "draft",
    includeCitations: true);

Console.WriteLine(result.Response);
if (result.Sources != null)
{
    foreach (HarveyCitationSource source in result.Sources)
    {
        Console.WriteLine($"[{source.CitationNum}] {source.DocumentName} (page {source.Page}): {source.Text}");
    }
}
```

### 2. Completion with file attachment

```csharp
HarveyCompletionResponse result = await client.GetCompletionAsync(
    "Summarize the key obligations in this contract.",
    filePaths: new[] { @"C:\path\to\contract.pdf" },
    stream: false,
    includeCitations: true);
```

### 3. Streaming (SSE)

```csharp
await foreach (HarveyCompletionResponse chunk in client.StreamCompletionAsync(
    "Draft a short response to a client asking about renewal timeline.",
    includeCitations: false,
    cancellationToken))
{
    if (!string.IsNullOrEmpty(chunk.Response))
    {
        Console.Write(chunk.Response);
    }
}
```

### 4. Error handling

```csharp
try
{
    HarveyCompletionResponse result = await client.GetCompletionAsync("Your prompt");
}
catch (HarveyApiException ex)
{
    // ex.StatusCode: 400 Bad Request, 401 Unauthorized, 429 Rate limit, 500 Server error
    // ex.ResponseBody: API error message
}
```

### 5. EU-hosted API

```csharp
HarveyApiClient client = new HarveyApiClient(
    bearerToken: apiToken,
    baseUrl: "https://eu.api.harvey.ai");
```

---

## Prompt limits

- **Without file:** prompt up to 20,000 characters.
- **With file:** prompt up to 4,000 characters.

Use `mode`: `"draft"` (default) or `"assist"`. Do not mix `file` and `knowledge_sources` in the same request.
