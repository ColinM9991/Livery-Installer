namespace LiveryInstaller.UI.Models.Parsing;

/// <summary>
/// An iterator over a stream of <see cref="IniToken"/>.
/// </summary>
/// <param name="tokens">The stream of tokens.</param>
public sealed class IniTokenStream(IAsyncEnumerable<IniToken> tokens) : IAsyncDisposable
{
    private readonly IAsyncEnumerator<IniToken> _enumerator = tokens.GetAsyncEnumerator();
    private IniToken? _current;
    private IniToken? _previous;

    /// <summary>
    /// Consumes the current token and returns it.
    /// </summary>
    /// <returns>The current token.</returns>
    /// <exception cref="InvalidOperationException">When the token is not of the expected type.</exception>
    public async ValueTask<IniToken> ConsumeAsync(IniTokenType type)
    {
        if (await CheckAsync(type)) return await AdvanceAsync();

        throw new InvalidOperationException($"Expected token of type {type}");
    }

    /// <summary>
    /// Expects the token to be of a given type and returns it.
    /// </summary>
    /// <param name="type">The expected type.</param>
    /// <returns>The current token.</returns>
    /// <exception cref="InvalidOperationException">When the token is not of the expected type.</exception>
    public async ValueTask<IniToken> ExpectAsync(IniTokenType type)
    {
        if (await CheckAsync(type)) return await ConsumeAsync(type);

        throw new InvalidOperationException($"Expected token of type {type}");
    }

    /// <summary>
    /// Consumes a series of tokens of a given type in order.
    /// </summary>
    /// <param name="types">The expected types.</param>
    /// <returns>True if the token was consumed, otherwise false if the type did not match.</returns>
    public async ValueTask<bool> MatchAsync(params IniTokenType[] types)
    {
        foreach (var type in types)
        {
            if (!await CheckAsync(type)) return false;

            await ConsumeAsync(type);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the current token is of a given type.
    /// </summary>
    /// <param name="type">The expected type.</param>
    /// <returns>True if the token is of the expected type.</returns>
    public async ValueTask<bool> CheckAsync(IniTokenType type)
    {
        var token = await PeekAsync();

        return token.Type == type;
    }

    /// <summary>
    /// Gets the current token without consuming it.
    /// </summary>
    /// <returns>The current token if it was not null, otherwise the first token.</returns>
    /// <remarks>
    /// This lazily advances the token stream to the next token.
    /// </remarks>
    public async ValueTask<IniToken> PeekAsync()
    {
        await EnsureCurrentAsync();
        return _current!.Value;
    }

    public bool End => _current?.Type == IniTokenType.EndOfFile;

    /// <summary>
    /// Advances the token stream to the next token.
    /// </summary>
    /// <returns>The consumed token.</returns>
    /// <exception cref="InvalidOperationException">When the stream reached the end.</exception>
    private async ValueTask<IniToken> AdvanceAsync()
    {
        if (!await _enumerator.MoveNextAsync())
            throw new InvalidOperationException("Unexpected end of stream");

        _previous = _current;
        _current = _enumerator.Current;
        return _previous!.Value;
    }

    /// <summary>
    /// Lazily advances the token stream to the next token if the current is null.
    /// </summary>
    private async ValueTask EnsureCurrentAsync()
    {
        if (_current.HasValue) return;
        if (End) return;

        await _enumerator.MoveNextAsync();
        _current = _enumerator.Current;
    }

    /// <summary>
    /// Disposes the token stream.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _enumerator.DisposeAsync();
    }
}