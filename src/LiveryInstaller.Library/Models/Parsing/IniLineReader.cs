namespace LiveryInstaller.Library.Models.Parsing;

public struct IniLineReader(string buffer)
{
    private int _position = 0;

    public char Current => End ? '\0' : buffer[_position];

    public bool End => _position >= buffer.Length;

    public char Peek() => End || _position + 1 >= buffer.Length ? '\0' : buffer[_position + 1];

    public void Advance()
    {
        _position++;
    }

    public void SkipWhitespace()
    {
        while (!End && Current is ' ' or '\t')
        {
            Advance();
        }
    }

    public string TakeUntil(Predicate<(char Current, char Next)> predicate)
    {
        var startPosition = _position;
        while (!End && !predicate((Current, Peek())))
        {
            Advance();
        }

        return buffer[startPosition.._position];
    }

    public string TakeRest()
    {
        var startPosition = _position;
        _position = buffer.Length;

        return buffer[startPosition..];
    }
}