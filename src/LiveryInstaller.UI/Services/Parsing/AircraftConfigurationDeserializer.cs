using System.IO;
using LiveryInstaller.UI.Models.INI;
using LiveryInstaller.UI.Models.Parsing;

namespace LiveryInstaller.UI.Services.Parsing;

public sealed class AircraftConfigurationDeserializer(
    IIniLexer lexer,
    IAircraftConfigurationParser parser) : IAircraftConfigurationDeserializer
{
    public async Task<AircraftConfiguration> DeserializeAsync(StreamReader streamReader)
    {
        var tokens = lexer.LexAsync(streamReader);
        await using var tokenStream = new IniTokenStream(tokens);
        
        return await parser.ParseAsync(tokenStream);
    }
}