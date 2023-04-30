using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

// This class defines the configuration methods that will be used to change the behavior of the parser.
public partial class EnvParser
{
    /// <inheritdoc />
    public IEnvParser DisableTrimStartValues()
    {
        _configuration.TrimStartValues = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser DisableTrimEndValues()
    {
        _configuration.TrimEndValues = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser DisableTrimValues()
    {
        DisableTrimStartValues();
        DisableTrimEndValues();
        return this;
    }

    /// <inheritdoc />
    public IEnvParser DisableTrimStartKeys()
    {
        _configuration.TrimStartKeys = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser DisableTrimEndKeys()
    {
        _configuration.TrimEndKeys = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser DisableTrimKeys()
    {
        DisableTrimStartKeys();
        DisableTrimEndKeys();
        return this;
    }

    /// <inheritdoc />
    public IEnvParser DisableTrimStartComments()
    {
        _configuration.TrimStartComments = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser AllowOverwriteExistingVars()
    {
        _configuration.OverwriteExistingVars = true;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser SetCommentChar(char commentChar)
    {
        _configuration.CommentChar = commentChar;
        _configuration.InlineCommentChars = new[] { $" {commentChar}", $"\t{commentChar}" };
        return this;
    }

    /// <inheritdoc />
    public IEnvParser SetDelimiterKeyValuePair(char separator)
    {
        _configuration.DelimiterKeyValuePair = separator;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End)
    {
        if (option < ConcatKeysOptions.Start || option > ConcatKeysOptions.End)
            throw new ArgumentException(ExceptionMessages.OptionInvalidMessage, nameof(option));

        _configuration.ConcatDuplicateKeys = option;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser IgnoreParserException()
    {
        _configuration.ThrowException = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvParser AvoidModifyEnvironment()
    {
        _configuration.EnvVars = new DictionaryProvider();
        return this;
    }

    /// <inheritdoc />
    public IEnvParser SetEnvironmentVariablesProvider(IEnvironmentVariablesProvider provider)
    {
        _configuration.EnvVars = provider;
        return this;
    }
}
