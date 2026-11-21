// Copyright (c) Frends. All rights reserved.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

/// <summary>
/// Options parameter.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets the API version used in the request. API versions use the YYYYMM or YYYYMM.RR format.
    /// See https://learn.microsoft.com/en-us/linkedin/marketing/versioning?view=li-lms-2023-09.
    /// </summary>
    /// <example><c>202309</c>.</example>
    [DefaultValue("202309")]
    [DisplayFormat(DataFormatString = "Text")]
    public string LinkedInAPIVersion { get; set; }

    /// <summary>
    /// Gets or sets the access token used for requests. The token is added as an Authorization header.
    /// </summary>
    /// <example><c>sample-access-token</c>.</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the connection and operation timeout, in seconds.
    /// </summary>
    /// <example><c>30</c>.</example>
    [DefaultValue(30)]
    public int ConnectionTimeoutSeconds { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to follow redirects. When false, responses with a 3xx status code are
    /// returned to the application.
    /// </summary>
    /// <example><c>true</c>.</example>
    [DefaultValue(true)]
    public bool FollowRedirects { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore certificate errors.
    /// </summary>
    /// <example><c>true</c>.</example>
    public bool AllowInvalidCertificate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to override a faulty response content-type charset.
    /// </summary>
    /// <example><c>true</c>.</example>
    public bool AllowInvalidResponseContentTypeCharSet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception when the request returns a non-success status code.
    /// </summary>
    /// <example><c>true</c>.</example>
    public bool ThrowExceptionOnErrorResponse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to handle cookies automatically. Defaults to true.
    /// </summary>
    /// <example><c>true</c>.</example>
    [DefaultValue(true)]
    public bool AutomaticCookieHandling { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to throw an error when the task fails.
    /// </summary>
    /// <example><c>true</c>.</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message to use when the task fails.
    /// </summary>
    /// <example><c>LinkedIn ad analytics request failed.</c>.</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
