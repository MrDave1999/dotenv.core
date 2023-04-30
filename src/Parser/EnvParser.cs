using System;
using System.Collections.Generic;
using System.IO;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core;

/// <inheritdoc cref="IEnvParser" />
public partial class EnvParser : IEnvParser
{
    private static readonly string[] s_newLines = new[] { "\r\n", "\n", "\r" };

    private const string ExportPrefix = "export ";
    private const char DoubleQuote    = '"';
    private const char SingleQuote    = '\'';

    /// <summary>
    /// The maximum number of substrings to be returned by the Split method.
    /// </summary>
    private const int MaxCount = 2;

    /// <summary>
    /// Allows access to the configuration options for the parser.
    /// </summary>
    private readonly EnvParserOptions _configuration = new();

    /// <summary>
    /// Allows access to the errors container of the parser.
    /// </summary>
    internal EnvValidationResult ValidationResult { get; } = new EnvValidationResult();

    /// <summary>
    /// Allows access to the name of the file that caused an error.
    /// This property is for the loader to pass data to the parser.
    /// </summary>
    internal string FileName { get; set; }

    /// <summary>
    /// Allows access to the environment variables provider.
    /// </summary>
    internal IEnvironmentVariablesProvider EnvVarsProvider 
        => _configuration.EnvVars;

    /// <inheritdoc />
    public IEnvironmentVariablesProvider Parse(string dataSource)
        => Parse(dataSource, out _);

    /// <inheritdoc />
    public IEnvironmentVariablesProvider Parse(string dataSource, out EnvValidationResult result)
    {
        _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        result = ValidationResult;

        if (string.IsNullOrWhiteSpace(dataSource))
        {
            ValidationResult.Add(errorMsg: FormatParserExceptionMessage(
                DataSourceIsEmptyOrWhitespaceMessage, 
                envFileName: FileName
            ));
            CreateAndThrowParserException();
            return _configuration.EnvVars;
        }

        var lines = dataSource.Split(s_newLines, StringSplitOptions.None);
        for (int i = 0, len = lines.Length; i < len; ++i)
        {
            var line = lines[i];
            int currentLine = i + 1;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (IsComment(line))
                continue;

            line = RemoveInlineComment(line, out var removedComment);
            if (HasNoKeyValuePair(line))
            {
                ValidationResult.Add(errorMsg: FormatParserExceptionMessage(
                    LineHasNoKeyValuePairMessage, 
                    actualValue: line, 
                    lineNumber: currentLine, 
                    column: 1, 
                    envFileName: FileName
                ));
                continue;
            }

            line = ExpandEnvironmentVariables(line, currentLine);

            var key   = ExtractKey(line);
            var value = ExtractValue(line);

            key   = RemovePrefixBeforeKey(key, ExportPrefix);
            key   = TrimKey(key);
            if (IsQuoted(value))
                value = RemoveQuotes(value);
            else if (IsMultiline(value))
            {
                value = GetValuesMultilines(lines, ref i, ConcatCommentWithValue(value, removedComment));
                if (value is null)
                    continue;
            }
            else
                value = TrimValue(value);
            value = ConvertStringEmptyToWhitespace(value);

            var retrievedValue = EnvVarsProvider[key];
            if (retrievedValue is null)
                EnvVarsProvider[key] = value;
            else if (_configuration.ConcatDuplicateKeys is not null)
                EnvVarsProvider[key] = ConcatValues(retrievedValue, value);
            else if (_configuration.OverwriteExistingVars)
                EnvVarsProvider[key] = value;
        }

        CreateAndThrowParserException();
        return _configuration.EnvVars;
    }
}
