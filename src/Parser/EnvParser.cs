using System;
using System.Collections.Generic;
using System.IO;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvParser" />
    public partial class EnvParser : IEnvParser
    {
        /// <summary>
        /// Allows access to the configuration options for the parser.
        /// </summary>
        protected readonly EnvParserOptions _configuration = new EnvParserOptions();

        /// <inheritdoc />
        // This is the template method and defines the skeleton of the algorithm.
        // See https://en.wikipedia.org/wiki/Template_method_pattern
        public void Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                if(_configuration.ThrowException)
                    throw new ParserException(ExceptionMessages.InputIsEmptyOrWhitespaceMessage);

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
                        if(_configuration.ThrowException)
                            throw new ParserException(ExceptionMessages.LineHasNoKeyValuePairMessage, line, i);

                        continue;
                    }

                    string key = ExtractKey(line);
                    if (string.IsNullOrEmpty(key))
                    {
                        if(_configuration.ThrowException)
                            throw new ParserException(ExceptionMessages.KeyIsAnEmptyStringMessage, currentLine: i);

                        continue;
                    }

                    string value = ExtractValue(line);
                    value = ExpandEnvironmentVariables(value, lineNumber: i);

                    SetEnvironmentVariable(key, value);
                }
            }
        }
    }
}
