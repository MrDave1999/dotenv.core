using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents the .env files loader.
    /// </summary>
    public interface IEnvLoader
    {
        /// <param name="result">The result that contains the errors found by the loader.</param>
        /// <inheritdoc cref="LoadEnv()" />
        void LoadEnv(out EnvValidationResult result);

        /// <summary>
        /// Loads an .env file based on the environment (development, test, staging or production).
        /// This method will load these .env files in the following order:
        /// .env.[environment].local, .env.local, .env.[environment], .env
        /// </summary>
        /// <inheritdoc cref="Load()" />
        void LoadEnv();

        /// <param name="result">The result that contains the errors found by the loader.</param>
        /// <inheritdoc cref="Load()" />
        void Load(out EnvValidationResult result);

        /// <summary>
        /// Loads more than one .env file. By default, it will search for a file called <c>.env</c>.
        /// </summary>
        /// <remarks>This method starts find the .env file in the current directory and if it does not found it, it starts find in the parent directories of the current directory.</remarks>
        /// <exception cref="ParserException">
        /// If the parser find an error during the parsing.
        /// This exception is only thrown if the <see cref="DisableParserException" /> method is invoked.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// If the .env file is not found.
        /// This exception is only thrown if the <see cref="EnableFileNotFoundException" /> method is invoked.
        /// </exception>
        void Load();

        /// <summary>
        /// Sets the default name of an .env file.
        /// </summary>
        /// <param name="envFileName">The default name to set.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader SetDefaultEnvFileName(string envFileName);

        /// <summary>
        /// Sets the base path for the .env files.
        /// </summary>
        /// <param name="basePath">The base path to set.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader SetBasePath(string basePath);

        /// <summary>
        /// Adds the .env files in a collection.
        /// </summary>
        /// <param name="paths">The .env files paths to add.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader AddEnvFiles(params string[] paths);

        /// <summary>
        /// Adds an .env file in a collection.
        /// </summary>
        /// <param name="path">The .env file path to add.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader AddEnvFile(string path);

        /// <summary>
        /// Adds an .env file with its encoding in a collection.
        /// </summary>
        /// <param name="path">The .env file path to add.</param>
        /// <param name="encoding">The encoding of the .env file.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader AddEnvFile(string path, Encoding encoding);

        /// <summary>
        /// Sets the encoding of the .env files.
        /// </summary>
        /// <param name="encoding">The type of encoding to set.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader SetEncoding(Encoding encoding);

        /// <summary>
        /// Enables <see cref="FileNotFoundException" /> when the .env file is not found.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvLoader EnableFileNotFoundException();

        /// <inheritdoc cref="IEnvParser.DisableTrimStartValues" />
        IEnvLoader DisableTrimStartValues();

        /// <inheritdoc cref="IEnvParser.DisableTrimEndValues" />
        IEnvLoader DisableTrimEndValues();

        /// <inheritdoc cref="IEnvParser.DisableTrimValues" />
        IEnvLoader DisableTrimValues();

        /// <inheritdoc cref="IEnvParser.DisableTrimStartKeys" />
        IEnvLoader DisableTrimStartKeys();

        /// <inheritdoc cref="IEnvParser.DisableTrimEndKeys" />
        IEnvLoader DisableTrimEndKeys();

        /// <inheritdoc cref="IEnvParser.DisableTrimKeys" />
        IEnvLoader DisableTrimKeys();

        /// <inheritdoc cref="IEnvParser.DisableTrimStartComments" />
        IEnvLoader DisableTrimStartComments();

        /// <inheritdoc cref="IEnvParser.AllowOverwriteExistingVars" />
        IEnvLoader AllowOverwriteExistingVars();

        /// <inheritdoc cref="IEnvParser.SetCommentChar" />
        IEnvLoader SetCommentChar(char commentChar);

        /// <inheritdoc cref="IEnvParser.SetDelimiterKeyValuePair" />
        IEnvLoader SetDelimiterKeyValuePair(char separator);

        /// <inheritdoc cref="IEnvParser.AllowConcatDuplicateKeys" />
        IEnvLoader AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End);

        /// <inheritdoc cref="IEnvParser.DisableParserException" />
        IEnvLoader DisableParserException();
    }
}
