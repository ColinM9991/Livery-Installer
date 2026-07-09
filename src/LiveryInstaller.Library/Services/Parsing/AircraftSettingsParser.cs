using LiveryInstaller.Library.Models.INI;
using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

public sealed class AircraftSettingsParser : IAircraftConfigurationParser<AircraftSettings>
{
    public async Task<AircraftSettings> ParseAsync(IniTokenStream tokens)
    {
        AircraftSettings aircraftSettings = null;
        while (!tokens.End)
        {
            if (await tokens.CheckAsync(IniTokenType.Comment))
            {
                _ = await tokens.MatchAsync(IniTokenType.Comment);
                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.NewLine))
            {
                _ = await tokens.MatchAsync(IniTokenType.NewLine);
                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.LeftBracket))
            {
                _ = await tokens.MatchAsync(IniTokenType.LeftBracket);

                var section = await tokens.ExpectAsync(IniTokenType.Text);

                _ = await tokens.MatchAsync(IniTokenType.RightBracket);

                if (string.Equals(section.Value, "Settings", StringComparison.OrdinalIgnoreCase))
                {
                    aircraftSettings = new AircraftSettings();
                }

                continue;
            }

            if (await tokens.CheckAsync(IniTokenType.Text))
            {
                await ParseKeyValueSectionAsync(tokens, aircraftSettings);

                continue;
            }
            
            if (await tokens.CheckAsync(IniTokenType.Unknown))
            {
                throw new FormatException("Unexpected token");
            }
        }

        return aircraftSettings;
    }

    private static async ValueTask ParseKeyValueSectionAsync(IniTokenStream tokens, AircraftSettings aircraftSettings)
    {
        var key = await tokens.ExpectAsync(IniTokenType.Text);
        _ = await tokens.MatchAsync(IniTokenType.Equals);
        var value = await tokens.ExpectAsync(IniTokenType.Text);

        switch (key.Value)
        {
            case "Aircraft": 
                aircraftSettings.Aircraft = value.Value;
                break;
            case "Variant":
                aircraftSettings.Variant = value.Value;
                break;
            case "Name":
                aircraftSettings.Name = value.Value;
                break;
            default:
                break;
        }
    }
}