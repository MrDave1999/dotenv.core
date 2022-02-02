using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.EnvFileNotFoundException;

namespace DotEnv.Core
{
    // This class defines the method that loads the .env file based on the environment (development, test, staging, production).
    public partial class EnvLoader
    {
        /// <inheritdoc />
        public void LoadEnv()
        {
            LoadEnv(out _);
        }

        /// <inheritdoc />
        public void LoadEnv(out EnvValidationResult result)
        {
            result = _parser.ValidationResult;
            var enviroment = Environment.GetEnvironmentVariable("DOTNET_ENV");
            AddOptionalEnvFiles(enviroment != null ? new[] { $".env.{enviroment}.local" } : new[] { ".env.development.local", ".env.dev.local" });
            AddOptionalEnvFiles(".env.local");
            AddOptionalEnvFiles(enviroment != null ? new[] { $".env.{enviroment}" } : new[] { ".env.development", ".env.dev" });
            AddOptionalEnvFiles(".env");

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                SetConfigurationEnvFile(envFile);
                string fullPath = GetEnvFilePath(envFile.Path);
                if (fullPath != null)
                {
                    ReadAndParse(envFile, fullPath);
                    continue;
                }
                envFile.Exists = false;
                if (!envFile.Optional)
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

            _parser.CreateParserException();
            CreateFileNotFoundException();
        }

        /// <summary>
        /// Adds optional .env files to a collection.
        /// </summary>
        /// <param name="envFilesNames">The names of the .env files.</param>
        private void AddOptionalEnvFiles(params string[] envFilesNames)
        {
            foreach (string envFileName in envFilesNames)
                _configuration.EnvFiles.Add(new EnvFile { Path = envFileName, Optional = true });
        }
    }
}
