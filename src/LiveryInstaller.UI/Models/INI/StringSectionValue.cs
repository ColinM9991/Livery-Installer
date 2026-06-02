namespace LiveryInstaller.UI.Models.INI;

/// <summary>
/// Represents a string value in a section of the aircraft configuration file.
/// </summary>
/// <param name="value"></param>
public class StringSectionValue(string value) : SectionValue
{
    /// <summary>
    /// The value of the section value.
    /// </summary>
    private string Value { get; } = value;

    /// <summary>
    /// Converts the value to a string.
    /// </summary>
    /// <returns>The value as a string.</returns>
    public override string ToString() => Value;
}