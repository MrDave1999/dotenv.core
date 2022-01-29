using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.ParserException;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvLoader" />
    public partial class EnvLoader : IEnvLoader
    {
        private readonly EnvLoaderOptions _configuration;
        private readonly EnvParser _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvLoader" /> class.
        /// </summary>
        public EnvLoader()
        {
            _configuration = new EnvLoaderOptions();
            _parser = new EnvParser();
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
            var validationResult = new EnvValidationResult();
            result = _parser.ValidationResult;

            if(_configuration.EnvFiles.Count == 0)
                _configuration.EnvFiles.Add(new EnvFile { Path = _configuration.DefaultEnvFileName, Encoding = _configuration.Encoding });

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                if (!Path.HasExtension(envFile.Path))
                    envFile.Path = Path.Combine(envFile.Path, _configuration.DefaultEnvFileName);

                if (envFile.Encoding == null)
                    envFile.Encoding = _configuration.Encoding;

                envFile.Path = Path.Combine(_configuration.BasePath, envFile.Path);

                string path = GetEnvFilePath(envFile.Path);
                if (path != null)
                {
                    string source = File.ReadAllText(path, envFile.Encoding);
                    _parser.FileName = envFile.Path;
                    try
                    {
                        _parser.Parse(source);
                    }
                    catch(ParserException) { }
                    continue;
                }
                
                validationResult.Add(errorMsg: $"{FileNotFoundMessage} (FileName: {envFile.Path})");
            }

            _parser.CreateParserException();

            if (validationResult.HasError())
            {
                if (_configuration.ThrowFileNotFoundException)
                    throw new FileNotFoundException(message: validationResult.ErrorMessages);

                _parser.ValidationResult.Add(errorMsg: validationResult.ErrorMessages);
            }
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
