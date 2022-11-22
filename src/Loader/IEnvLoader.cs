using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DotEnv.Core;

/// <summary>
/// Represents the .env files loader.
/// </summary>
public interface IEnvLoader
{
    /// <param name="result">The result contains the errors found by the loader.</param>
    /// <inheritdoc cref="LoadEnv()" />
    IEnvironmentVariablesProvider LoadEnv(out EnvValidationResult result);

    /// <summary>
    /// Loads an .env file based on the environment (development, test, staging or production).
    /// This method will load these .env files in the following order:
    /// <list type="bullet">
    /// <item><c>.env.[environment].local</c> (has the highest priority).</item>
    /// <item><c>.env.local</c></item>
    /// <item><c>.env.[environment]</c></item>
    /// <item><c>.env</c> (has the lowest priority).</item>
    /// </list>
    /// The <c>environment</c> is specified by the actual environment variable <c>DOTNET_ENV</c>.
    /// <para>It should be noted that the default environment will be <c>development</c> or <c>dev</c> if the environment is never specified with <c>DOTNET_ENV</c>.</para>
    /// </summary>
    /// <inheritdoc cref="Load()" />
    IEnvironmentVariablesProvider LoadEnv();

    /// <param name="result">The result contains the errors found by the loader.</param>
    /// <inheritdoc cref="Load()" />
    IEnvironmentVariablesProvider Load(out EnvValidationResult result);

    /// <summary>
    /// Loads one or more .env files. By default, it will search for a file called <c>.env</c>.
    /// </summary>
    /// <remarks>This method starts find the .env file in the current directory and if it does not found it, it starts find in the parent directories of the current directory.</remarks>
    /// <exception cref="ParserException">
    /// If the parser encounters one or more errors.
    /// This exception is not thrown if the <see cref="IgnoreParserException" /> method is invoked.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// If the .env files are not found.
    /// This exception is only thrown if the <see cref="EnableFileNotFoundException" /> method is invoked.
    /// </exception>
    /// <returns>An instance representing the provider of environment variables.</returns>
    IEnvironmentVariablesProvider Load();

    /// <summary>
    /// Sets the default name of an .env file.
    /// </summary>
    /// <param name="envFileName">The default name to set.</param>
    /// <exception cref="ArgumentNullException"><c>envFileName</c> is <c>null</c>.</exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader SetDefaultEnvFileName(string envFileName);

    /// <summary>
    /// Sets the base path for the .env files.
    /// </summary>
    /// <param name="basePath">The base path to set.</param>
    /// <exception cref="ArgumentNullException"><c>basePath</c> is <c>null</c>.</exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader SetBasePath(string basePath);

    /// <summary>
    /// Adds the .env files in a collection.
    /// </summary>
    /// <param name="paths">The .env files paths to add.</param>
    /// <exception cref="ArgumentNullException"><c>paths</c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The length of the <c>paths</c> list is zero.</exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader AddEnvFiles(params string[] paths);

    /// <summary>
    /// Adds an .env file in a collection.
    /// </summary>
    /// <param name="path">The .env file path to add.</param>
    /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception> 
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader AddEnvFile(string path);

    /// <summary>
    /// Adds an .env file with its encoding in a collection.
    /// </summary>
    /// <param name="path">The .env file path to add.</param>
    /// <param name="encoding">The encoding of the .env file.</param>
    /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception> 
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader AddEnvFile(string path, Encoding encoding);

    /// <param name="path">The .env file path to add.</param>
    /// <param name="encoding">The encoding of the .env file.</param>
    /// <param name="optional">The value indicating whether the existence of the .env file is optional, or not.</param>
    /// <inheritdoc cref="AddEnvFile(string, Encoding)" />
    IEnvLoader AddEnvFile(string path, Encoding encoding, bool optional);

    /// <summary>
    /// Adds an .env file with its encoding name in a collection.
    /// </summary>
    /// <param name="path">The .env file path to add.</param>
    /// <param name="encodingName">The encoding name of the .env file.</param>
    /// <exception cref="ArgumentNullException"><c>path</c>, or <c>encodingName</c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    /// <c>encodingName</c> is not a valid code page name or
    /// is not supported by the underlying platform.
    /// </exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader AddEnvFile(string path, string encodingName);

    /// <param name="path">The .env file path to add.</param>
    /// <param name="encodingName">The encoding name of the .env file.</param>
    /// <param name="optional">The value indicating whether the existence of the .env file is optional, or not.</param>
    /// <inheritdoc cref="AddEnvFile(string, string)" />
    IEnvLoader AddEnvFile(string path, string encodingName, bool optional);

    /// <summary>
    /// Adds an .env file to a collection and indicates whether the .env file can be optional, or not.
    /// </summary>
    /// <param name="path">The .env file path to add.</param>
    /// <param name="optional">The value indicating whether the existence of the .env file is optional, or not.</param>
    /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader AddEnvFile(string path, bool optional);

    /// <summary>
    /// Sets the encoding of the .env files.
    /// </summary>
    /// <param name="encoding">The type of encoding to set.</param>
    /// <exception cref="ArgumentNullException"><c>encoding</c> is <c>null</c>.</exception> 
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader SetEncoding(Encoding encoding);

    /// <summary>
    /// Sets the encoding name of the .env files.
    /// </summary>
    /// <param name="encodingName">The name of encoding to set.</param>
    /// <exception cref="ArgumentNullException"><c>encodingName</c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    /// <c>encodingName</c> is not a valid code page name or
    /// is not supported by the underlying platform.
    /// </exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader SetEncoding(string encodingName);

    /// <summary>
    /// Enables <see cref="FileNotFoundException" />. This method tells the loader to throw an exception when one or more .env files are not found.
    /// </summary>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader EnableFileNotFoundException();

    /// <summary>
    /// Sets the name of the environment.
    /// </summary>
    /// <param name="envName">The name of the environment.</param>
    /// <exception cref="ArgumentNullException"><c>envName</c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    /// <c>envName</c> is a <see cref="string.Empty" /> or consists only of white-space characters.
    /// </exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader SetEnvironmentName(string envName);

    /// <summary>
    /// Allows all .env files to be optional. This tells the loader not to raise an error in case the .env file is not found in any directory.
    /// </summary>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader AllowAllEnvFilesOptional();

    /// <summary>
    /// Ignores search in parent directories. This tells the loader not to search in parent directories when the .env file is not in a directory.
    /// </summary>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvLoader IgnoreParentDirectories();

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

    /// <inheritdoc cref="IEnvParser.IgnoreParserException" />
    IEnvLoader IgnoreParserException();

    /// <inheritdoc cref="IEnvParser.AvoidModifyEnvironment" />
    IEnvLoader AvoidModifyEnvironment();

    /// <inheritdoc cref="IEnvParser.SetEnvironmentVariablesProvider" />
    IEnvLoader SetEnvironmentVariablesProvider(IEnvironmentVariablesProvider provider);
}
