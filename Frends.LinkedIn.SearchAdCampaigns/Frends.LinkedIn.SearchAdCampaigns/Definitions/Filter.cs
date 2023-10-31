namespace Frends.LinkedIn.SearchAdCampaigns.Definitions
{
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
        /// Searches for accounts by reference.
        /// </summary>
        [DefaultValue("")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Reference { get; set; }

        /// <summary>
        /// Searches for an account by name.
        /// </summary>
        [DefaultValue("")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Name { get; set; }

        /// <summary>
        /// Searches for an account by ID.
        /// </summary>
        [DefaultValue("")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Id { get; set; }

        /// <summary>
        /// Filters for LInkedIn request.
        /// </summary>
        public TypeFilter[] TypeFilters { get; set; }

        /// <summary>
        /// Filters for LInkedIn request.
        /// </summary>
        public StatusFilter[] StatusFilters { get; set; }
    }
}