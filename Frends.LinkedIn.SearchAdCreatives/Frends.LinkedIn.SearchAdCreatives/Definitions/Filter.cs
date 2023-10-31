namespace Frends.LinkedIn.SearchAdCreatives.Definitions
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Input class usually contains parameters that are required.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Ad Account Id. This is necessary for the search to work.
        /// </summary>
        /// <example>506333826</example>
        [DefaultValue("")]
        [DisplayFormat(DataFormatString = "Text")]
        public string AdAccountId { get; set; }

        /// <summary>
        /// If enabled, search is done for singular AdCreative with AdCreativeUrn.
        /// </summary>
        /// <example>true</example>
        [DefaultValue(false)]
        public bool GetSingleCreative { get; set; }

        /// <summary>
        /// Urn for the AdCreative to be searched.
        /// </summary>
        [UIHint(nameof(GetSingleCreative), "", true)]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("")]
        public string AdCreativeUrn { get; set; }

        /// <summary>
        /// Searches for an creatives by campaign Urns.
        /// </summary>
        /// <example>[ AdCampaign { urn:li:sponsoredCampaign:360035215 } ]</example>
        public AdCampaign[] AdCampaignUrns { get; set; }

        /// <summary>
        /// Searches for an creatives by creative Urns.
        /// </summary>
        /// <example>[ AdCreative { urn%3Ali%3AsponsoredCreative%3A119962155 } ]</example>
        public AdCreative[] AdCreativesUrns { get; set; }
    }
}