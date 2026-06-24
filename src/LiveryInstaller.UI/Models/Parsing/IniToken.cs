namespace LiveryInstaller.UI.Models.Parsing;

/// <summary>
/// An INI token created through lexical analysis.
/// </summary>
/// <param name="Type">The token type.</param>
public readonly record struct IniToken(IniTokenType Type)
{
    /// <summary>
    /// Creates a new token with the specified type and value.
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="value">The value of the token.</param>
    public IniToken(
        IniTokenType type,
        string value) : this(type)
    {
        Value = value;
    }

    /// <summary>
    /// The type of the token.
    /// </summary>
    public IniTokenType Type { get; } = Type;

    /// <summary>
    /// The value of the token.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new equals token.
    /// </summary>
    /// <returns>A new equals token.</returns>
    public static IniToken CreateEquals() => new(IniTokenType.Equals);

    /// <summary>
    /// Creates a new new line token.
    /// </summary>
    /// <returns>A new new line token.</returns>
    public static IniToken CreateNewLine() => new(IniTokenType.NewLine);

    /// <summary>
    /// Creates a new end of file token.
    /// </summary>
    /// <returns>A new end of file token.</returns>
    public static IniToken CreateEndOfFile() => new(IniTokenType.EndOfFile);
}