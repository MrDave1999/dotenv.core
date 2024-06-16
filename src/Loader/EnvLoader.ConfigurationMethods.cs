using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core;

// This class defines the configuration methods that will be used to change the behavior of the loader and parser.
public partial class EnvLoader
{
    /// <inheritdoc />
    public IEnvLoader SetDefaultEnvFileName(string envFileName)
    {
        ThrowHelper.ThrowIfNull(envFileName, nameof(envFileName));
        _configuration.DefaultEnvFileName = envFileName;
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader SetBasePath(string basePath)
    {
        ThrowHelper.ThrowIfNull(basePath, nameof(basePath));
        _configuration.BasePath = basePath;
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader AddEnvFiles(params string[] paths)
    {
        ThrowHelper.ThrowIfNull(paths, nameof(paths));
        ThrowHelper.ThrowIfEmptyCollection(paths, nameof(paths));
        foreach (string path in paths)
            AddEnvFile(path);
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader AddEnvFile(string path)
        => AddEnvFile(path, (Encoding)null, false);

    /// <inheritdoc />
    public IEnvLoader AddEnvFile(string path, Encoding encoding)
        => AddEnvFile(path, encoding, false);

    /// <inheritdoc />
    public IEnvLoader AddEnvFile(string path, Encoding encoding, bool optional)
    {
        ThrowHelper.ThrowIfNull(path, nameof(path));
        _configuration.EnvFiles.Add(new EnvFile { Path = path, Encoding = encoding, Optional = optional});
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader AddEnvFile(string path, string encodingName)
        => AddEnvFile(path, encodingName, false);

    /// <inheritdoc />
    public IEnvLoader AddEnvFile(string path, string encodingName, bool optional)
    {
        ThrowHelper.ThrowIfNull(path, nameof(path));
        ThrowHelper.ThrowIfNull(encodingName, nameof(encodingName));
        try
        {
            AddEnvFile(path, Encoding.GetEncoding(encodingName), optional);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException(string.Format(EncodingNotFoundMessage, encodingName), nameof(encodingName));
        }
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader AddEnvFile(string path, bool optional)
        => AddEnvFile(path, (Encoding)null, optional);

    /// <inheritdoc />
    public IEnvLoader SetEncoding(Encoding encoding)
    {
        ThrowHelper.ThrowIfNull(encoding, nameof(encoding));
        _configuration.Encoding = encoding;
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader SetEncoding(string encodingName)
    {
        try
        {
            _configuration.Encoding = Encoding.GetEncoding(encodingName);
        }
        catch(ArgumentException)
        {
            throw new ArgumentException(string.Format(EncodingNotFoundMessage, encodingName), nameof(encodingName));
        }
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader EnableFileNotFoundException()
    {
        _configuration.ThrowFileNotFoundException = true;
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader SetEnvironmentName(string envName)
    {
        ThrowHelper.ThrowIfNull(envName, nameof(envName));
        ThrowHelper.ThrowIfNullOrWhiteSpace(envName, nameof(envName));
        _configuration.EnvironmentName = envName;
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader AllowAllEnvFilesOptional()
    {
        _configuration.Optional = true;
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader IgnoreParentDirectories()
    {
        _configuration.SearchParentDirectories = false;
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
    public IEnvLoader DisableTrimKeys()
    {
        _parser.DisableTrimKeys();
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

    /// <inheritdoc />
    public IEnvLoader AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End)
    {
        _parser.AllowConcatDuplicateKeys(option);
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader IgnoreParserException()
    {
        _parser.IgnoreParserException();
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader AvoidModifyEnvironment()
    {
        _parser.AvoidModifyEnvironment();
        return this;
    }

    /// <inheritdoc />
    public IEnvLoader SetEnvironmentVariablesProvider(IEnvironmentVariablesProvider provider)
    {
        _parser.SetEnvironmentVariablesProvider(provider);
        return this;
    }
}
