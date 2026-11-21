namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Status filter used in a LinkedIn account search.
/// </summary>
public class StatusFilter
{
    /// <summary>
    /// Gets or sets the account status filter.
    /// </summary>
    /// <example>AccountStatus.ACTIVE</example>
    public AccountStatus AccountStatus { get; set; }
}
