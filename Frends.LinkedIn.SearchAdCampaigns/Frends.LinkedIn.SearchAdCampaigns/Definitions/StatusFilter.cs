namespace Frends.LinkedIn.SearchAdCampaigns.Definitions
{
    /// <summary>
    /// Filter object used with LinkedIn request.
    /// </summary>
    public class StatusFilter
    {
        /// <summary>
        /// Campaign status filter.
        /// </summary>
        /// <example>CampaignStatus.ACTIVE</example>
        public CampaignStatus CampaignStatus { get; set; }
    }
}
