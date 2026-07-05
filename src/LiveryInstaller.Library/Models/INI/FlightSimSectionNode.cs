namespace LiveryInstaller.Library.Models.INI;

/// <summary>
/// Represents a fltsim section node in the aircraft configuration file.
/// </summary>
public class FlightSimSectionNode : SectionNode
{
    /// <summary>
    /// Creates a new instance of the <see cref="FlightSimSectionNode"/> class.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    public FlightSimSectionNode(string sectionName) : base(sectionName)
    {
        var indexString = sectionName.Split('.')[1];
        var isIndexable = int.TryParse(indexString, out var index);
        
        Index = isIndexable ? index : -1;
    }

    /// <summary>
    /// The index of the section.
    /// </summary>
    private int Index { get; set; }
    
    /// <summary>
    /// Gets the index of the section.
    /// </summary>
    /// <returns>The index of the section.</returns>
    public int GetIndex() => Index;
    
    /// <summary>
    /// Sets the index of the section.
    /// </summary>
    /// <param name="index">The index to set.</param>
    public void SetIndex(int index)
    {
        Index = index;
        SectionName = $"fltsim.{Index}";
    }
    
    public string GetValue(string key) => Values.OfType<KeySectionValue>().FirstOrDefault(x => x.Key == key)?.Value;
}