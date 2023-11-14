namespace Frends.LinkedIn.SearchAdAnalytics.Definitions
{
    /// <summary>
    /// Time granularity of results.
    /// </summary>
    public enum TimeGranularity
    {
        /// <summary>
        /// Results grouped into a single result across the entire time range of the report.
        /// </summary>
        ALL,

        /// <summary>
        /// Results grouped by day.
        /// </summary>
        DAILY,

        /// <summary>
        /// Results grouped by month.
        /// </summary>
        MONTHLY,

        /// <summary>
        /// Results grouped by year.
        /// </summary>
        YEARLY,
    }
}
