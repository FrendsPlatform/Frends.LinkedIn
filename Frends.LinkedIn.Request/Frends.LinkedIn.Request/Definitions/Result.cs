namespace Frends.LinkedIn.Request.Definitions
{
    using System.Collections.Generic;

    /// <summary>
    /// Result of the request.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates if the task completed successfully.
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Body of response. Response body contains two properties: paging and elements.
        /// </summary>
        /// <example>{"paging": {"start": 0, "count":10, "links": [], "total": 1}, "elements": [{"totalBudget": ...}]}</example>
        public dynamic Body { get; set; }

        /// <summary>
        /// Headers of response
        /// </summary>
        /// <example>{[ "content-type": "application/json", ... ]}</example>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Status code of the response.
        /// </summary>
        /// <example>200</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error that occurred during task execution.
        /// </summary>
        /// <example>object { string Message, Exception AdditionalInfo }</example>
        public Error Error { get; set; }
    }
}
