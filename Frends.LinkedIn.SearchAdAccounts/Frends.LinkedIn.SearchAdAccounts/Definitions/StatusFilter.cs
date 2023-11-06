namespace Frends.LinkedIn.SearchAdAccounts.Definitions
{
    /// <summary>
    /// Filter object used with LinkedIn request.
    /// </summary>
    public class StatusFilter
    {
        /// <summary>
        /// Account status filter.
        /// </summary>
        /// <example>AccountStatus.ACTIVE</example>
        public AccountStatus AccountStatus { get; set; }
    }
}
