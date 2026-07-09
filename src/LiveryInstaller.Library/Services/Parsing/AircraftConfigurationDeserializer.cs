using LiveryInstaller.Library.Models.INI;
using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

public sealed class AircraftConfigurationDeserializer(
    IIniLexer lexer,
    IParserProvider parserProvider) : IAircraftConfigurationDeserializer
{
    public async Task<T> DeserializeAsync<T>(StreamReader streamReader)
    {
        var tokens = lexer.LexAsync(streamReader);
        await using var tokenStream = new IniTokenStream(tokens);
        
        var parser = parserProvider.Provide<T>();
        return await parser.ParseAsync(tokenStream);
    }
}