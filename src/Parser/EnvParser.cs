using System;
using System.Collections.Generic;
using System.IO;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.ParserException;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvParser" />
    public partial class EnvParser : IEnvParser
    {
        /// <summary>
        /// The maximum number of substrings to be returned by the Split method.
        /// </summary>
        private const int MaxCount = 2;

        /// <summary>
        /// Allows access to the configuration options for the parser.
        /// </summary>
        private readonly EnvParserOptions _configuration = new EnvParserOptions();

        /// <summary>
        /// Allows access to the key dictionary.
        /// </summary>
        internal IDictionary<string, string> KeyValuePairs { set; get; }

        /// <summary>
        /// Allows access to the errors container of the parser.
        /// </summary>
        internal EnvValidationResult ValidationResult { get; } = new EnvValidationResult();

        /// <summary>
        /// Allows access to the name of the file that caused an error.
        /// This property is for the loader to pass data to the parser.
        /// </summary>
        internal string FileName { set; get; }

        /// <inheritdoc />
        public IDictionary<string, string> Parse(string dataSource)
            => Parse(dataSource, out _);

        /// <inheritdoc />
        public IDictionary<string, string> Parse(string dataSource, out EnvValidationResult result)
        {
            _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            result = ValidationResult;

            if (string.IsNullOrWhiteSpace(dataSource))
            {
                ValidationResult.Add(errorMsg: FormatErrorMessage(DataSourceIsEmptyOrWhitespaceMessage, envFileName: FileName));
                CreateAndThrowParserException();
                return KeyValuePairs;
            }

            CreateDictionary();

            using(var lines = new StringReader(dataSource))
            {
                int i = 1;
                for(var line = lines.ReadLine(); line != null; line = lines.ReadLine(), ++i)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (IsComment(line))
                        continue;

                    if (HasNoKeyValuePair(line))
                    {
                        ValidationResult.Add(errorMsg: FormatErrorMessage(LineHasNoKeyValuePairMessage, actualValue: line, lineNumber: i, envFileName: FileName));
                        continue;
                    }

                    var key = ExtractKey(line);
                    if (string.IsNullOrEmpty(key))
                    {
                        ValidationResult.Add(errorMsg: FormatErrorMessage(KeyIsAnEmptyStringMessage, lineNumber: i, envFileName: FileName));
                        continue;
                    }

                    var value = ExtractValue(line);
                    value = ExpandEnvironmentVariables(value, lineNumber: i);

                    var retrievedValue = GetEnvironmentVariable(key);
                    if (retrievedValue == null)
                        SetEnvironmentVariable(key, value);
                    else if (_configuration.ConcatDuplicateKeys != null)
                        SetEnvironmentVariable(key, ConcatValues(retrievedValue, value));
                    else if (_configuration.OverwriteExistingVars)
                        SetEnvironmentVariable(key, value);
                }
            }

            CreateAndThrowParserException();
            return KeyValuePairs;
        }
    }
}
