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
        /// Extracts the key from the line.
        /// </summary>
        /// <param name="line">The line with the key-value pair.</param>
        /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
        /// <returns>The key extracted.</returns>
        private string ExtractKey(string line)
        {
            _ = line ?? throw new ArgumentNullException(nameof(line));
            string key = line.Split(_configuration.DelimiterKeyValuePair, MaxCount)[0];
            key = _configuration.TrimStartKeys ? key.TrimStart() : key;
            key = _configuration.TrimEndKeys ? key.TrimEnd() : key;
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
            value = _configuration.TrimStartValues ? value.TrimStart() : value;
            value = _configuration.TrimEndValues ? value.TrimEnd() : value;
            return string.IsNullOrEmpty(value) ? " " : value;
        }

        /// <summary>
        /// Checks if the line has no a key-value pair.
        /// </summary>
        /// <param name="line">The line to test.</param>
        /// <exception cref="ArgumentNullException"><c>line</c> is <c>null</c>.</exception>
        /// <returns><c>true</c> if the line has no the key-value format, otherwise <c>false</c>.</returns>
        private bool HasNoKeyValuePair(string line)
        {
            _ = line ?? throw new ArgumentNullException(nameof(line));
            return line.Split(_configuration.DelimiterKeyValuePair, MaxCount).Length != 2;
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
        /// <param name="value">A string containing the names of zero or more environment variables.</param>
        /// <param name="lineNumber">The line number where the value was found.</param>
        /// <exception cref="ArgumentNullException"><c>value</c> is <c>null</c>.</exception>
        /// <returns>A string with each environment variable replaced by its value.</returns>
        private string ExpandEnvironmentVariables(string value, int lineNumber)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            var pattern = @"\$\{([^}]*)\}";
            value = Regex.Replace(value, pattern, match =>
            {
                var variable = match.Groups[1].Value;

                if (string.IsNullOrWhiteSpace(variable))
                {
                    ValidationResult.Add(errorMsg: FormatParserExceptionMessage(VariableIsAnEmptyStringMessage, lineNumber: lineNumber, envFileName: FileName));
                    return string.Empty;
                }

                var retrievedValue = EnvVarsProvider[variable];
                if (retrievedValue == null)
                {
                    ValidationResult.Add(errorMsg: FormatParserExceptionMessage(InterpolatedVariableNotFoundMessage, actualValue: variable, lineNumber: lineNumber, envFileName: FileName));
                    return string.Empty;
                }

                return retrievedValue;
            });
            return value;
        }
    }
}
