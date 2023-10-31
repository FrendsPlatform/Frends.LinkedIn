namespace Frends.LinkedIn.SearchAdAccounts.Definitions
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Options parameter.
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
        /// Bearer token to be used for requests. Token will be added as Authorization header.
        /// </summary>
        /// <example>xRqjX_9QXZlvRL6HZHJvbrubhvceV4NFccANh6RwndmAJDkPfP...</example>
        [PasswordPropertyText]
        [DisplayFormat(DataFormatString = "Text")]
        public string Token { get; set; }

        /// <summary>
        /// Timeout in seconds to be used for the connection and operation.
        /// </summary>
        /// <example>30</example>
        [DefaultValue(30)]
        public int ConnectionTimeoutSeconds { get; set; }

        /// <summary>
        /// If FollowRedirects is set to false, all responses with an HTTP status code from 300 to 399 is returned to the application.
        /// </summary>
        /// <example>true</example>
        [DefaultValue(true)]
        public bool FollowRedirects { get; set; }

        /// <summary>
        /// Do not throw an exception on certificate error.
        /// </summary>
        /// <example>true</example>
        public bool AllowInvalidCertificate { get; set; }

        /// <summary>
        /// Some Api's return faulty content-type charset header. This setting overrides the returned charset.
        /// </summary>
        /// <example>true</example>
        public bool AllowInvalidResponseContentTypeCharSet { get; set; }

        /// <summary>
        /// Throw exception if return code of request is not successfull.
        /// </summary>
        /// <example>true</example>
        public bool ThrowExceptionOnErrorResponse { get; set; }

        /// <summary>
        /// If set to false, cookies must be handled manually. Defaults to true.
        /// </summary>
        /// <example>true</example>
        [DefaultValue(true)]
        public bool AutomaticCookieHandling { get; set; } = true;
    }
}