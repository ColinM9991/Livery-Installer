using LiveryInstaller.UI.Models.INI;
using LiveryInstaller.UI.Models.Parsing;

namespace LiveryInstaller.UI.Services.Parsing;

/// <summary>
/// Represents a service that can deserialize aircraft configurations from a StreamReader.
/// </summary>
public interface IAircraftConfigurationParser
{
    /// <summary>
    /// Parses the aircraft configuration from the specified StreamReader.
    /// </summary>
    /// <param name="tokens">A stream of INI tokens.</param>
    /// <returns>A Task that represents the asynchronous operation and returns the deserialized AircraftConfiguration.</returns>
    Task<AircraftConfiguration> ParseAsync(IniTokenStream tokens);
}