using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

/// <summary>
/// Connection parameters for the LinkedIn API.
/// </summary>
public class Connection
{
    /// <summary>
    /// LinkedIn access token used to authenticate requests.
    /// </summary>
    /// <example>your-linkedin-access-token</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    public string Token { get; set; }
}
