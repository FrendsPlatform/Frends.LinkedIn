namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class.
/// </summary>
public class Filter
{
    /// <summary>
    /// Enumeration to choose how the search is done.
    /// </summary>
    /// <example>SearchAttribute.FindAdAccountsByAuthenticatedUser</example>
    public SearchAttribute SearchMethod { get; set; }

    /// <summary>
    /// Searches for users with AdAccount URN.
    /// </summary>
    /// <example>urn:li:sponsoredAccount:516986977</example>
    [UIHint(nameof(SearchMethod), "", SearchAttribute.GetAdAccountUser)]
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string AccountUrn { get; set; }

    /// <summary>
    /// Searches for users with AdAccount URN.
    /// </summary>
    /// <example>[ urn:li:sponsoredAccount:516986977 ]</example>
    [UIHint(nameof(SearchMethod), "", SearchAttribute.FindAdAccountUsersByAccounts)]
    [DefaultValue("")]
    public AccountUrn[] AccountUrns { get; set; }

    /// <summary>
    /// Searches for users with person URN.
    /// </summary>
    /// <example>urn:li:person:_mVMF2Kp8p</example>
    [UIHint(nameof(SearchMethod), "", SearchAttribute.GetAdAccountUser)]
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string UserUrn { get; set; }
}