namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Filter
{
    /// <summary>
    /// Ad Account Id.
    /// </summary>
    /// <example>506333826</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string AdAccountId { get; set; }

    /// <summary>
    /// Searches for an creatives by campaign Urn.
    /// </summary>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string CampaignUrn { get; set; }
}