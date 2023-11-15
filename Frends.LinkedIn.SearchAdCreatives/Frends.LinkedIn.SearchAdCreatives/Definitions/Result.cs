namespace Frends.LinkedIn.SearchAdCreatives.Definitions
{
    using System.Collections.Generic;

    /// <summary>
    /// Result class containing properties with private setters.
    /// </summary>
    public class Result
    {
        internal Result(object body, Dictionary<string, string> headers, int statusCode)
        {
            Body = body;
            Headers = headers;
            StatusCode = statusCode;
        }

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
        /// Statuscode of response
        /// </summary>
        /// <example>200</example>
        public int StatusCode { get; private set; }
    }
}
