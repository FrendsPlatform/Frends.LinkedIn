namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Campaign filter used to search for creatives.
/// </summary>
public class AdCampaign
{
    /// <summary>
    /// Campaign URN used as a filter.
    /// </summary>
    /// <example>urn:li:sponsoredCampaign:360035215</example>
    public string AdCampaignUrn { get; set; }
}
