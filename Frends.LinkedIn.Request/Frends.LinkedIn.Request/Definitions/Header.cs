namespace Frends.LinkedIn.Request.Definitions
{
    /// <summary>
    /// Request header.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Gets or sets the header name.
        /// </summary>
        /// <example>X-Restli-Protocol-Version</example>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the header value.
        /// </summary>
        /// <example>2.0.0</example>
        public string Value { get; set; }
    }
}
