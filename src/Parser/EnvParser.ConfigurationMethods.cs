using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    // This class defines the configuration methods that will be used to change the behavior of the parser.
    public partial class EnvParser
    {
        /// <inheritdoc />
        public IEnvParser DisableTrimStartValues()
        {
            configuration.TrimStartValues = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimEndValues()
        {
            configuration.TrimEndValues = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimValues()
        {
            DisableTrimStartValues();
            DisableTrimEndValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimStartKeys()
        {
            configuration.TrimStartKeys = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimEndKeys()
        {
            configuration.TrimEndKeys = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimKeys()
        {
            DisableTrimStartKeys();
            DisableTrimEndKeys();
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableTrimStartComments()
        {
            configuration.TrimStartComments = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser AllowOverwriteExistingVars()
        {
            configuration.OverwriteExistingVars = true;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser SetCommentChar(char commentChar)
        {
            configuration.CommentChar = commentChar;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser SetDelimiterKeyValuePair(char separator)
        {
            configuration.DelimiterKeyValuePair = separator;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End)
        {
            if (option < ConcatKeysOptions.Start || option > ConcatKeysOptions.End)
                throw new ArgumentException(ExceptionMessages.OptionInvalidMessage, nameof(option));

            configuration.ConcatDuplicateKeys = option;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser DisableParserException()
        {
            configuration.ThrowException = false;
            return this;
        }

        /// <inheritdoc />
        public IEnvParser AvoidModifyEnvironment()
        {
            configuration.ModifyEnvironment = false;
            return this;
        }
    }
}
