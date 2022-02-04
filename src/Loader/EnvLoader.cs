using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.EnvFileNotFoundException;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvLoader" />
    public partial class EnvLoader : IEnvLoader
    {
        /// <summary>
        /// Allows access to the configuration options for the loader.
        /// </summary>
        private readonly EnvLoaderOptions _configuration;

        /// <summary>
        /// Allows access to the members that control the parser.
        /// </summary>
        private readonly EnvParser _parser;

        /// <summary>
        /// Allows access to the errors container of the loader.
        /// </summary>
        private readonly EnvValidationResult _validationResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvLoader" /> class.
        /// </summary>
        public EnvLoader()
        {
            _configuration = new EnvLoaderOptions();
            _parser = new EnvParser();
            _validationResult = new EnvValidationResult();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvLoader" /> class with a parser.
        /// </summary>
        /// <param name="parser">The parser instance.</param>
        public EnvLoader(EnvParser parser) : this()
        {
            _parser = parser;
        }

        /// <inheritdoc />
        public void Load()
        {
            Load(out _);
        }

        /// <inheritdoc />
        public void Load(out EnvValidationResult result)
        {
            if(_configuration.EnvFiles.Count == 0)
                _configuration.EnvFiles.Add(new EnvFile { Path = _configuration.DefaultEnvFileName, Encoding = _configuration.Encoding });

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                string fullPath = GetEnvFilePath(envFile.Path);
                if (fullPath != null)
                {
                    ReadAndParse(envFile, fullPath);
                    continue;
                }
                
                _validationResult.Add(errorMsg: FormatErrorMessage(FileNotFoundMessage, envFile.Path));
            }

            _parser.CreateParserException();
            CreateFileNotFoundException();

            result = GetInstanceForOutParams();
        }
    }
}
