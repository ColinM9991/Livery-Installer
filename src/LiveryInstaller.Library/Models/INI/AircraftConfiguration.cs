namespace LiveryInstaller.Library.Models.INI;

/// <summary>
/// Represents the configuration of an aircraft.
/// </summary>
public class AircraftConfiguration
{
    private readonly List<SectionNode> _sections = [];

    /// <summary>
    /// Gets or sets the header of the configuration.
    /// </summary>
    public HeaderNode Header { get; set; }

    /// <summary>
    /// Gets the sections in the configuration.
    /// </summary>
    /// <remarks>
    /// The flight sim sections are listed first, followed by the other sections.
    /// </remarks>
    public IReadOnlyList<SectionNode> Sections
    {
        get
        {
            var flightSimSections = GetSections<FlightSimSectionNode>().ToList();
            var otherSections = _sections.Except(flightSimSections);

            return flightSimSections.Concat(otherSections).ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Adds a section to the configuration.
    /// </summary>
    /// <param name="section">The section to add.</param>
    public void AddSection(SectionNode section)
    {
        _sections.Add(section);
    }

    /// <summary>
    /// Removes the given section from the configuration.
    /// </summary>
    /// <remarks>
    /// If the section is a flight sim section, the indexes of all subsequent flight sim sections will be decremented.
    /// </remarks>
    /// <param name="section">The section to remove.</param>
    public void RemoveSection(SectionNode section)
    {
        if (section is FlightSimSectionNode flightSimSection)
        {
            var index = flightSimSection.GetIndex();

            foreach (var s in GetSections<FlightSimSectionNode>().Where(x => x.GetIndex() > index))
            {
                s.SetIndex(s.GetIndex() - 1);
            }
        }

        _sections.Remove(section);
    }

    /// <summary>
    /// Merges the fltsim sections from the other configuration into this one.
    /// </summary>
    /// <param name="other">The other configuration to merge from.</param>
    public void Merge(AircraftConfiguration other)
    {
        foreach (var section in other.GetSections<FlightSimSectionNode>())
        {
            var nextIndex = GetNextFlightSimIndex();
            section.SetIndex(nextIndex);
            AddSection(section);
        }
    }

    public FlightSimSectionNode GetFirstFlightSimSection() => GetSections<FlightSimSectionNode>().FirstOrDefault();
    
    /// <summary>
    /// Finds the flight sim section with the given ATC ID.
    /// </summary>
    /// <param name="atcId">The ATC ID to find.</param>
    /// <returns>The flight sim section with the given ATC ID, or null if not found. </returns>
    public FlightSimSectionNode FindFlightSimSection(string atcId)
    {
        return GetSections<FlightSimSectionNode>()
            .FirstOrDefault(x => x.Values.Any(y => string.Equals(y, $"atc_id={atcId}")));
    }

    /// <summary>
    /// Gets the next available index for a flight sim section.
    /// </summary>
    /// <remarks>
    /// The index is based on the maximum index of the flight sim sections. If no sections exist, the index is 0.
    /// </remarks>
    /// <returns>The next available index for a flight sim section. </returns>
    private int GetNextFlightSimIndex()
    {
        var flightSimSections = GetSections<FlightSimSectionNode>().ToList();
        
        return flightSimSections.Count == 0
            ? 0
            : flightSimSections.Max(s => s.GetIndex()) + 1;
    }

    /// <summary>
    /// Gets the sections of the given type.
    /// </summary>
    /// <typeparam name="T">The type of section to get. </typeparam>
    /// <returns>The sections of the given type. </returns>
    private IEnumerable<T> GetSections<T>() where T : SectionNode => _sections.OfType<T>();
}