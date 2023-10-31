namespace Frends.LinkedIn.SearchAdAccounts.Definitions
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Input class usually contains parameters that are required.
    /// </summary>
    public class Filter
    {
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
        /// Filters for LInkedIn API request.
        /// </summary>
        public TypeFilter[] TypeFilters { get; set; }

        /// <summary>
        /// Filters for LInkedIn API request.
        /// </summary>
        public StatusFilter[] StatusFilters { get; set; }
    }
}