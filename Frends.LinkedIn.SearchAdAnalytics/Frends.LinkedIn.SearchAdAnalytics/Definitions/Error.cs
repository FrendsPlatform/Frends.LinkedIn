// Copyright (c) Frends. All rights reserved.

using System;

namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

/// <summary>
/// Error that occurred during the task.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets a summary of the error.
    /// </summary>
    /// <example>LinkedIn returned HTTP status code 400.</example>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets additional exception information.
    /// </summary>
    /// <example><c>object { Exception AdditionalInfo }</c>.</example>
    public Exception AdditionalInfo { get; set; }
}
