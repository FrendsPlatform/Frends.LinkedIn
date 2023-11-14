namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Filter
{
    /// <summary>
    /// Specifies the Finder used in the request.
    /// Statistics allows up to three pivots in the request.
    /// Analytics finder is used when specifying a single pivot.
    /// </summary>
    /// <example>Finder.Statistics</example>
    [DefaultValue(Finder.Analytics)]
    public Finder Finder { get; set; }

    /// <summary>
    /// Pivot of results, by which each report data point is grouped. Optional.
    /// </summary>
    /// <example>Pivot.COMPANY</example>
    [UIHint(nameof(Finder), "", Finder.Analytics)]
    public Pivot Pivot { get; set; }

    /// <summary>
    /// Pivot of results, by which each report data point is grouped. Optional.
    /// </summary>
    /// <example>[ Pivot.COMPANY, Pivot.ACCOUNT]</example>
    [UIHint(nameof(Finder), "", Finder.Statistics)]
    public Pivot[] Pivots { get; set; }

    /// <summary>
    /// Represents the inclusive start time range of the analytics. https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers
    /// </summary>
    /// <example>12-10-2023</example>
    public string StartDate { get; set; }

    /// <summary>
    /// Represents the inclusive end time range of the analytics. Must be after start time if it's present. Optional. https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers
    /// </summary>
    /// <example>12-10-2023</example>
    public string EndDate { get; set; }

    /// <summary>
    /// Time granularity of results.
    /// </summary>
    /// <example>TimeGranularity.ALL</example>
    public TimeGranularity TimeGranularity { get; set; }

    /// <summary>
    /// Match result by campaign facets.
    /// </summary>
    /// <example>[ urn:li:sponsoredCampaign:1234567, urn:li:sponsoredCampaign:1234568 ]</example>
    public string[] Campaigns { get; set; }

    /// <summary>
    /// Match result by creative facets.
    /// </summary>
    /// <example>[  ]</example>
    public string[] Creatives { get; set; }

    /// <summary>
    /// LinkedIn requires you to issue specific metrics in a request using fields. You can request up to 20 metrics.
    /// You can find all metrics available here: https://learn.microsoft.com/en-us/linkedin/marketing/integrations/ads-reporting/ads-reporting?view=li-lms-2023-10&tabs=http#metrics-available
    /// </summary>
    /// <example>[ clicks, comments, actionClicks ]</example>
    public string[] Metrics { get; set; }
}