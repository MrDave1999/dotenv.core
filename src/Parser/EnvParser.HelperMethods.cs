using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core;

// This class defines the helper private (or internal) methods.
public partial class EnvParser
{
    /// <summary>
    /// Creates and throws an exception of type <see cref="ParserException" />.
    /// </summary>
    /// <exception cref="ParserException"></exception>
    internal void CreateAndThrowParserException()
    {
        if (ValidationResult.HasError() && _configuration.ThrowException)
            throw new ParserException(message: ValidationResult.ErrorMessages);
    }

    /// <summary>
    /// Checks if the line is a comment.
    /// </summary>
    /// <param name="line">The line to test.</param>
    /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
    /// <returns><c>true</c> if the line is a comment, otherwise <c>false</c>.</returns>
    private bool IsComment(string line)
    {
        _ = line ?? throw new ArgumentNullException(nameof(line));
        line = _configuration.TrimStartComments ? line.TrimStart() : line;
        return line[0] == _configuration.CommentChar;
    }

    /// <summary>
    /// Removes the inline comment.
    /// </summary>
    /// <param name="line">The line with the inline comment to remove.</param>
    /// <param name="comment">Contains the removed comment or null if there is no comment.</param>
    /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
    /// <returns>A string without the inline comment.</returns>
    private string RemoveInlineComment(string line, out string comment)
    {
        _ = line ?? throw new ArgumentNullException(nameof(line));
        var substrings = line.Split(_configuration.InlineCommentChars, MaxCount, StringSplitOptions.None);
        comment = substrings.Length == 1 ? null : substrings[1];
        return substrings[0];
    }

    /// <summary>
    /// Concatenates the comment with the value.
    /// </summary>
    /// <param name="value">The value of a key.</param>
    /// <param name="comment">The comment to concatenate with the value.</param>
    /// <returns>A string with the concatenated comment.</returns>
    private string ConcatCommentWithValue(string value, string comment)
        => comment is null ? value : $"{value} {_configuration.CommentChar}{comment}";

    /// <summary>
    /// Removes all leading and trailing white-space characters from the current key.
    /// </summary>
    /// <param name="key">The key to trim.</param>
    /// <exception cref="ArgumentNullException"><c>key</c> is <c>null</c>.</exception>
    /// <returns>
    /// The key that remains after all white-space characters are removed from the start and end of the current key.
    /// If no characters can be trimmed from the current key, the method returns the current key unchanged.
    /// </returns>
    private string TrimKey(string key)
    {
        _ = key ?? throw new ArgumentNullException(nameof(key));
        key = _configuration.TrimStartKeys ? key.TrimStart() : key;
        key = _configuration.TrimEndKeys ? key.TrimEnd() : key;
        return key;
    }

    /// <summary>
    /// Removes all leading and trailing white-space characters from the current value.
    /// </summary>
    /// <param name="value">The value to trim.</param>
    /// <exception cref="ArgumentNullException"><c>value</c> is <c>null</c>.</exception>
    /// <returns>
    /// The value that remains after all white-space characters are removed from the start and end of the current value.
    /// If no characters can be trimmed from the current value, the method returns the current value unchanged.
    /// </returns>
    private string TrimValue(string value)
    {
        _ = value ?? throw new ArgumentNullException(nameof(value));
        value = _configuration.TrimStartValues ? value.TrimStart() : value;
        value = _configuration.TrimEndValues ? value.TrimEnd() : value;
        return value;
    }

    /// <summary>
    /// Extracts the key from the line.
    /// </summary>
    /// <param name="line">The line with the key-value pair.</param>
    /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
    /// <returns>The key extracted.</returns>
    private string ExtractKey(string line)
    {
        _ = line ?? throw new ArgumentNullException(nameof(line));
        string key = line.Split(_configuration.DelimiterKeyValuePair, MaxCount)[0];
        return key;
    }

    /// <summary>
    /// Extracts the value from the line.
    /// </summary>
    /// <param name="line">The line with the key-value pair.</param>
    /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
    /// <returns>The value extracted.</returns>
    private string ExtractValue(string line)
    {
        _ = line ?? throw new ArgumentNullException(nameof(line));
        string value = line.Split(_configuration.DelimiterKeyValuePair, MaxCount)[1];
        return value;
    }

    /// <summary>
    /// Checks if the line has no a key-value pair.
    /// </summary>
    /// <param name="line">The line to test.</param>
    /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
    /// <returns><c>true</c> if the line has no the key-value pair, otherwise <c>false</c>.</returns>
    private bool HasNoKeyValuePair(string line)
    {
        _ = line ?? throw new ArgumentNullException(nameof(line));
        var keyValuePair = line.Split(_configuration.DelimiterKeyValuePair, MaxCount);
        return keyValuePair.Length != 2 || string.IsNullOrWhiteSpace(keyValuePair[0]);
    }

    /// <summary>
    /// Concatenates a value with the current value of a variable.
    /// </summary>
    /// <param name="currentValue">The current value of the variable.</param>
    /// <param name="value">The value to be concatenated with the current value.</param>
    /// <returns>The string with the concatenated values.</returns>
    private string ConcatValues(string currentValue, string value)
        => _configuration.ConcatDuplicateKeys == ConcatKeysOptions.End ? $"{currentValue}{value}" : $"{value}{currentValue}";

