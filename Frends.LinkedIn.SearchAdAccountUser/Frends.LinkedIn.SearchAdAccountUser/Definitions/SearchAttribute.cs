namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

/// <summary>
/// Enumeration to determine how the user is searched in the LinkedIn REST API.
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
    /// Finds a specific user associated with an ad account.
    /// </summary>
    GetAdAccountUser,
}
