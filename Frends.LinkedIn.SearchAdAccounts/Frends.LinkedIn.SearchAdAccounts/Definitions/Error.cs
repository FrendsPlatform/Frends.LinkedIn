namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Details of an error that occurred during task execution.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets a summary of the error.
    /// </summary>
    /// <example>LinkedIn ad account search failed: request timed out</example>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the exception raised during task execution.
    /// </summary>
    /// <example>object { Exception AdditionalInfo }</example>
    public Exception AdditionalInfo { get; set; }
}
