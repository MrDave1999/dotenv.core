using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core;

// This class defines the helper private methods.
public partial class EnvLoader
{
    private void StartFileLoading(EnvFile envFile)
    {
        if (!Path.HasExtension(envFile.Path))
            envFile.Path = Path.Combine(envFile.Path, _configuration.DefaultEnvFileName);

        envFile.Encoding ??= _configuration.Encoding;
        envFile.Optional   = envFile.Optional ? envFile.Optional : _configuration.Optional;
        envFile.Path       = Path.Combine(_configuration.BasePath, envFile.Path);
        envFile.Exists     = ReadFileContents(envFile);

        if (envFile.NotExists && envFile.IsNotOptional)
            _validationResult.Add(errorMsg: string.Format(FileNotFoundMessage, envFile.Path));
    }

    /// <summary>
    /// Reads the contents of a .env file and invokes the parser.
    /// </summary>
    /// <param name="envFile">The instance representing the .env file.</param>
    /// <exception cref="ArgumentNullException"><c>envFile</c> is <c>null</c>.</exception>
    /// <returns>true if the .env file exists, otherwise false.</returns>
    private bool ReadFileContents(EnvFile envFile)
    {
        ThrowHelper.ThrowIfNull(envFile, nameof(envFile));
        Result<string> result;
        if (_configuration.SearchParentDirectories)
            result = GetEnvFilePath(envFile.Path);
        else
            result = File.Exists(envFile.Path) ? 
                Result<string>.Success(envFile.Path) : 
                Result<string>.Failure();

        if (result.IsFailed)
            return false;

        string fullPath = result.Value;
        string source = File.ReadAllText(fullPath, envFile.Encoding);
        _parser.FileName = envFile.Path;
        _parser.ParseStart(source);
        return true;
    }

    /// <summary>
    /// Gets the full path of the .env file.
    /// </summary>
    /// <param name="envFileName">
    /// The name of the .env file to search for.
    /// The .env file name can include an absolute or relative path.
    /// </param>
    /// <returns>
    /// A result with the path to the .env file, otherwise it returns a failure result if the path is not found.
    /// </returns>
    /// <exception cref="ArgumentNullException"><c>envFileName</c> is <c>null</c>.</exception>
    /// <inheritdoc cref="Load()" path="/remarks" />
    private Result<string> GetEnvFilePath(string envFileName)
    {
        ThrowHelper.ThrowIfNull(envFileName, nameof(envFileName));
        string path;
        if (Path.IsPathRooted(envFileName))
        {
            path = Path.GetDirectoryName(envFileName);
            envFileName = Path.GetFileName(envFileName);
        }
        else
            path = AppContext.BaseDirectory;

        for (var directoryInfo = new DirectoryInfo(path);
            directoryInfo is not null;
            directoryInfo = directoryInfo.Parent)
        {
            string fullName = Path.Combine(directoryInfo.FullName, envFileName);
            if (File.Exists(fullName))
                return Result<string>.Success(fullName);
        }
        return Result<string>.Failure();
    }

    /// <summary>
    /// Throws an exception if there are errors.
    /// </summary>
    /// <exception cref="FileNotFoundException"></exception>
    private void ThrowFileNotFoundIfErrorsExist()
    {
        if (_validationResult.HasError())
        {
            if (_configuration.ThrowFileNotFoundException)
                throw new FileNotFoundException(message: _validationResult.ErrorMessages);

            CombineValidationResults();
        }
    }

    /// <summary>
    /// Combines the validation result of the loader with the parser.
    /// </summary>
    private void CombineValidationResults()
    {
        if (_parser.ValidationResult.HasError())
            _parser.ValidationResult.AddRange(errorMessages: _validationResult);
    }

    /// <summary>
    /// Gets an instance of validation result.
    /// </summary>
    private EnvValidationResult GetInstanceOfValidationResult()
        => _parser.ValidationResult.HasError() ? _parser.ValidationResult : _validationResult;

    /// <summary>
    /// Adds optional .env files to a collection.
    /// </summary>
    /// <param name="envFilesNames">The names of the .env files.</param>
    /// <exception cref="ArgumentNullException"><c>envFilesNames</c> is <c>null</c>.</exception>
    private void AddOptionalEnvFiles(params string[] envFilesNames)
    {
        ThrowHelper.ThrowIfNull(envFilesNames, nameof(envFilesNames));
        foreach (string envFileName in envFilesNames)
            _configuration.EnvFiles.Add(new EnvFile { Path = envFileName, Optional = true });
    }
}
