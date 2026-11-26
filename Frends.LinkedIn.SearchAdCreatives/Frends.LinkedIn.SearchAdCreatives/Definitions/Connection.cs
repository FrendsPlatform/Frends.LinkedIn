using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// LinkedIn authentication parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// LinkedIn access token used to authorize the request.
    /// </summary>
    /// <example>linkedin-access-token</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string Token { get; set; }

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
    /// Whether cookies are handled automatically.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool AutomaticCookieHandling { get; set; } = true;
}
