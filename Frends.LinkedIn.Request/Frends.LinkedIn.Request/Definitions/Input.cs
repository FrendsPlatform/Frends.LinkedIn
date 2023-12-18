namespace Frends.LinkedIn.Request.Definitions
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Input class usually contains parameters that are required.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// The HTTP Method to be used with the request.
        /// </summary>
        /// <example>GET</example>
        [DefaultValue(Method.GET)]
        public Method Method { get; set; }

        /// <summary>
        /// Something that will be repeated.
        /// </summary>
        /// <example>Some example of the expected value.</example>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("https://api.linkedin.com/rest/")]
        public string Url { get; set; }

        /// <summary>
        /// The message text to be sent with the request.
        /// </summary>
        /// <example>{ "Body": "Message" }</example>
        [UIHint(nameof(Method), "", Method.POST, Method.DELETE, Method.PATCH, Method.PUT)]
        public string Message { get; set; }

        /// <summary>
        /// List of HTTP headers to be added to the request.
        /// </summary>
        /// <example>Name: Header, Value: HeaderValue</example>
        public Header[] Headers { get; set; }
    }
}