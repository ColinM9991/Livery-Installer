using LiveryInstaller.Library.Models.INI;
using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

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