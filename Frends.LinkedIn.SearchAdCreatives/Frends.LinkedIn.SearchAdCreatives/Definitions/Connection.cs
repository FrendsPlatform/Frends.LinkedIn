using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// LinkedIn authentication parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// LinkedIn access token used to authorize the request.
    /// </summary>
    /// <example>linkedin-access-token</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string Token { get; set; }
}
