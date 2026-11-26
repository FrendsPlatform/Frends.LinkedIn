using System.Collections.Generic;

namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

/// <summary>
/// Result of the LinkedIn ad account user search.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Body of response. Response body contains paging and elements.
    /// </summary>
    /// <example>{"paging":{"start":0,"count":10,"links":[],"total":1},"elements":[{"totalBudget":100}]}</example>
    public dynamic Body { get; set; }

    /// <summary>
    /// Headers of response.
    /// </summary>
    /// <example>{"content-type":"application/json"}</example>
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Status code of the response.
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Error that occurred during the task.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
