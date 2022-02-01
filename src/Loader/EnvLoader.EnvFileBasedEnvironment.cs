using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;

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
            AddEnvFiles(enviroment != null ? new[] { $".env.{enviroment}.local" } : new[] { ".env.development.local", ".env.dev.local" });
            AddEnvFile(".env.local");
            AddEnvFiles(enviroment != null ? new[] { $".env.{enviroment}" } : new[] { ".env.development", ".env.dev" });
            AddEnvFile(".env");

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                envFile.Encoding = _configuration.Encoding;
                envFile.Path = Path.Combine(_configuration.BasePath, envFile.Path);
                string fullPath = GetEnvFilePath(envFile.Path);
                if (fullPath != null)
                {
                    ReadAndParse(envFile, fullPath);
                    continue;
                }
                envFile.Exists = false;
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
    }
}