    /// <summary>
    /// Replaces the name of each environment variable embedded in the specified string with the string equivalent of the value of the variable, then returns the resulting string.
    /// </summary>
    /// <param name="name">A string containing the names of zero or more environment variables.</param>
    /// <param name="currentLine">The number of the current line.</param>
    /// <exception cref="ArgumentNullException"><c>name</c> is <c>null</c>.</exception>
    /// <returns>A string with each environment variable replaced by its value.</returns>
    private string ExpandEnvironmentVariables(string name, int currentLine)
    {
        _ = name ?? throw new ArgumentNullException(nameof(name));
        var pattern = @"\$\{([^}]*)\}";
        name = Regex.Replace(name, pattern, match =>
        {
            var variable = match.Groups[1].Value;

            if (string.IsNullOrWhiteSpace(variable))
            {
                // So that the position starts from '1' instead of '0'.
                int index = match.Groups[0].Captures[0].Index + 1;
                var value = match.Groups[0].Value;
                ValidationResult.Add(errorMsg: FormatParserExceptionMessage(
                    VariableIsAnEmptyStringMessage, 
                    actualValue: value, 
                    lineNumber: currentLine, 
                    column: index, 
                    envFileName: FileName
                ));
                return string.Empty;
            }

            var retrievedValue = EnvVarsProvider[variable];
            if (retrievedValue is null)
            {
                // So that the position starts from '1' instead of '0'.
                int index = match.Groups[1].Captures[0].Index + 1;
                ValidationResult.Add(errorMsg: FormatParserExceptionMessage(
                    VariableNotSetMessage, 
                    actualValue: variable, 
                    lineNumber: currentLine, 
                    column: index, 
                    envFileName: FileName
                ));
                return string.Empty;
            }

            return retrievedValue;
        });
        return name;
    }

    /// <summary>
    /// Checks if the text is quoted with single or double quotes.
    /// </summary>
    /// <param name="text">The text to validate.</param>
    /// <exception cref="ArgumentNullException"><c>text</c> is <c>null</c>.</exception>
    /// <returns><c>true</c> if the text is quoted, or <c>false</c>.</returns>
    private bool IsQuoted(string text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text));
        text = text.Trim();
        if(text.Length <= 1)
            return false;
        return (text.StartsWith(DoubleQuote) && text.EndsWith(DoubleQuote)) 
            || (text.StartsWith(SingleQuote) && text.EndsWith(SingleQuote));
    }

    /// <summary>
    /// Removes single or double quotes.
    /// </summary>
    /// <param name="text">The text with quotes to remove.</param>
    /// <exception cref="ArgumentNullException"><c>text</c> is <c>null</c>.</exception>
    /// <returns>A string without single or double quotes.</returns>
    private string RemoveQuotes(string text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text));
        return text.Trim().Trim(new[] { SingleQuote, DoubleQuote });
    }

    /// <summary>
    /// Removes the prefix before the key.
    /// </summary>
    /// <param name="key">The key with the prefix to remove.</param>
    /// <param name="prefix">The prefix name.</param>
    /// <exception cref="ArgumentNullException">key or prefix is <c>null</c>.</exception>
    /// <returns>A key without the prefix.</returns>
    private string RemovePrefixBeforeKey(string key, string prefix)
    {
        _ = key    ?? throw new ArgumentNullException(nameof(key));
        _ = prefix ?? throw new ArgumentNullException(nameof(prefix));
        var aux = key;
        key = key.TrimStart();
        return key.IndexOf(prefix) == 0 ? key.Remove(0, prefix.Length) : aux;
    }

    /// <summary>
    /// Checks if the value of a key is in multi-lines.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <exception cref="ArgumentNullException"><c>value</c> is <c>null</c>.</exception>
    /// <returns>
    /// <c>true</c> if the <c>value</c> of a key is in multi-lines, or <c>false</c>.
    /// </returns>
    private bool IsMultiline(string value)
    {
        _ = value ?? throw new ArgumentNullException(nameof(value));
        value = value.Trim();
        if(value.Length == 0)
            return false;

        if(value.Length == 1 && value[0] is DoubleQuote or SingleQuote)
            return true;

        return (value.StartsWith(DoubleQuote) && !value.EndsWith(DoubleQuote)) 
            || (value.StartsWith(SingleQuote) && !value.EndsWith(SingleQuote));
    }

    /// <summary>
    /// Gets the values of several lines of a key.
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="index">Contains the index of a line.</param>
    /// <param name="value">The value of a key.</param>
    /// <exception cref="ArgumentNullException">lines or value is <c>null</c>.</exception>
    /// <returns>
    /// A string with the values separated by a new line, or <c>null</c> if the line has no end quote.
    /// </returns>
    private string GetValuesMultilines(string[] lines, ref int index, string value)
    {
        _ = lines ?? throw new ArgumentNullException(nameof(lines));
        _ = value ?? throw new ArgumentNullException(nameof(value));
        value = value.TrimStart();
        // Double or single-quoted.
        char quoteChar = value[0];
        value = value.Substring(1);
        int initialLine = index + 1;
        int len = lines.Length;
        while(++index < len)
        {
            var line = lines[index];
            line = ExpandEnvironmentVariables(line, currentLine: index + 1);
            var trimmedLine = line.TrimEnd();
            if (trimmedLine.EndsWith(quoteChar))
            {
                var lineWithoutQuote = line.Substring(0, trimmedLine.Length - 1);
                value = $"{value}\n{lineWithoutQuote}";
                return value;
            }
            value = $"{value}\n{line}";
        }
        
        ValidationResult.Add(errorMsg: FormatParserExceptionMessage(
            quoteChar == DoubleQuote ? LineHasNoEndDoubleQuoteMessage : LineHasNoEndSingleQuoteMessage,
            lineNumber: initialLine,
            column: 1,
            envFileName: FileName
        ));
        return null;
    }

    /// <summary>
    /// Converts an empty string to a whitespace.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A string with the converted value.</returns>
    private string ConvertStringEmptyToWhitespace(string value)
        => string.IsNullOrEmpty(value) ? " " : value;
}