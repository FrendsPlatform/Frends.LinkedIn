using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.Request.Definitions;

/// <summary>
/// Options parameter.
/// </summary>
public class Options
{
    /// <summary>
    /// Throw an exception when the request fails.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Custom message to include when the request fails.
    /// </summary>
    /// <example>LinkedIn request failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
