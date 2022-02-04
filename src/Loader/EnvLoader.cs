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
            result = _parser.ValidationResult;

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
        }

        /// <summary>
        /// Creates and throws an exception of type <see cref="FileNotFoundException" />.
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        private void CreateFileNotFoundException()
        {
            if (_validationResult.HasError())
            {
                if (_configuration.ThrowFileNotFoundException)
                    throw new FileNotFoundException(message: _validationResult.ErrorMessages);

                foreach(var errorMsg in _validationResult)
                    _parser.ValidationResult.Add(errorMsg);
            }
        }

        /// <summary>
        /// Reads the contents of an .env file and invokes the parser.
        /// </summary>
        /// <param name="envFile">The instance representing the .env file.</param>
        /// <param name="fullPath">The full path to the .env file.</param>
        private void ReadAndParse(EnvFile envFile, string fullPath)
        {
            string source = File.ReadAllText(fullPath, envFile.Encoding);
            _parser.FileName = envFile.Path;
            try
            {
                _parser.Parse(source);
            }
            catch (ParserException) { }
        }

        /// <summary>
        /// Sets the configuration of an .env file.
        /// </summary>
        /// <param name="envFile">The instance representing the .env file.</param>
        private void SetConfigurationEnvFile(EnvFile envFile)
        {
            if (!Path.HasExtension(envFile.Path))
                envFile.Path = Path.Combine(envFile.Path, _configuration.DefaultEnvFileName);

            if (envFile.Encoding == null)
                envFile.Encoding = _configuration.Encoding;

            envFile.Path = Path.Combine(_configuration.BasePath, envFile.Path);
        }

        /// <summary>
        /// Gets the full path of the .env file.
        /// </summary>
        /// <param name="envFileName">The name of the .env file to search for.</param>
        /// <returns>The path of the .env file, otherwise <c>null</c> if not found.</returns>
        /// <inheritdoc cref="Load()" path="/remarks" />
        private string GetEnvFilePath(string envFileName)
        {
            if(Path.IsPathRooted(envFileName))
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
    }
}
