using System.IO;
using LiveryInstaller.UI.Models.INI;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class AircraftConfigurationDeserializer : IAircraftConfigurationDeserializer
{
    /// <inheritdoc />
    public async Task<AircraftConfiguration> DeserializeAsync(StreamReader streamReader)
    {
        var root = new AircraftConfiguration();
        SectionNode currentNode = null;
        List<string> headerBuffer = [];

        while (await streamReader.ReadLineAsync() is { } line)
        {
            var trimmedLine = line.Trim();
            if (IsSection(trimmedLine))
            {
                if (root.Header is null && headerBuffer is { Count: > 0 })
                {
                    var header = new HeaderNode();
                    foreach (var headerLine in headerBuffer)
                        header.Values.Add(new StringSectionValue(headerLine));

                    root.Header = header;
                }

                currentNode = GetSectionNode(trimmedLine[1..^1]);
                root.AddSection(currentNode);

                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                continue;
            }

            if (root.Sections.Count == 0)
            {
                headerBuffer.Add(trimmedLine);
                continue;
            }

            SectionValue sectionValue = currentNode is FlightSimSectionNode
                ? new KeySectionValue(trimmedLine[..trimmedLine.IndexOf('=')], trimmedLine[(trimmedLine.IndexOf('=') + 1)..])
                : new StringSectionValue(trimmedLine);
            
            currentNode?.Values.Add(sectionValue);
        }

        return root.Sections.Count == 0
            ? throw new InvalidOperationException("No sections found")
            : root;
    }

    /// <summary>
    /// Gets the section node for the given section name.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    /// <returns>The SectionNode corresponding to the given section name.</returns>
    private static SectionNode GetSectionNode(string sectionName) => sectionName switch
    {
        var s when s.StartsWith("fltsim") => new FlightSimSectionNode(s),
        _ => new SectionNode(sectionName),
    };

    /// <summary>
    /// Determines whether the given line is a section.
    /// </summary>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line is a section, otherwise false.</returns>
    private static bool IsSection(string line) => line.StartsWith('[') && line.EndsWith(']');
}