using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

public interface IIniLexer
{
    IAsyncEnumerable<IniToken> LexAsync(TextReader streamReader, CancellationToken cancellationToken = default);
}