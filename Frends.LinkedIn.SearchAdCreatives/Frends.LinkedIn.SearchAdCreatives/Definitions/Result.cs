using System.Collections.Generic;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Result of the LinkedIn ad creative search.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Response body returned by LinkedIn.
    /// </summary>
    /// <example>{"paging":{"start":0,"count":10,"total":1},"elements":[{"id":"119962155"}]}</example>
    public dynamic Body { get; set; }

    /// <summary>
    /// Headers returned by LinkedIn.
    /// </summary>
    /// <example>{"content-type":"application/json"}</example>
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// HTTP status code returned by LinkedIn.
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Error details when the task fails.
    /// </summary>
    /// <example>null</example>
    public Error Error { get; set; }
}
