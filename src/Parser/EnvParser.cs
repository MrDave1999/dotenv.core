using System;
using System.Collections.Generic;
using System.IO;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

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
        /// Allows access to the errors container of the parser.
        /// </summary>
        internal EnvValidationResult ValidationResult { get; } = new EnvValidationResult();

        /// <summary>
        /// Allows access to the name of the file that caused an error.
        /// This property is for the loader to pass data to the parser.
        /// </summary>
        internal string FileName { get; set; }

        /// <summary>
        /// Allows access to the environment variables provider.
        /// </summary>
        internal IEnvironmentVariablesProvider EnvVarsProvider
        {
            get => _configuration.EnvVars;
        }

        /// <inheritdoc />
        public IEnvironmentVariablesProvider Parse(string dataSource)
            => Parse(dataSource, out _);

        /// <inheritdoc />
        public IEnvironmentVariablesProvider Parse(string dataSource, out EnvValidationResult result)
        {
            _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            result = ValidationResult;

            if (string.IsNullOrWhiteSpace(dataSource))
            {
                ValidationResult.Add(errorMsg: FormatParserExceptionMessage(DataSourceIsEmptyOrWhitespaceMessage, envFileName: FileName));
                CreateAndThrowParserException();
                return _configuration.EnvVars;
            }

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
                        ValidationResult.Add(errorMsg: FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: line, lineNumber: i, column: 1, envFileName: FileName));
                        continue;
                    }

                    line = ExpandEnvironmentVariables(line, currentLine: i);

                    var key   = ExtractKey(line);
                    var value = ExtractValue(line);

                    key   = TrimKey(key);
                    value = TrimValue(value);
                    value = string.IsNullOrEmpty(value) ? " " : value;

                    var retrievedValue = EnvVarsProvider[key];
                    if (retrievedValue == null)
                        EnvVarsProvider[key] = value;
                    else if (_configuration.ConcatDuplicateKeys != null)
                        EnvVarsProvider[key] = ConcatValues(retrievedValue, value);
                    else if (_configuration.OverwriteExistingVars)
                        EnvVarsProvider[key] = value;
                }
            }

            CreateAndThrowParserException();
            return _configuration.EnvVars;
        }
    }
}
