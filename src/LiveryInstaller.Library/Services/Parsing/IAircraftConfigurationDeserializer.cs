using LiveryInstaller.Library.Models.INI;

namespace LiveryInstaller.Library.Services.Parsing;

/// <summary>
/// Represents a service that can deserialize aircraft configurations from a StreamReader.
/// </summary>
public interface IAircraftConfigurationDeserializer
{
    /// <summary>
    /// Deserializes the aircraft configuration from the specified StreamReader.
    /// </summary>
    /// <param name="streamReader">The StreamReader to read the serialized configuration from.</param>
    /// <returns>A Task that represents the asynchronous operation and returns the deserialized AircraftConfiguration.</returns>
    Task<T> DeserializeAsync<T>(StreamReader streamReader);
}