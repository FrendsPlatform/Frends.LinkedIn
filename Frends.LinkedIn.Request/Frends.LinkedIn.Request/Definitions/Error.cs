namespace Frends.LinkedIn.Request.Definitions
{
    using System;

    /// <summary>
    /// Error details returned when a LinkedIn request fails.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets the summary of the error.
        /// </summary>
        /// <example>LinkedIn request failed with status code 401</example>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets additional information about the error.
        /// </summary>
        /// <example>object { Exception AdditionalInfo }</example>
        public Exception AdditionalInfo { get; set; }
    }
}
