using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.ParserException;

namespace DotEnv.Core
{
    // This class defines the helper methods to be used in the template method.
    // See https://en.wikipedia.org/wiki/Template_method_pattern
    public partial class EnvParser
    {
        /// <summary>
        /// The maximum number of substrings to be returned by the Split method.
        /// </summary>
        private const int MaxCount = 2;

        /// <summary>
        /// Checks if the line is a comment.
        /// </summary>
        /// <param name="line">The line to test.</param>
        /// <returns><c>true</c> if the line is a comment, otherwise <c>false</c>.</returns>
        protected virtual bool IsComment(string line)
        {
            line = _configuration.TrimStartComments ? line.TrimStart() : line;
            return line[0] == _configuration.CommentChar;
        }

        /// <summary>
        /// Extracts the key from the line.
        /// </summary>
        /// <param name="line">The line with the key-value pair.</param>
        /// <returns>The key extracted.</returns>
        protected virtual string ExtractKey(string line)
        {
            string key = line.Split(_configuration.DelimiterKeyValuePair, MaxCount)[0];
            key = _configuration.TrimStartKeys ? key.TrimStart() : key;
            key = _configuration.TrimEndKeys ? key.TrimEnd() : key;
            return key;
        }

        /// <summary>
        /// Extracts the value from the line.
        /// </summary>
        /// <param name="line">The line with the key-value pair.</param>
        /// <returns>The value extracted.</returns>
        protected virtual string ExtractValue(string line)
        {
            string value = line.Split(_configuration.DelimiterKeyValuePair, MaxCount)[1];
            value = _configuration.TrimStartValues ? value.TrimStart() : value;
            value = _configuration.TrimEndValues ? value.TrimEnd() : value;
            return string.IsNullOrEmpty(value) ? " " : value;
        }

        /// <summary>
        /// Checks if the line has no a key-value pair.
        /// </summary>
        /// <param name="line">The line to test.</param>
        /// <returns><c>true</c> if the line has no the key-value format, otherwise <c>false</c>.</returns>
        protected virtual bool HasNoKeyValuePair(string line)
            => line.Split(_configuration.DelimiterKeyValuePair, MaxCount).Length != 2;

        /// <summary>
        /// Create or update an environment variable.
        /// </summary>
        /// <remarks>The environment variable will only be updated if the <see cref="EnvParserOptions.OverwriteExistingVars" /> property is set to <c>true</c> 
        /// or if the <see cref="EnvParserOptions.ConcatDuplicateKeys" /> property is set to <see cref="ConcatKeysOptions.Start" /> or <see cref="ConcatKeysOptions.End" />.
        /// </remarks>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        protected virtual void SetEnvironmentVariable(string key, string value)
        {
            var retrievedValue = Environment.GetEnvironmentVariable(key);
            if (retrievedValue == null)
                Environment.SetEnvironmentVariable(key, value);
            else if (_configuration.ConcatDuplicateKeys != ConcatKeysOptions.None)
                Environment.SetEnvironmentVariable(key, ConcatValues(retrievedValue, value));
            else if (_configuration.OverwriteExistingVars)
                Environment.SetEnvironmentVariable(key, value);
        }

        /// <summary>
        /// Concatenates a value with the current value of a variable.
        /// </summary>
        /// <param name="currentValue">The current value of the variable.</param>
        /// <param name="value">The value to be concatenated with the current value.</param>
        /// <returns>The string with the concatenated values.</returns>
        protected virtual string ConcatValues(string currentValue, string value)
            => _configuration.ConcatDuplicateKeys == ConcatKeysOptions.End ? $"{currentValue}{value}" : $"{value}{currentValue}";

        /// <summary>
        /// Replaces the name of each environment variable embedded in the specified string with the string equivalent of the value of the variable, then returns the resulting string.
        /// </summary>
        /// <param name="value">A string containing the names of zero or more environment variables.</param>
        /// <param name="lineNumber">The line number where the value was found.</param>
        /// <returns>A string with each environment variable replaced by its value.</returns>
        protected virtual string ExpandEnvironmentVariables(string value, int lineNumber)
        {
            var pattern = @"\$\{([^}]*)\}";
            value = Regex.Replace(value, pattern, match =>
            {
                var variable = match.Groups[1].Value;

                if (string.IsNullOrWhiteSpace(variable))
                {
                    ValidationResult.Add(errorMsg: FormatErrorMessage(VariableIsAnEmptyStringMessage, lineNumber: lineNumber, envFileName: FileName));
                    return match.Value;
                }

                var retrievedValue = Environment.GetEnvironmentVariable(variable);
                if (retrievedValue == null)
                {
                    ValidationResult.Add(errorMsg: FormatErrorMessage(VariableNotFoundMessage, actualValue: variable, lineNumber: lineNumber, envFileName: FileName));
                    return match.Value;
                }

                return retrievedValue;
            });
            return value;
        }
    }
}
