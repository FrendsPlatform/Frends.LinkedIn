namespace Frends.LinkedIn.Request.Definitions
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Options parameter.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the version of the API used in the request.
        /// API versions should have date format as YYYYMM or YYYYMM.RR where RR is the revision.
        /// More information: https://learn.microsoft.com/en-us/linkedin/marketing/versioning?view=li-lms-2023-09.
        /// </summary>
        /// <example>202309</example>
        [DefaultValue("202309")]
        [DisplayFormat(DataFormatString = "Text")]
        public string LinkedInAPIVersion { get; set; }

        /// <summary>
        /// Gets or sets the access token to use for requests. It is added as an Authorization header.
        /// </summary>
        /// <example>xRqjX_9QXZlvRL6HZHJvbrubhvceV4NFccANh6RwndmAJDkPfP...</example>
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
        /// Gets or sets a value indicating whether responses with HTTP status codes from 300 to 399 are returned to the application.
        /// </summary>
        /// <example>true</example>
        [DefaultValue(true)]
        public bool FollowRedirects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore certificate errors.
        /// </summary>
        /// <example>true</example>
        public bool AllowInvalidCertificate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to override invalid response content-type charset values.
        /// </summary>
        /// <example>true</example>
        public bool AllowInvalidResponseContentTypeCharSet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw an exception when the response status code is unsuccessful.
        /// </summary>
        /// <example>true</example>
        public bool ThrowExceptionOnErrorResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to handle cookies automatically. Defaults to true.
        /// </summary>
        /// <example>true</example>
        [DefaultValue(true)]
        public bool AutomaticCookieHandling { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to throw an exception when the request fails.
        /// </summary>
        /// <example>true</example>
        [DefaultValue(true)]
        public bool ThrowErrorOnFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets a custom message to include when the request fails.
        /// </summary>
        /// <example>LinkedIn request failed</example>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("")]
        public string ErrorMessageOnFailure { get; set; } = string.Empty;
    }
}
