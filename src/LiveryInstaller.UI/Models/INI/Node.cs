namespace LiveryInstaller.UI.Models.INI;

/// <summary>
/// Represents a node in the aircraft configuration file.
/// </summary>
public abstract class Node
{
    /// <summary>
    /// Gets the values of the node.
    /// </summary>
    public ICollection<SectionValue> Values { get; } = new List<SectionValue>();
}