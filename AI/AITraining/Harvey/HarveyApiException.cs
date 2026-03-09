using System.Net;

namespace AITraining.Harvey;

/// <summary>
/// Thrown when the Harvey API returns a non-success HTTP status (e.g. 400, 401, 429, 500).
/// </summary>
public class HarveyApiException : Exception
{
    /// <summary>
    /// HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Response body from the API (may contain error message).
    /// </summary>
    public string ResponseBody { get; }

    /// <summary>
    /// Creates a new HarveyApiException.
    /// </summary>
    public HarveyApiException(HttpStatusCode statusCode, string responseBody)
        : base($"Harvey API error ({(int)statusCode} {statusCode}): {responseBody}")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody ?? string.Empty;
    }
}
