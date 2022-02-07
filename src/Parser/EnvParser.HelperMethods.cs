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
            line = configuration.TrimStartComments ? line.TrimStart() : line;
            return line[0] == configuration.CommentChar;
        }

        /// <summary>
        /// Extracts the key from the line.
        /// </summary>
        /// <param name="line">The line with the key-value pair.</param>
        /// <returns>The key extracted.</returns>
        protected virtual string ExtractKey(string line)
        {
            string key = line.Split(configuration.DelimiterKeyValuePair, MaxCount)[0];
            key = configuration.TrimStartKeys ? key.TrimStart() : key;
            key = configuration.TrimEndKeys ? key.TrimEnd() : key;
            return key;
        }

        /// <summary>
        /// Extracts the value from the line.
        /// </summary>
        /// <param name="line">The line with the key-value pair.</param>
        /// <returns>The value extracted.</returns>
        protected virtual string ExtractValue(string line)
        {
            string value = line.Split(configuration.DelimiterKeyValuePair, MaxCount)[1];
            value = configuration.TrimStartValues ? value.TrimStart() : value;
            value = configuration.TrimEndValues ? value.TrimEnd() : value;
            return string.IsNullOrEmpty(value) ? " " : value;
        }

        /// <summary>
        /// Checks if the line has no a key-value pair.
        /// </summary>
        /// <param name="line">The line to test.</param>
        /// <returns><c>true</c> if the line has no the key-value format, otherwise <c>false</c>.</returns>
        protected virtual bool HasNoKeyValuePair(string line)
            => line.Split(configuration.DelimiterKeyValuePair, MaxCount).Length != 2;

        /// <summary>
        /// Creates a dictionary in case the environment cannot be modified.
        /// </summary>
        protected virtual void CreateDictionary()
        {
            if (!configuration.ModifyEnvironment && keyValuePairs == null)
                keyValuePairs = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets a key as an environment variable stored in the current process. 
        /// </summary>
        /// <remarks>
        /// In case the environment cannot be modified, the method will add the key in a dictionary.
        /// </remarks>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        protected virtual void SetEnvironmentVariable(string key, string value)
        {
            if (!configuration.ModifyEnvironment)
                keyValuePairs[key] = value;
            else 
                Environment.SetEnvironmentVariable(key, value);
        }

        /// <summary>
        /// Gets the value of an environment variable from the current process.
        /// </summary>
        /// <remarks>
        /// In the case that the environment is not accessible, the method will get the value of the key from a dictionary.
        /// </remarks>
        /// <param name="key">The key to get.</param>
        /// <returns>The value of the environment variable or <c>null</c> if the variable is not found.</returns>
        protected virtual string GetEnvironmentVariable(string key)
        {
            if (!configuration.ModifyEnvironment)
            {
                keyValuePairs.TryGetValue(key, out string value);
                return value;
            }
            return Environment.GetEnvironmentVariable(key);
        }

        /// <summary>
        /// Gets an error message in case the variable is not found in the environment or dictionary.
        /// </summary>
        /// <returns>A message that describes the error.</returns>
        protected virtual string GetVariableNotFoundMessage()
            => configuration.ModifyEnvironment ? VariableNotFoundMessage : KeyNotFoundMessage;

        /// <summary>
        /// Concatenates a value with the current value of a variable.
        /// </summary>
        /// <param name="currentValue">The current value of the variable.</param>
        /// <param name="value">The value to be concatenated with the current value.</param>
        /// <returns>The string with the concatenated values.</returns>
        protected virtual string ConcatValues(string currentValue, string value)
            => configuration.ConcatDuplicateKeys == ConcatKeysOptions.End ? $"{currentValue}{value}" : $"{value}{currentValue}";

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

                var retrievedValue = GetEnvironmentVariable(variable);
                if (retrievedValue == null)
                {
                    ValidationResult.Add(errorMsg: FormatErrorMessage(GetVariableNotFoundMessage(), actualValue: variable, lineNumber: lineNumber, envFileName: FileName));
                    return match.Value;
                }

                return retrievedValue;
            });
            return value;
        }
    }
}
