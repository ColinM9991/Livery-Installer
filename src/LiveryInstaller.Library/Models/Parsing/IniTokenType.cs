namespace LiveryInstaller.Library.Models.Parsing;

/// <summary>
/// Represents the type of an INI token.
/// </summary>
public enum IniTokenType
{
    /// <summary>
    /// Represents an unknown token.
    /// </summary>
    Unknown,
    /// <summary>
    /// A left bracket in a section name.
    /// </summary>
    LeftBracket,
    /// <summary>
    ///  A right bracket in a section name.
    /// </summary>
    RightBracket,
    /// <summary>
    /// Text that can represent asection, key or value.
    /// </summary>
    Text,
    /// <summary>
    /// An equals sign.
    /// </summary>
    Equals,
    /// <summary>
    /// A comment
    /// </summary>
    /// <remarks>
    /// Due to PMDG not using correct INI syntax, this can be one of ; # or //
    /// </remarks>
    Comment,
    /// <summary>
    /// A new line.
    /// </summary>
    NewLine,
    /// <summary>
    /// The end of the file or stream.
    /// </summary>
    EndOfFile
}