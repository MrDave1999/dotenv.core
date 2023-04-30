using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Represents the options for configuring various behaviors of the parser.
/// </summary>
internal class EnvParserOptions
{
    /// <summary>
    /// A value indicating whether to remove leading white-spaces from values. Its default value is <c>true</c>.
    /// </summary>
    public bool TrimStartValues { get; set; } = true;

    /// <summary>
    /// A value indicating whether to remove trailing white-spaces from values. Its default value is <c>true</c>.
    /// </summary>
    public bool TrimEndValues { get; set; } = true;

    /// <summary>
    /// A value indicating whether to remove leading white-spaces from keys. Its default value is <c>true</c>.
    /// </summary>
    public bool TrimStartKeys { get; set; } = true;

    /// <summary>
    /// A value indicating whether to remove trailing white-spaces from keys. Its default value is <c>true</c>.
    /// </summary>
    public bool TrimEndKeys { get; set; } = true;

    /// <summary>
    /// A value indicating whether to remove leading white-spaces from comments. Its default value is <c>true</c>.
    /// </summary>
    public bool TrimStartComments { get; set; } = true;

    /// <summary>
    /// A value indicating whether the parser should overwrite the value of an existing variable. Its default value is <c>false</c>.
    /// </summary>
    public bool OverwriteExistingVars { get; set; }

    /// <summary>
    /// A character indicating the beginning of a comment. Its default value is <c>#</c>.
    /// </summary>
    public char CommentChar { get; set; } = '#';

    /// <summary>
    /// Gets or sets the characters indicating the beginning of a inline comment 
    /// that will be used as delimiter in the <see cref="EnvParser.RemoveInlineComment" /> method.
    /// </summary>
    public string[] InlineCommentChars { get; set; } = new[] { " #", "\t#" };

    /// <summary>
    /// A character that separates a key-value pair. Its default value is <c>=</c>.
    /// </summary>
    public char DelimiterKeyValuePair { get; set; } = '=';

    /// <summary>
    /// A value that indicates whether duplicate keys can be concatenated. Its default value is <c>null</c>.
    /// </summary>
    public ConcatKeysOptions? ConcatDuplicateKeys { get; set; }

    /// <summary>
    /// A value indicating whether the parser should throw an exception when it encounters one or more errors. Its default value is <c>true</c>.
    /// </summary>
    public bool ThrowException { get; set; } = true;

    /// <summary>
    /// Gets or sets the environment variables provider.
    /// </summary>
    public IEnvironmentVariablesProvider EnvVars { get; set; } = new DefaultEnvironmentProvider();
}
