using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DotEnv.Core.FormattingMessage;
using static DotEnv.Core.EnvFileNames;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvLoader" />
    public partial class EnvLoader : IEnvLoader
    {
        /// <summary>
        /// Allows access to the configuration options for the loader.
        /// </summary>
        private readonly EnvLoaderOptions _configuration = new EnvLoaderOptions();

        /// <summary>
        /// Allows access to the members that control the parser.
        /// </summary>
        private readonly EnvParser _parser = new EnvParser();

        /// <summary>
        /// Allows access to the errors container of the loader.
        /// </summary>
        private readonly EnvValidationResult _validationResult = new EnvValidationResult();

        /// <inheritdoc />
        public IEnvironmentVariablesProvider Load()
            => Load(out _);

        /// <inheritdoc />
        public IEnvironmentVariablesProvider Load(out EnvValidationResult result)
        {
            if (_configuration.EnvFiles.IsEmpty())
            {
                var envFileDefault = new EnvFile 
                { 
                    Path     = _configuration.DefaultEnvFileName, 
                    Encoding = _configuration.Encoding, 
                    Optional = _configuration.Optional
                };
                _configuration.EnvFiles.Add(envFileDefault);
            }

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                envFile.Exists = ReadAndParse(envFile);
                CheckEnvFileNotExistsAndNotOptional(envFile);
            }

            _parser.CreateAndThrowParserException();
            CreateAndThrowFileNotFoundException();

            result = GetInstanceForOutParams();
            return _parser.EnvVarsProvider;
        }

        /// <inheritdoc />
        public IEnvironmentVariablesProvider LoadEnv()
            => LoadEnv(out _);

        /// <inheritdoc />
        public IEnvironmentVariablesProvider LoadEnv(out EnvValidationResult result)
        {
            Env.CurrentEnvironment ??= _configuration.EnvironmentName;
            var environment = Env.CurrentEnvironment;
            var envFiles = _configuration.EnvFiles;
            var copyEnvFiles = envFiles.ToArray();
            envFiles.Clear();

            AddOptionalEnvFiles(environment != null ? new[] { $".env.{environment}.local" } : new[] { EnvDevelopmentLocalName, EnvDevLocalName });
            AddOptionalEnvFiles(EnvLocalName);
            AddOptionalEnvFiles(environment != null ? new[] { $".env.{environment}" } : new[] { EnvDevelopmentName, EnvDevName });
            AddOptionalEnvFiles(EnvName);

            // The .env files that were added with the 'AddEnvFile' method are added at the end of the collection.
            envFiles.AddRange(copyEnvFiles);

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                envFile.Exists = ReadAndParse(envFile);
                CheckEnvFileNotExistsAndNotOptional(envFile);
            }

            if (environment == null)
            {
                var envDevelopmentLocal = envFiles[0]; // .env.development.local
                var envDevLocal = envFiles[1];         // .env.dev.local
                var envLocal = envFiles[2];            // .env.local
                Env.CurrentEnvironment = EnvironmentNames.Development[0]; // Defines the default environment.
                if (!envDevelopmentLocal.Exists && !envDevLocal.Exists && !envLocal.Exists)
                    _validationResult.Add(errorMsg: FormatLocalFileNotPresentMessage());
            }
            else
            {
                var envEnvironmentLocal = envFiles[0];  // .env.[environment].local
                var envLocal = envFiles[1];             // .env.local
                if (!envEnvironmentLocal.Exists && !envLocal.Exists)
                    _validationResult.Add(errorMsg: FormatLocalFileNotPresentMessage(environmentName: environment));
            }

            _parser.CreateAndThrowParserException();
            CreateAndThrowFileNotFoundException();

            result = GetInstanceForOutParams();
            return _parser.EnvVarsProvider;
        }
    }
}
