namespace LiveryInstaller.Library.Models.INI;

/// <summary>
/// Represents a section node in the aircraft configuration file.
/// </summary>
/// <param name="name">The name of the section.</param>
public class SectionNode(string name) : Node
{
    /// <summary>
    /// Gets or sets the name of the section.
    /// </summary>
    protected string SectionName { get; set; } = name;

    /// <summary>
    /// Converts the section node to a string.
    /// </summary>
    /// <returns>The section name as an INI formatted section.</returns>
    public override string ToString() => $"[{SectionName}]";
    
    /// <summary>
    /// Implicit conversion from SectionNode to string.
    /// </summary>
    /// <param name="section"></param>
    /// <returns>The section name as an INI formatted section.</returns>
    public static implicit operator string(SectionNode section) => section.ToString();
}