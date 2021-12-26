using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvLoader" />
    public class EnvLoader : IEnvLoader
    {
        private readonly EnvLoaderOptions _configuration;
        private readonly IEnvParser _parser;

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
        public EnvLoader(IEnvParser parser) : this()
        {
            _parser = parser;
        }

        /// <inheritdoc />
        public void Load()
        {
            if(_configuration.EnvFiles.Count == 0)
                _configuration.EnvFiles.Add(new EnvFile { Path = _configuration.DefaultEnvFileName, Encoding = _configuration.Encoding });

            foreach (EnvFile envFile in _configuration.EnvFiles)
            {
                if (!Path.HasExtension(envFile.Path))
                    envFile.Path = Path.Combine(envFile.Path, _configuration.DefaultEnvFileName);

                if (envFile.Encoding == null)
                    envFile.Encoding = _configuration.Encoding;

                // In case if the path is absolute.
                if (Path.IsPathRooted(envFile.Path))
                {
                    if (File.Exists(envFile.Path))
                    {
                        string source = File.ReadAllText(envFile.Path, envFile.Encoding);
                        _parser.Parse(source);
                        continue;
                    }
                }
                else
                {
                    string path = GetEnvFilePath(Path.Combine(_configuration.BasePath, envFile.Path));
                    if (path != null)
                    {
                        string source = File.ReadAllText(path, envFile.Encoding);
                        _parser.Parse(source);
                        continue;
                    }
                }

                if (_configuration.ThrowFileNotFoundException)
                    throw new FileNotFoundException("The .env file could not be found.", envFile.Path);
            }
        }

        /// <summary>
        /// Gets the full path of the .env file.
        /// </summary>
        /// <param name="envFileName">The name of the .env file to search for </param>
        /// <returns>The path of the .env file, otherwise <c>null</c> if not found.</returns>
        /// <inheritdoc cref="Load" path="/remarks" />
        private string GetEnvFilePath(string envFileName)
        {
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

        /// <inheritdoc />
        public IEnvLoader SetDefaultEnvFileName(string envFileName)
        {
            _configuration.DefaultEnvFileName = envFileName;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetBasePath(string basePath)
        {
            _configuration.BasePath = basePath;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AddEnvFiles(params string[] paths)
        {
            foreach (string path in paths)
                AddEnvFile(path, null);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AddEnvFile(string path)
        {
            AddEnvFile(path, null);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AddEnvFile(string path, Encoding encoding)
        {
            _configuration.EnvFiles.Add(new EnvFile { Path = path, Encoding = encoding });
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetEncoding(Encoding encoding)
        {
            _configuration.Encoding = encoding;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader EnableFileNotFoundException()
        {
            _configuration.ThrowFileNotFoundException = true;
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimStartValues()
        {
            _parser.DisableTrimStartValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimEndValues()
        {
            _parser.DisableTrimEndValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimValues()
        {
            _parser.DisableTrimValues();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimStartKeys()
        {
            _parser.DisableTrimStartKeys();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimEndKeys()
        {
            _parser.DisableTrimEndKeys();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader DisableTrimStartComments()
        {
            _parser.DisableTrimStartComments();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader AllowOverwriteExistingVars()
        {
            _parser.AllowOverwriteExistingVars();
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetCommentChar(char commentChar)
        {
            _parser.SetCommentChar(commentChar);
            return this;
        }

        /// <inheritdoc />
        public IEnvLoader SetDelimiterKeyValuePair(char separator)
        {
            _parser.SetDelimiterKeyValuePair(separator);
            return this;
        }
    }
}
