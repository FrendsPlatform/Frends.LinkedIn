using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCampaigns.Definitions;

/// <summary>
/// Essential parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Ad Account Id.
    /// </summary>
    /// <example>506333826</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string AdAccountId { get; set; }

    /// <summary>
    /// Searches for campaigns by reference.
    /// </summary>
    /// <example>urn:li:organization:12345</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Reference { get; set; }

    /// <summary>
    /// Searches for a campaign by name.
    /// </summary>
    /// <example>Summer campaign</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Name { get; set; }

    /// <summary>
    /// Searches for a campaign by ID.
    /// </summary>
    /// <example>277937223</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Id { get; set; }

    /// <summary>
    /// Campaign type filters for LinkedIn request.
    /// </summary>
    /// <example>[ { CampaignType.TEXT_AD } ]</example>
    public TypeFilter[] TypeFilters { get; set; }

    /// <summary>
    /// Campaign status filters for LinkedIn request.
    /// </summary>
    /// <example>[ { CampaignStatus.ACTIVE } ]</example>
    public StatusFilter[] StatusFilters { get; set; }
}
