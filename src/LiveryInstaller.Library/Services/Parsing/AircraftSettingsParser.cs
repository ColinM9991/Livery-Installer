using LiveryInstaller.Library.Models.INI;
using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

public sealed class AircraftSettingsParser : IAircraftConfigurationParser<AircraftSettings>
{
    public async Task<AircraftSettings> ParseAsync(IniTokenStream tokens)
    {
        var bucket = new Dictionary<string, Dictionary<string, string>>();
        string currentSection = null;

        while (!tokens.End)
        {
            if (await tokens.MatchAsync(IniTokenType.Comment)
                || await tokens.MatchAsync(IniTokenType.NewLine))
            {
                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.LeftBracket))
            {
                _ = await tokens.MatchAsync(IniTokenType.LeftBracket);
                var section = await tokens.ExpectAsync(IniTokenType.Text);
                _ = await tokens.MatchAsync(IniTokenType.RightBracket);

                currentSection = section.Value;
                bucket.Add(currentSection, new Dictionary<string, string>());

                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.Text))
            {
                if (currentSection is null)
                {
                    _ = await tokens.MatchAsync(IniTokenType.Text);
                    continue;
                }

                var key = await tokens.ExpectAsync(IniTokenType.Text);
                _ = await tokens.MatchAsync(IniTokenType.Equals);
                var value = await tokens.ExpectAsync(IniTokenType.Text);

                var kvp = bucket[currentSection];
                kvp[key.Value] = value.Value;

                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.Unknown))
            {
                throw new FormatException("Unexpected token");
            }
        }

        const string settingsKey = "Settings";
        if (!bucket.TryGetValue(settingsKey, out var settings))
        {
            throw new FormatException("Settings section not found");
        }

        return new AircraftSettings(
            Aircraft: Required(settings, nameof(AircraftSettings.Aircraft)),
            Variant: Required(settings, nameof(AircraftSettings.Variant)),
            Name: Required(settings, nameof(AircraftSettings.Name)));
    }

    private static string Required(
        Dictionary<string, string> section,
        string key) =>
        section.TryGetValue(key, out var value)
            ? value
            : throw new FormatException($"Missing required key '{key}'.");
}