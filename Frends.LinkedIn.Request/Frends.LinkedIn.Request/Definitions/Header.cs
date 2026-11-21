namespace Frends.LinkedIn.Request.Definitions;

/// <summary>
/// Request header.
/// </summary>
public class Header
{
    /// <summary>
    /// Name of header.
    /// </summary>
    /// <example>X-Restli-Protocol-Version</example>
    public string Name { get; set; }

    /// <summary>
    /// Value of header.
    /// </summary>
    /// <example>2.0.0</example>
    public string Value { get; set; }
}
