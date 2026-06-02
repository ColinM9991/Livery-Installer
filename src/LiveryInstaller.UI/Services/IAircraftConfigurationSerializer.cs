using System.IO;
using LiveryInstaller.UI.Models.INI;

namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can serialize aircraft configurations to a StreamWriter.
/// </summary>
public interface IAircraftConfigurationSerializer
{
    /// <summary>
    /// Serializes the given aircraft configuration to the specified StreamWriter.
    /// </summary>
    /// <param name="configuration">The aircraft configuration to serialize.</param>
    /// <param name="streamWriter">The StreamWriter to write the serialized configuration to.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task SerializeAsync(AircraftConfiguration configuration, StreamWriter streamWriter);
}