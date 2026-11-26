using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Search criteria for LinkedIn ad creatives.
/// </summary>
public class Input
{
    /// <summary>
    /// Ad account ID required for the search.
    /// </summary>
    /// <example>506333826</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string AdAccountId { get; set; }

    /// <summary>
    /// Set to true to search for one creative by its URN.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool GetSingleCreative { get; set; }

    /// <summary>
    /// URN of the creative to retrieve when searching for a single creative.
    /// </summary>
    /// <example>urn:li:sponsoredCreative:119962155</example>
    [UIHint(nameof(GetSingleCreative), "", true)]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string AdCreativeUrn { get; set; }

    /// <summary>
    /// Campaign URNs used to filter the search.
    /// </summary>
    /// <example>[ AdCampaign { AdCampaignUrn: urn:li:sponsoredCampaign:360035215 } ]</example>
    public AdCampaign[] AdCampaignUrns { get; set; }

    /// <summary>
    /// Creative URNs used to filter the search.
    /// </summary>
    /// <example>[ AdCreativeFilter { AdCreativeUrn: urn:li:sponsoredCreative:119962155 } ]</example>
    public AdCreativeFilter[] AdCreativesUrns { get; set; }
}
