namespace Frends.LinkedIn.SearchAdAnalytics.Definitions
{
    /// <summary>
    /// Pivot of results, by which each report data point is grouped.
    /// </summary>
    public enum Pivot
    {
        /// <summary>
        /// Pivot will not be added to the request.
        /// </summary>
        NONE,

        /// <summary>
        /// Group results by advertiser's company.
        /// </summary>
        COMPANY,

        /// <summary>
        /// Group results by account.
        /// </summary>
        ACCOUNT,

        /// <summary>
        /// Group results by sponsored share.
        /// </summary>
        SHARE,

        /// <summary>
        /// Group results by campaign.
        /// </summary>
        CAMPAIGN,

        /// <summary>
        /// Group results by creative.
        /// </summary>
        CREATIVE,

        /// <summary>
        /// Group results by campaign group.
        /// </summary>
        CAMPAIGN_GROUP,

        /// <summary>
        /// Group results by conversion.
        /// </summary>
        CONVERSION,

        /// <summary>
        /// The element row in the conversation will be the information for each individual node of the conversation tree.
        /// </summary>
        CONVERSATION_NODE,

        /// <summary>
        /// Used actionClicks are deaggregated and reported at the Node Button level. The second value of the pivot_values will be the index of the button in the node.
        /// </summary>
        CONVERSATION_NODE_OPTION_INDEX,

        /// <summary>
        /// Group results by serving location, onsite or offsite.
        /// </summary>
        SERVING_LOCATION,

        /// <summary>
        /// Group results by the index of where a card appears in a carousel ad creative. Metrics are based on the index of the card at the time when the user's action (impression, click, etc.) happened on the creative (Carousel creatives only).
        /// </summary>
        CARD_INDEX,

        /// <summary>
        /// Group results by member company size.
        /// </summary>
        MEMBER_COMPANY_SIZE,

        /// <summary>
        /// Group results by member industry.
        /// </summary>
        MEMBER_INDUSTRY,

        /// <summary>
        /// Group results by member seniority.
        /// </summary>
        MEMBER_SENIORITY,

        /// <summary>
        /// Group results by member job title.
        /// </summary>
        MEMBER_JOB_TITLE,

        /// <summary>
        /// Group results by member job function.
        /// </summary>
        MEMBER_JOB_FUNCTION,

        /// <summary>
        /// Group results by member country.
        /// </summary>
        MEMBER_COUNTRY_V2,

        /// <summary>
        /// Group results by member region.
        /// </summary>
        MEMBER_REGION_V2,

        /// <summary>
        /// Group results by member company.
        /// </summary>
        MEMBER_COMPANY,

        /// <summary>
        /// Group results by placement.
        /// </summary>
        PLACEMENT_NAME,

        /// <summary>
        /// Group results by the device type the ad made an impression on. Reach metrics and conversion metrics will not be available when this pivot is used.
        /// </summary>
        IMPRESSION_DEVICE_TYPE,
    }
}
