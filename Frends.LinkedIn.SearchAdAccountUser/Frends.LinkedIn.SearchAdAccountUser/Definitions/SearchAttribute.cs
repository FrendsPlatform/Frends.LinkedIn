namespace Frends.LinkedIn.SearchAdAccountUser.Definitions
{
    /// <summary>
    /// Enumeration to determine how the user if searched from LinkedIn Rest API.
    /// </summary>
    public enum SearchAttribute
    {
        /// <summary>
        /// Finds all ad accounts that an authenticated user has access to.
        /// </summary>
        FindAdAccountsByAuthenticatedUser,

        /// <summary>
        /// Finds all users associated with a specific ad account.
        /// </summary>
        FindAdAccountUsersByAccounts,

        /// <summary>
        /// Fetching a specific ad account user requires both account and user params to look up an existing ad account user.
        /// </summary>
        GetAdAccountUser,
    }
}