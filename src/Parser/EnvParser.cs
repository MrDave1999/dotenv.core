using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvParser" />
    public class EnvParser : IEnvParser
    {
        private const int MaxCount = 2;

        /// <summary>
        /// Allows access to the configuration options for the parser.
        /// </summary>
        protected readonly EnvParserOptions _configuration = new EnvParserOptions();

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
        /// <remarks>The environment variable will only be updated if property <see cref="EnvParserOptions.OverwriteExistingVars" /> is set to <c>true</c>.</remarks>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        protected virtual void SetEnvironmentVariable(string key, string value)
        {
            if (Environment.GetEnvironmentVariable(key) == null)
                Environment.SetEnvironmentVariable(key, value);
            else if(_configuration.OverwriteExistingVars)
                Environment.SetEnvironmentVariable(key, value);
        }

        /// <inheritdoc />
        public void Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ParserException(ExceptionMessages.InputIsEmptyOrWhitespaceMessage);

            var lines = input.Split(Environment.NewLine.ToCharArray());
            for(int i = 0, len = lines.Length; i != len; ++i)
            {
                string line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (IsComment(line))
                    continue;

                if (HasNoKeyValuePair(line))
                    throw new ParserException(ExceptionMessages.LineHasNoKeyValuePairMessage, line, i + 1);

                string key = ExtractKey(line);
                if (string.IsNullOrEmpty(key))
                    throw new ParserException(ExceptionMessages.KeyIsAnEmptyStringMessage, currentLine: i + 1);

                string value = ExtractValue(line);

                SetEnvironmentVariable(key, value);
            }
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimStartValues()
        {
            _configuration.TrimStartValues = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimEndValues()
        {
            _configuration.TrimEndValues = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimStartKeys()
        {
            _configuration.TrimStartKeys = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimEndKeys()
        {
            _configuration.TrimEndKeys = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimStartComments()
        {
            _configuration.TrimStartComments = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser AllowOverwriteExistingVars()
        {
            _configuration.OverwriteExistingVars = true;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser SetCommentChar(char commentChar)
        {
            _configuration.CommentChar = commentChar;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser SetDelimiterKeyValuePair(char separator)
        {
            _configuration.DelimiterKeyValuePair = separator;
            return this;
        }
    }
}
