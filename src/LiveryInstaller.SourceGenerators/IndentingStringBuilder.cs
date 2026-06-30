using System;
using System.Text;

namespace LiveryInstaller.SourceGenerators;

public sealed class IndentingStringBuilder
{
    private readonly string _indentation;
    private readonly StringBuilder _builder;
    private int _indentationLevel;
    private string _currentIndentation = string.Empty;

    private string[] _indentationStrings = [string.Empty, string.Empty, string.Empty, string.Empty];

    public IndentingStringBuilder(string indentation = "    ")
    {
        _indentation = indentation;
        _builder = new StringBuilder();

        for (var i = 1; i < _indentationStrings.Length; i++)
            _indentationStrings[i] = _indentationStrings[i - 1] + _indentation;
    }

    public IndentingStringBuilder DecrementIndentation()
    {
        if (_indentationLevel == 0)
            throw new InvalidOperationException("Cannot decrement indentation level below 0");
        
        _indentationLevel--;
        _currentIndentation = _indentationStrings[_indentationLevel];

        return this;
    }

    public IndentingStringBuilder IncrementIndentation()
    {
        _indentationLevel++;
        
        if (_indentationLevel >= _indentationStrings.Length)
            Array.Resize(ref _indentationStrings, _indentationStrings.Length * 2);

        _indentationStrings[_indentationLevel] ??= _indentationStrings[_indentationLevel - 1] + _indentation;
        _currentIndentation = _indentationStrings[_indentationLevel];

        return this;
    }
    
    public void Append(string content)
    {
        AppendContent(content);
    }

    public IndentingStringBuilder AppendLine(string line = "")
    {
        AppendContent(line);
        _builder.AppendLine();
        return this;
    }
    
    public override string ToString() => _builder.ToString();

    private void AppendContent(string content)
    {
        if (_builder.Length == 0 || IsNewLine(_builder[_builder.Length - 1]))
            _builder.Append(_currentIndentation);
        
        _builder.Append(content);
    }

    private static bool IsNewLine(char input) => input is '\r' or '\n' or '\u0085' or '\u2028' or '\u2029' or '\f';
}