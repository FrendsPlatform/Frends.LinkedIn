using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCampaigns.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// OAuth access token to be used for requests. Token will be added as Authorization header.
    /// </summary>
    /// <example>xRqjX_9QXZlvRL6HZHJvbrubhvceV4NFccANh6RwndmAJDkPfP...</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    public string Token { get; set; }
}
