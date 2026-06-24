using System.IO;
using LiveryInstaller.UI.Models.INI;

namespace LiveryInstaller.UI.Services.Parsing;

/// <inheritdoc />
public sealed class AircraftConfigurationSerializer : IAircraftConfigurationSerializer
{
    /// <inheritdoc />
    public async Task SerializeAsync(AircraftConfiguration configuration, StreamWriter streamWriter)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(streamWriter);

        if (configuration.Header is not null)
        {
            foreach (var line in configuration.Header.Values)
            {
                await streamWriter.WriteLineAsync(line);
            }

            await streamWriter.WriteLineAsync();
        }

        foreach (var section in configuration.Sections)
        {
            await streamWriter.WriteLineAsync(section);
            
            foreach (var value in section.Values)
            {
                await streamWriter.WriteLineAsync(value);
            }
            
            await streamWriter.WriteLineAsync();
        }
    }
}