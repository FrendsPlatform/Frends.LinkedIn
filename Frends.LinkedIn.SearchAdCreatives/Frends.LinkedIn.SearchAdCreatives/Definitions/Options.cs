using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Additional request and error-handling options.
/// </summary>
public class Options
{
    /// <summary>
    /// API version used in the request.
    /// </summary>
    /// <example>202309</example>
    [DefaultValue("202309")]
    [DisplayFormat(DataFormatString = "Text")]
    public string LinkedInAPIVersion { get; set; }

    /// <summary>
    /// Request timeout in seconds.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int ConnectionTimeoutSeconds { get; set; }

    /// <summary>
    /// Whether HTTP redirects are followed.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool FollowRedirects { get; set; }

    /// <summary>
    /// Whether certificate validation errors are ignored.
    /// </summary>
    /// <example>false</example>
    public bool AllowInvalidCertificate { get; set; }

    /// <summary>
    /// Whether to tolerate invalid response content-type charset values.
    /// </summary>
    /// <example>false</example>
    public bool AllowInvalidResponseContentTypeCharSet { get; set; }

    /// <summary>
    /// Whether a non-success HTTP response causes the task to fail.
    /// </summary>
    /// <example>true</example>
    public bool ThrowExceptionOnErrorResponse { get; set; }

    /// <summary>
    /// Whether cookies are handled automatically.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool AutomaticCookieHandling { get; set; } = true;

    /// <summary>
    /// Whether task failures are thrown instead of returned in the result.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Optional message prefix used when handling a task failure.
    /// </summary>
    /// <example>LinkedIn creative search failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
