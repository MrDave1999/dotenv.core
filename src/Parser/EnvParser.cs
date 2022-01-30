﻿using System;
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
        /// Allows access to the configuration options for the parser.
        /// </summary>
        protected readonly EnvParserOptions configuration = new EnvParserOptions();

        /// <summary>
        /// Allows access to the errors container of the parser.
        /// </summary>
        internal EnvValidationResult ValidationResult { get; } = new EnvValidationResult();

        /// <summary>
        /// This property is for the loader to pass data to the parser.
        /// </summary>
        internal string FileName { get; set; }

        /// <inheritdoc />
        public void Parse(string input)
        {
            Parse(input, out _);
        }

        /// <inheritdoc />
        // This is the template method and defines the skeleton of the algorithm.
        // See https://en.wikipedia.org/wiki/Template_method_pattern
        public void Parse(string input, out EnvValidationResult result)
        {
            result = ValidationResult;

            if (string.IsNullOrWhiteSpace(input))
            {
                ValidationResult.Add(errorMsg: FormatErrorMessage(InputIsEmptyOrWhitespaceMessage, envFileName: FileName));
                CreateParserException();
                return;
            }
            
            using(var lines = new StringReader(input))
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

                    string key = ExtractKey(line);
                    if (string.IsNullOrEmpty(key))
                    {
                        ValidationResult.Add(errorMsg: FormatErrorMessage(KeyIsAnEmptyStringMessage, lineNumber: i, envFileName: FileName));
                        continue;
                    }

                    string value = ExtractValue(line);
                    value = ExpandEnvironmentVariables(value, lineNumber: i);

                    SetEnvironmentVariable(key, value);
                }
            }

            CreateParserException();
        }

        /// <summary>
        /// Creates and throws an exception of type <see cref="ParserException" />.
        /// </summary>
        /// <exception cref="ParserException"></exception>
        internal void CreateParserException()
        {
            if (ValidationResult.HasError() && configuration.ThrowException)
                throw new ParserException(message: ValidationResult.ErrorMessages);
        }
    }
}
