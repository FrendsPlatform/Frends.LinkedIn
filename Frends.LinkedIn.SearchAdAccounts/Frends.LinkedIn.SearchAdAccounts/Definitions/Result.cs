namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Result of a LinkedIn ad account search.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets or sets a value indicating whether the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the body of the response, containing paging and account elements.
    /// </summary>
    /// <example>{"paging":{"start":0,"count":10,"total":1},"elements":[{"id":"512500029"}]}</example>
    public dynamic Body { get; set; }

    /// <summary>
    /// Gets or sets the headers returned by LinkedIn.
    /// </summary>
    /// <example>{"content-type":"application/json"}</example>
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code returned by LinkedIn.
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the details of an error that occurred during task execution.
    /// </summary>
    /// <example>null</example>
    public Error Error { get; set; }
}
