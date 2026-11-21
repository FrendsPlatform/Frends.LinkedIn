namespace Frends.LinkedIn.Request.Definitions
{
    using System;

    /// <summary>
    /// Error details returned when a LinkedIn request fails.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Summary of the error.
        /// </summary>
        /// <example>LinkedIn request failed with status code 401</example>
        public string Message { get; set; }

        /// <summary>
        /// Additional information about the error.
        /// </summary>
        /// <example>object { Exception AdditionalInfo }</example>
        public Exception AdditionalInfo { get; set; }
    }
}
