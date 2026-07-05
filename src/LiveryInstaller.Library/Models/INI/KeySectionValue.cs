namespace LiveryInstaller.Library.Models.INI;

/// <summary>
/// Represents a key-value pair in a section of the aircraft configuration file.
/// </summary>
/// <param name="key">The key of the key-value pair. </param>
/// <param name="value">The value of the key-value pair. </param>
public sealed class KeySectionValue(string key, string value) : SectionValue
{
    /// <summary>
    /// The key of the key-value pair.
    /// </summary>
    public string Key { get; } = key;
    
    /// <summary>
    /// The value of the key-value pair.
    /// </summary>
    public string Value { get; } = value;

    /// <summary>
    /// Converts the key-value pair to a string.
    /// </summary>
    /// <returns>The key-value pair as a string.</returns>
    public override string ToString() => $"{Key}={Value}";
}