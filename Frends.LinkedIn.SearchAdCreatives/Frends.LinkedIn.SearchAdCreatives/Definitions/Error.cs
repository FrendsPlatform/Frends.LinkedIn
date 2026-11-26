namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Error details returned when a task execution fails.
/// </summary>
public class Error
{
    /// <summary>
    /// Summary of the error.
    /// </summary>
    /// <example>LinkedIn creative search failed: request timed out.</example>
    public string Message { get; set; }

    /// <summary>
    /// Exception details for the failure.
    /// </summary>
    /// <example>object { Exception AdditionalInfo }</example>
    public Exception AdditionalInfo { get; set; }
}
