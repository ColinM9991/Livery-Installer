using LiveryInstaller.Library.Models.INI;
using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

/// <inheritdoc />
public sealed class AircraftConfigurationParser : IAircraftConfigurationParser<AircraftConfiguration>
{
    /// <inheritdoc />
    public async Task<AircraftConfiguration> ParseAsync(IniTokenStream tokens)
    {
        var root = new AircraftConfiguration();
        SectionNode currentNode = null;
        List<string> headerBuffer = [];

        while (!tokens.End)
        {
            // Skip newline
            if (await tokens.CheckAsync(IniTokenType.NewLine))
            {
                _ = await tokens.MatchAsync(IniTokenType.NewLine);
                continue;
            }

            // Parse comment
            if (await tokens.CheckAsync(IniTokenType.Comment))
            {
                var comment = await tokens.ExpectAsync(IniTokenType.Comment);
                var commentValue = comment.Value;
                if (currentNode is null)
                {
                    headerBuffer.Add(commentValue);
                }
                else
                {
                    currentNode.Values.Add(new StringSectionValue(commentValue));
                }

                continue;
            }

            // Parse Section
            if (await tokens.CheckAsync(IniTokenType.LeftBracket))
            {
                if (root.Header is null && headerBuffer is { Count: > 0 })
                {
                    root.Header = ProcessHeaderNode(headerBuffer);
                }
                
                currentNode = await ParseSectionAsync(tokens);
                root.AddSection(currentNode);
                continue;
            }

            // Parse Text
            if (currentNode is not null && await tokens.CheckAsync(IniTokenType.Text))
            {
                var node = await ParseKeyValueAsync(tokens);

                currentNode.Values.Add(node);
                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.Unknown))
            {
                throw new FormatException("Unexpected token");
            }
        }

        return root.Sections.Count == 0
            ? throw new InvalidOperationException("No sections found")
            : root;
    }

    private static async ValueTask<SectionNode> ParseSectionAsync(IniTokenStream tokens)
    {
        _ = await tokens.ExpectAsync(IniTokenType.LeftBracket);

        var sectionName = await tokens.ExpectAsync(IniTokenType.Text);
        var sectionNameValue = sectionName.Value ?? throw new FormatException("Section name cannot be null");

        _ = await tokens.ExpectAsync(IniTokenType.RightBracket);
        
        return sectionNameValue switch
        {
            _ when sectionNameValue.StartsWith("fltsim") => new FlightSimSectionNode(sectionNameValue),
            _ => new SectionNode(sectionNameValue),
        };;
    }

    private static async ValueTask<KeySectionValue> ParseKeyValueAsync(IniTokenStream tokens)
    {
        var lhs = await tokens.ExpectAsync(IniTokenType.Text);
        var keyValue = lhs.Value ?? throw new FormatException("Key value cannot be null");

        _ = await tokens.ExpectAsync(IniTokenType.Equals);

        var rhs = await tokens.CheckAsync(IniTokenType.Text)
            ? await tokens.ExpectAsync(IniTokenType.Text)
            : default;
                
        var valueText = rhs.Value ?? string.Empty;

        return new KeySectionValue(keyValue, valueText);
    }

    private static HeaderNode ProcessHeaderNode(IEnumerable<string> headerBuffer)
    {
        var header = new HeaderNode();
        foreach (var headerLine in headerBuffer)
            header.Values.Add(new StringSectionValue(headerLine));

        return header;
    }
}