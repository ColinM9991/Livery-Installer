using System.IO;
using LiveryInstaller.UI.Models.Parsing;

namespace LiveryInstaller.UI.Services.Parsing;

public interface IIniLexer
{
    IAsyncEnumerable<IniToken> LexAsync(TextReader streamReader, CancellationToken cancellationToken = default);
}