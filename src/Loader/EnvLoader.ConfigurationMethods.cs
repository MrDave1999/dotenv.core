using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    // This class defines the configuration methods that will be used to change the behavior of the loader and parser.
    public partial class EnvLoader
    {
        /// <inheritdoc />
        public IEnvLoader SetDefaultEnvFileName(string envFileName)
        {
            _configuration.DefaultEnvFileName = envFileName;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetBasePath(string basePath)
        {
            _configuration.BasePath = basePath;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AddEnvFiles(params string[] paths)
        {
            foreach (string path in paths)
                AddEnvFile(path, null);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AddEnvFile(string path)
        {
            AddEnvFile(path, null);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AddEnvFile(string path, Encoding encoding)
        {
            _configuration.EnvFiles.Add(new EnvFile { Path = path, Encoding = encoding });
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetEncoding(Encoding encoding)
        {
            _configuration.Encoding = encoding;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader EnableFileNotFoundException()
        {
            _configuration.ThrowFileNotFoundException = true;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimStartValues()
        {
            _parser.DisableTrimStartValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimEndValues()
        {
            _parser.DisableTrimEndValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimValues()
        {
            _parser.DisableTrimValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimStartKeys()
        {
            _parser.DisableTrimStartKeys();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimEndKeys()
        {
            _parser.DisableTrimEndKeys();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimKeys()
        {
            _parser.DisableTrimKeys();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimStartComments()
        {
            _parser.DisableTrimStartComments();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AllowOverwriteExistingVars()
        {
            _parser.AllowOverwriteExistingVars();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetCommentChar(char commentChar)
        {
            _parser.SetCommentChar(commentChar);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetDelimiterKeyValuePair(char separator)
        {
            _parser.SetDelimiterKeyValuePair(separator);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End)
        {
            _parser.AllowConcatDuplicateKeys(option);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableParserException()
        {
            _parser.DisableParserException();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AvoidModifyEnvironment()
        {
            _parser.AvoidModifyEnvironment();
            return this;
        }
    }
}
