using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Additional error-handling options.
/// </summary>
public class Options
{
    /// <summary>
    /// Whether task failures are thrown instead of returned in the result.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Optional message prefix used when handling a task failure.
    /// </summary>
    /// <example>LinkedIn creative search failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
