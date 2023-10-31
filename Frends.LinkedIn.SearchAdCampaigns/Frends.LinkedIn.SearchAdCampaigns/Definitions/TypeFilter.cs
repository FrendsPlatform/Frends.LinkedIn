namespace Frends.LinkedIn.SearchAdCampaigns.Definitions
{
    /// <summary>
    /// Filter object used with LinkedIn request.
    /// </summary>
    public class TypeFilter
    {
        /// <summary>
        /// Campaign type filter.
        /// </summary>
        /// <example>CampaignType.TEXT_AD</example>
        public CampaignType CampaignType { get; set; }
    }
}
