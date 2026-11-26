namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Creative filter used to search for creatives.
/// </summary>
public class AdCreativeFilter
{
    /// <summary>
    /// Creative URN used as a filter.
    /// </summary>
    /// <example>urn:li:sponsoredCreative:119962155</example>
    public string AdCreativeUrn { get; set; }
}
