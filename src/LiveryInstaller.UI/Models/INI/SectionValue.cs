namespace LiveryInstaller.UI.Models.INI;

/// <summary>
/// Represents a value in a section of the aircraft configuration file.
/// </summary>
public abstract class SectionValue
{
    /// <summary>
    /// Converts the value to a string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The value as a string.</returns>
    public static implicit operator string(SectionValue value) => value.ToString();
}