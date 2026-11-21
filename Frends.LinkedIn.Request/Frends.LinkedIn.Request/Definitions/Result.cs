namespace Frends.LinkedIn.Request.Definitions
{
    using System.Collections.Generic;

    /// <summary>
    /// Result class usually contains properties of the return object.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class for a successful response.
        /// </summary>
        /// <param name="body">The response body.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        internal Result(object body = null, Dictionary<string, string> headers = null, int statusCode = 0)
        {
            this.Success = true;
            this.Body = body;
            this.Headers = headers;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Whether the request completed successfully.
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Body of response. Response body contains two properties: paging and elements.
        /// </summary>
        /// <example>{"paging": {"start": 0, "count":10, "links": [], "total": 1}, "elements": [{"totalBudget": ...}]}</example>
        public dynamic Body { get; private set; }

        /// <summary>
        /// Headers of response
        /// </summary>
        /// <example>{[ "content-type": "application/json", ... ]}</example>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// Status code of the response.
        /// </summary>
        /// <example>200</example>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Error details when the request fails.
        /// </summary>
        /// <example>null</example>
        public Error Error { get; set; }
    }
}
