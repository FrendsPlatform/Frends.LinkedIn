namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Filter object used with LinkedIn request.
/// </summary>
public class Campaigns
{
    /// <summary>
    /// Enables filtering with campaigns. 
    /// </summary>
    /// <example>CampaignStatus.ACTIVE</example>
    public Campaign[] Campaigns { get; set; }
}
