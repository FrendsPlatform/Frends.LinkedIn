namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Filter object used with LinkedIn request.
/// </summary>
public class TypeFilter
{
    /// <summary>
    /// Campaign type filter.
    /// </summary>
    /// <example>AccountType.BUSINESS</example>
    public AccountType AccountType { get; set; }
}
