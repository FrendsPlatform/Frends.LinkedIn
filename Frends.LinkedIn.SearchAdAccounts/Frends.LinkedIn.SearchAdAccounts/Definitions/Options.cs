namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Additional failure-handling options.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets a value indicating whether to throw an error when task execution fails.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets an optional prefix for the error message when task execution fails.
    /// </summary>
    /// <example>LinkedIn ad account search failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
