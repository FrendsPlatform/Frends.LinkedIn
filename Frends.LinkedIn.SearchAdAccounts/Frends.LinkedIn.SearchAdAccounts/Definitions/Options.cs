// <copyright file="Options.cs" company="Frends">
// Copyright (c) Frends. All rights reserved.
// </copyright>

namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Additional request and failure-handling options.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets the API version used in the request. API versions use the format YYYYMM or YYYYMM.RR.
    /// </summary>
    /// <example>202309</example>
    [DefaultValue("202309")]
    [DisplayFormat(DataFormatString = "Text")]
    public string LinkedInAPIVersion { get; set; }

    /// <summary>
    /// Gets or sets the access token used in the Authorization header.
    /// </summary>
    /// <example>A valid LinkedIn access token</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the timeout in seconds for the connection and operation.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int ConnectionTimeoutSeconds { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether redirects are returned to the application instead of followed.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool FollowRedirects { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the request can continue when the server certificate is invalid.
    /// </summary>
    /// <example>false</example>
    public bool AllowInvalidCertificate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to override faulty response content-type charset values.
    /// </summary>
    /// <example>false</example>
    public bool AllowInvalidResponseContentTypeCharSet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception when LinkedIn returns a non-success status code.
    /// </summary>
    /// <example>true</example>
    public bool ThrowExceptionOnErrorResponse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether cookies are handled automatically.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool AutomaticCookieHandling { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to throw an error when task execution fails.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets an optional prefix for the error message when task execution fails.
    /// </summary>
    /// <example>LinkedIn ad account search failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
