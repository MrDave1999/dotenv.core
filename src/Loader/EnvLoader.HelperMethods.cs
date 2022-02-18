﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEnv.Core
{
    // This class defines the helper private methods.
    public partial class EnvLoader
    {
        /// <summary>
        /// Creates and throws an exception of type <see cref="FileNotFoundException" />.
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        private void CreateAndThrowFileNotFoundException()
        {
            if (_validationResult.HasError())
            {
                if (_configuration.ThrowFileNotFoundException)
                    throw new FileNotFoundException(message: _validationResult.ErrorMessages);

                CombineContainers();
            }
        }

        /// <summary>
        /// Combines the container of the loader with the parser.
        /// </summary>
        private void CombineContainers()
        {
            if (_parser.ValidationResult.HasError())
                _parser.ValidationResult.AddRange(errorMessages: _validationResult);
        }

        /// <summary>
        /// Reads the contents of a .env file and invokes the parser.
        /// </summary>
        /// <param name="envFile">The instance representing the .env file.</param>
        /// <exception cref="ArgumentNullException"><c>envFile</c> is <c>null</c>.</exception>
        /// <returns>true if the .env file exists, otherwise false.</returns>
        private bool ReadAndParse(EnvFile envFile)
        {
            _ = envFile ?? throw new ArgumentNullException(nameof(envFile));
            string fullPath = GetEnvFilePath(envFile.Path);
            if (fullPath == null)
                return false;

            string source = File.ReadAllText(fullPath, envFile.Encoding);
            _parser.FileName = envFile.Path;
            try
            {
                _parser.Parse(source);
            }
            catch (ParserException) { }

            return true;
        }

        /// <summary>
        /// Sets the configuration of a .env file.
        /// </summary>
        /// <param name="envFile">The instance representing the .env file.</param>
        /// <exception cref="ArgumentNullException"><c>envFile</c> is <c>null</c>.</exception>
        private void SetConfigurationEnvFile(EnvFile envFile)
        {
            _ = envFile ?? throw new ArgumentNullException(nameof(envFile));
            if (!Path.HasExtension(envFile.Path))
                envFile.Path = Path.Combine(envFile.Path, _configuration.DefaultEnvFileName);

            envFile.Encoding = envFile.Encoding ?? _configuration.Encoding;
            envFile.Path = Path.Combine(_configuration.BasePath, envFile.Path);
        }

        /// <summary>
        /// Gets an instance for an out parameter ('result').
        /// </summary>
        private EnvValidationResult GetInstanceForOutParams()
            => _parser.ValidationResult.HasError() ? _parser.ValidationResult : _validationResult;

        /// <summary>
        /// Gets the full path of the .env file.
        /// </summary>
        /// <param name="envFileName">The name of the .env file to search for.</param>
        /// <returns>The path of the .env file, otherwise <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><c>envFileName</c> is <c>null</c>.</exception>
        /// <inheritdoc cref="Load()" path="/remarks" />
        private string GetEnvFilePath(string envFileName)
        {
            _ = envFileName ?? throw new ArgumentNullException(nameof(envFileName));
            if (Path.IsPathRooted(envFileName))
                return File.Exists(envFileName) ? envFileName : null;

            for (var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
                directoryInfo != null;
                directoryInfo = directoryInfo.Parent)
            {
                string fullName = Path.Combine(directoryInfo.FullName, envFileName);
                if (File.Exists(fullName))
                    return fullName;
            }
            return null;
        }

        /// <summary>
        /// Adds optional .env files to a collection.
        /// </summary>
        /// <param name="envFilesNames">The names of the .env files.</param>
        /// <exception cref="ArgumentNullException"><c>envFilesNames</c> is <c>null</c>.</exception>
        private void AddOptionalEnvFiles(params string[] envFilesNames)
        {
            _ = envFilesNames ?? throw new ArgumentNullException(nameof(envFilesNames));
            foreach (string envFileName in envFilesNames)
                _configuration.EnvFiles.Add(new EnvFile { Path = envFileName, Optional = true });
        }
    }
}
