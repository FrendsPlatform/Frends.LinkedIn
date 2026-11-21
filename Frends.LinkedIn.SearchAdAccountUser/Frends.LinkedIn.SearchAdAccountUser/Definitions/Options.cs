using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

/// <summary>
/// Additional task parameters.
/// </summary>
public class Options
{
    /// <summary>
    /// Version of the API used in the request.
    /// API versions should have date format as YYYYMM or YYYYMM.RR where RR is the revision.
    /// More information: https://learn.microsoft.com/en-us/linkedin/marketing/versioning?view=li-lms-2023-09
    /// </summary>
    /// <example>202309</example>
    [DefaultValue("202309")]
    [DisplayFormat(DataFormatString = "Text")]
    public string LinkedInAPIVersion { get; set; }

    /// <summary>
    /// Timeout in seconds to be used for the connection and operation.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int ConnectionTimeoutSeconds { get; set; }

    /// <summary>
    /// If FollowRedirects is set to false, all responses with an HTTP status code from 300 to 399 are returned to the application.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool FollowRedirects { get; set; } = true;

    /// <summary>
    /// Do not throw an exception on certificate error.
    /// </summary>
    /// <example>false</example>
    public bool AllowInvalidCertificate { get; set; }

    /// <summary>
    /// Some APIs return a faulty content-type charset header. This setting overrides the returned charset.
    /// </summary>
    /// <example>false</example>
    public bool AllowInvalidResponseContentTypeCharSet { get; set; }

    /// <summary>
    /// If set to false, cookies must be handled manually. Defaults to true.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool AutomaticCookieHandling { get; set; } = true;

    /// <summary>
    /// Whether to throw an error on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Overrides the error message on failure.
    /// </summary>
    /// <example>LinkedIn ad account user search failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
