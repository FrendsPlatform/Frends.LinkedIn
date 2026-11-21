// Copyright (c) Frends. All rights reserved.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

/// <summary>
/// Input parameters used to filter LinkedIn ad analytics.
/// </summary>
public class Input
{
    /// <summary>
    /// Gets or sets the finder used in the request. Statistics supports up to three pivots, while analytics uses a
    /// single pivot.
    /// </summary>
    /// <example><c>Finder.Statistics</c>.</example>
    [DefaultValue(Finder.Analytics)]
    public Finder Finder { get; set; }

    /// <summary>
    /// Gets or sets the pivot used to group each report data point. This parameter is optional.
    /// </summary>
    /// <example><c>Pivot.COMPANY</c>.</example>
    [UIHint(nameof(Finder), "", Finder.Analytics)]
    public Pivot Pivot { get; set; }

    /// <summary>
    /// Gets or sets the pivots used to group statistics results. This parameter is optional.
    /// </summary>
    /// <example><c>[ Pivot.COMPANY, Pivot.ACCOUNT]</c>.</example>
    [UIHint(nameof(Finder), "", Finder.Statistics)]
    public Pivot[] Pivots { get; set; }

    /// <summary>
    /// Gets or sets the inclusive start date for the analytics time range. See
    /// https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers.
    /// </summary>
    /// <example><c>2023-12-10</c>.</example>
    public string StartDate { get; set; }

    /// <summary>
    /// Gets or sets the inclusive end date for the analytics time range. If provided, it must be after the start date.
    /// See https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers.
    /// </summary>
    /// <example><c>2023-12-11</c>.</example>
    public string EndDate { get; set; }

    /// <summary>
    /// Gets or sets the time granularity of the results.
    /// </summary>
    /// <example><c>TimeGranularity.ALL</c>.</example>
    public TimeGranularity TimeGranularity { get; set; }

    /// <summary>
    /// Gets or sets the campaign facets used to filter results.
    /// </summary>
    /// <example><c>[ urn:li:sponsoredCampaign:1234567, urn:li:sponsoredCampaign:1234568 ]</c>.</example>
    public string[] Campaigns { get; set; }

    /// <summary>
    /// Gets or sets the creative facets used to filter results.
    /// </summary>
    /// <example><c>urn:li:sponsoredCreative:1234567</c>.</example>
    public string[] Creatives { get; set; }

    /// <summary>
    /// Gets or sets the metrics to request. LinkedIn requires specific metrics in the fields parameter and allows up
    /// to 20 metrics.
    /// See https://learn.microsoft.com/en-us/linkedin/marketing/integrations/ads-reporting/ads-reporting?view=li-lms-2023-10&amp;tabs=http#metrics-available.
    /// </summary>
    /// <example><c>[ clicks, comments, actionClicks ]</c>.</example>
    public string[] Metrics { get; set; }
}