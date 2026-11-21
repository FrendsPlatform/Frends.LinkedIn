// Copyright (c) Frends. All rights reserved.

using System.Collections.Generic;

namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

/// <summary>
/// Result of the SearchAdAnalytics task.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets or sets a value indicating whether the task completed successfully.
    /// </summary>
    /// <example><c>true</c>.</example>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the response body. The body contains paging and elements properties.
    /// </summary>
    /// <example><c>{"paging": {"start": 0, "count":10, "links": [], "total": 1}, "elements": [{"totalBudget": 100}]}</c>.</example>
    public dynamic Body { get; set; }

    /// <summary>
    /// Gets or sets the response headers.
    /// </summary>
    /// <example><c>{[ "content-type": "application/json" ]}</c>.</example>
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    /// <example><c>200</c>.</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the error details when the task fails.
    /// </summary>
    /// <example><c>null</c>.</example>
    public Error Error { get; set; }
}
