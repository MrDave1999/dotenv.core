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
        private IDictionary<string, string> _keyValuePairs;

        /// <inheritdoc cref="_keyValuePairs" />
        internal IDictionary<string, string> KeyValuePairs { get => _keyValuePairs; }

        /// <summary>
        /// Allows access to the errors container of the parser.
        /// </summary>
        private readonly EnvValidationResult _validationResult = new EnvValidationResult();

        /// <inheritdoc cref="_validationResult" />
        internal EnvValidationResult ValidationResult { get => _validationResult; }

        /// <summary>
        /// Allows access to the name of the file that caused an error.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// This property is for the loader to pass data to the parser.
        /// </summary>
        internal string FileName { get => _fileName; set => _fileName = value; }

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
                return _keyValuePairs;
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
            return _keyValuePairs;
        }
    }
}
