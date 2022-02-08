﻿using System;
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
        public IDictionary<string, string> Load()
            => Load(out _);

        /// <inheritdoc />
        public IDictionary<string, string> Load(out EnvValidationResult result)
        {
            if(_configuration.EnvFiles.Count == 0)
                _configuration.EnvFiles.Add(new EnvFile { Path = _configuration.DefaultEnvFileName, Encoding = _configuration.Encoding });

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                bool exists = ReadAndParse(envFile);
                if (!exists)
                    _validationResult.Add(errorMsg: FormatErrorMessage(FileNotFoundMessage, envFile.Path));
            }

            _parser.CreateAndThrowParserException();
            CreateAndThrowFileNotFoundException();

            result = GetInstanceForOutParams();
            return _parser.KeyValuePairs;
        }

        /// <inheritdoc />
        public IDictionary<string, string> LoadEnv()
            => LoadEnv(out _);

        /// <inheritdoc />
        public IDictionary<string, string> LoadEnv(out EnvValidationResult result)
        {
            var enviroment = Environment.GetEnvironmentVariable("DOTNET_ENV") ?? _configuration.EnvironmentName;
            AddOptionalEnvFiles(enviroment != null ? new[] { $".env.{enviroment}.local" } : new[] { ".env.development.local", ".env.dev.local" });
            AddOptionalEnvFiles(".env.local");
            AddOptionalEnvFiles(enviroment != null ? new[] { $".env.{enviroment}" } : new[] { ".env.development", ".env.dev" });
            AddOptionalEnvFiles(".env");

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                envFile.Exists = ReadAndParse(envFile);
                // This condition was added in case the client adds a new .env file with the 'AddEnvFile' method.
                if (!envFile.Exists && !envFile.Optional)
                    _validationResult.Add(errorMsg: FormatErrorMessage(FileNotFoundMessage, envFile.Path));
            }

            var envFiles = _configuration.EnvFiles;
            if (enviroment == null)
            {
                if (envFiles.NotExists(".env.development.local") &&
                    envFiles.NotExists(".env.dev.local") &&
                    envFiles.NotExists(".env.local"))
                    _validationResult.Add(errorMsg: $"{FileNotPresentLoadEnvMessage}: .env.development.local or .env.dev.local or .env.local");
            }
            else
            {
                if (envFiles.NotExists($".env.{enviroment}.local") && envFiles.NotExists(".env.local"))
                    _validationResult.Add(errorMsg: $"{FileNotPresentLoadEnvMessage}: .env.{enviroment}.local or .env.local");
            }

            _parser.CreateAndThrowParserException();
            CreateAndThrowFileNotFoundException();

            result = GetInstanceForOutParams();
            return _parser.KeyValuePairs;
        }
    }
}
