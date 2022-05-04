using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core
{
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
        /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
        /// <returns>A string without the inline comment.</returns>
        private string RemoveInlineComment(string line)
        {
            _ = line ?? throw new ArgumentNullException(nameof(line));
            return line.Split(new[] { " #" }, MaxCount, StringSplitOptions.None)[0];
        }

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
                    int index = match.Groups[0].Captures[0].Index + 1; // So that the position starts from '1' instead of '0'.
                    var value = match.Groups[0].Value;
                    ValidationResult.Add(errorMsg: FormatParserExceptionMessage(VariableIsAnEmptyStringMessage, actualValue: value, lineNumber: currentLine, column: index, envFileName: FileName));
                    return string.Empty;
                }

                var retrievedValue = EnvVarsProvider[variable];
                if (retrievedValue is null)
                {
                    int index = match.Groups[1].Captures[0].Index + 1; // So that the position starts from '1' instead of '0'.
                    ValidationResult.Add(errorMsg: FormatParserExceptionMessage(VariableNotSetMessage, actualValue: variable, lineNumber: currentLine, column: index, envFileName: FileName));
                    return string.Empty;
                }

                return retrievedValue;
            });
            return name;
        }
    }
}
