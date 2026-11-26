using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

/// <summary>
/// Options parameter.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets a value indicating whether to throw an error when the task fails.
    /// </summary>
    /// <example><c>true</c>.</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message to use when the task fails.
    /// </summary>
    /// <example><c>LinkedIn ad analytics request failed.</c>.</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
