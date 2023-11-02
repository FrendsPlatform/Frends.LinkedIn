namespace Frends.LinkedIn.SearchAdAnalytics.Definitions
{
    /// <summary>
    /// Specifies the Finder used in the request.
    /// </summary>
    public enum Finder
    {
        /// <summary>
        /// Statistics allows up to three pivots in the request.
        /// </summary>
        Statistics,

        /// <summary>
        /// Analytics finder is used when specifying a single pivot.
        /// </summary>
        Analytics,
    }
}
