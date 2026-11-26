using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

/// <summary>
/// Input parameters for selecting LinkedIn ad account users.
/// </summary>
public class Input
{
    /// <summary>
    /// Enumeration to choose how the search is done.
    /// </summary>
    /// <example>SearchAttribute.FindAdAccountsByAuthenticatedUser</example>
    public SearchAttribute SearchMethod { get; set; }

    /// <summary>
    /// Account URN to use when retrieving a specific account user.
    /// </summary>
    /// <example>urn:li:sponsoredAccount:516986977</example>
    [UIHint(nameof(SearchMethod), "", SearchAttribute.GetAdAccountUser)]
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string AccountUrn { get; set; }

    /// <summary>
    /// Account URNs to use when searching users for specific ad accounts.
    /// </summary>
    /// <example>[urn:li:sponsoredAccount:516986977]</example>
    [UIHint(nameof(SearchMethod), "", SearchAttribute.FindAdAccountUsersByAccounts)]
    [DefaultValue("")]
    public AccountUrn[] AccountUrns { get; set; }

    /// <summary>
    /// Person URN to use when retrieving a specific account user.
    /// </summary>
    /// <example>urn:li:person:_mVMF2Kp8p</example>
    [UIHint(nameof(SearchMethod), "", SearchAttribute.GetAdAccountUser)]
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string UserUrn { get; set; }
}
