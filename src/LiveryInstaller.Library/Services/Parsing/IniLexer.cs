using System.Runtime.CompilerServices;
using LiveryInstaller.Library.Models.Parsing;

namespace LiveryInstaller.Library.Services.Parsing;

public class IniLexer : IIniLexer
{
    public async IAsyncEnumerable<IniToken> LexAsync(
        TextReader streamReader,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await streamReader.ReadLineAsync(cancellationToken) is { } line)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var reader = new IniLineReader(line);
            var afterEquals = false;

            while (!reader.End)
            {
                reader.SkipWhitespace();

                if (reader.End) break;

                var token = reader.Current switch
                {
                    '[' => ReadLeftBracket(ref reader),
                    ']' => ReadRightBracket(ref reader),
                    '=' => ReadEquals(ref reader),
                    ';' or '#' => ReadComment(ref reader),
                    '/' when reader.Peek() == '/' => ReadComment(ref reader),
                    _ => ReadText(ref reader, afterEquals)
                };

                if (token is { Type: IniTokenType.Equals })
                {
                    afterEquals = true;
                }

                yield return token;
            }

            yield return IniToken.CreateNewLine();
        }

        yield return IniToken.CreateEndOfFile();
    }

    private static IniToken ReadLeftBracket(ref IniLineReader reader)
    {
        reader.Advance();

        return new IniToken(IniTokenType.LeftBracket);
    }

    private static IniToken ReadRightBracket(ref IniLineReader reader)
    {
        reader.Advance();

        return new IniToken(IniTokenType.RightBracket);
    }

    /// <summary>
    /// Reads the <see cref="IniLineReader"/> as text. This reads until the next delimiter or until the end of the line if it's a value of a key-value pair.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="afterEquals"></param>
    /// <returns>
    /// While the correct INI comment syntax is ';' or '#', PMDG uses // for comments in INI files. Comments exist within key-value pairs. Some comments contain '='. 
    /// They also use ';' within values which are not to be treated as comments. Their INI structure is a mess.
    /// </returns>
    private static IniToken ReadText(ref IniLineReader reader, bool afterEquals)
    {
        var text =
            afterEquals && reader.Current is '"'
                ? reader.TakeRest()
                : reader.TakeUntil(values => values.Current is '[' or ']' or '=' or ';' or '#' || values is { Current: '/', Next: '/' });

        return new IniToken(IniTokenType.Text, text.TrimEnd());
    }

    private static IniToken ReadComment(ref IniLineReader reader) => new(IniTokenType.Comment, reader.TakeRest());

    private static IniToken ReadEquals(ref IniLineReader reader)
    {
        reader.Advance();

        return IniToken.CreateEquals();
    }
}