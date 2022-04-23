using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core
{
    // This class defines the helper private methods.
    public partial class EnvLoader
    {
        /// <summary>
        /// Checks if the .env file does not exist and is not optional.
        /// </summary>
        /// <param name="envFile">The instance representing the .env file.</param>
        /// <exception cref="ArgumentNullException"><c>envFile</c> is <c>null</c>.</exception>
        private void CheckEnvFileNotExistsAndNotOptional(EnvFile envFile)
        {
            _ = envFile ?? throw new ArgumentNullException(nameof(envFile));
            if (!envFile.Exists && !envFile.Optional)
                _validationResult.Add(errorMsg: string.Format(FileNotFoundMessage, envFile.Path));
        }

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
            string fullPath;
            if (_configuration.SearchParentDirectories)
                fullPath = GetEnvFilePath(envFile.Path);
            else
                fullPath = File.Exists(envFile.Path) ? envFile.Path : null;

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

            envFile.Encoding ??= _configuration.Encoding;
            envFile.Optional = envFile.Optional ? envFile.Optional : _configuration.Optional;
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
        /// <param name="envFileName">
        /// The name of the .env file to search for.
        /// The .env file name can include an absolute or relative path.
        /// </param>
        /// <returns>The path of the .env file, otherwise <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><c>envFileName</c> is <c>null</c>.</exception>
        /// <inheritdoc cref="Load()" path="/remarks" />
        private string GetEnvFilePath(string envFileName)
        {
            _ = envFileName ?? throw new ArgumentNullException(nameof(envFileName));
            string path;
            if (Path.IsPathRooted(envFileName))
            {
                path = Path.GetDirectoryName(envFileName);
                envFileName = Path.GetFileName(envFileName);
            }
            else
                path = AppContext.BaseDirectory;

            for (var directoryInfo = new DirectoryInfo(path);
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
