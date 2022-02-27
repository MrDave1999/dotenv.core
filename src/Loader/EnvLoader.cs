using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

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
        public IDictionary<string, string> Load()
            => Load(out _);

        /// <inheritdoc />
        public IDictionary<string, string> Load(out EnvValidationResult result)
        {
            if(_configuration.EnvFiles.IsEmpty())
                _configuration.EnvFiles.Add(new EnvFile { Path = _configuration.DefaultEnvFileName, Encoding = _configuration.Encoding });

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                bool exists = ReadAndParse(envFile);
                if (!exists)
                    _validationResult.Add(errorMsg: FormatFileNotFoundExceptionMessage(FileNotFoundMessage, envFile.Path));
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
            var envFiles = _configuration.EnvFiles;
            var copyEnvFiles = envFiles.ToArray();
            envFiles.Clear();

            AddOptionalEnvFiles(enviroment != null ? new[] { $".env.{enviroment}.local" } : new[] { ".env.development.local", ".env.dev.local" });
            AddOptionalEnvFiles(".env.local");
            AddOptionalEnvFiles(enviroment != null ? new[] { $".env.{enviroment}" } : new[] { ".env.development", ".env.dev" });
            AddOptionalEnvFiles(".env");

            // The .env files that were added with the 'AddEnvFile' method are added at the end of the collection.
            envFiles.AddRange(copyEnvFiles);

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                envFile.Exists = ReadAndParse(envFile);
                // This condition was added in case the client adds a new .env file with the 'AddEnvFile' method.
                if (!envFile.Exists && !envFile.Optional)
                    _validationResult.Add(errorMsg: FormatFileNotFoundExceptionMessage(FileNotFoundMessage, envFile.Path));
            }

            if (enviroment == null)
            {
                var envDevelopmentLocal = envFiles[0]; // .env.development.local
                var envDevLocal = envFiles[1];         // .env.dev.local
                var envLocal = envFiles[2];            // .env.local
                if (!envDevelopmentLocal.Exists && !envDevLocal.Exists && !envLocal.Exists)
                    _validationResult.Add(errorMsg: FormatFileNotPresentLoadEnvMessage(FileNotPresentLoadEnvMessage));
            }
            else
            {
                var envEnvironmentLocal = envFiles[0];  // .env.[environment].local
                var envLocal = envFiles[1];             // .env.local
                if (!envEnvironmentLocal.Exists && !envLocal.Exists)
                    _validationResult.Add(errorMsg: FormatFileNotPresentLoadEnvMessage(FileNotPresentLoadEnvMessage, enviroment));
            }

            _parser.CreateAndThrowParserException();
            CreateAndThrowFileNotFoundException();

            result = GetInstanceForOutParams();
            return _parser.KeyValuePairs;
        }
    }
}
