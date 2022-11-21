using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Specifies the options for concatenation of duplicate keys such as whether to concatenate at the start or end of the value.
/// </summary>
public enum ConcatKeysOptions
{
    /// <summary>
    /// This option will tell the parser to concatenate at the beginning of the value of a duplicate key.
    /// </summary>
    Start,
    /// <summary>
    /// This option will tell the parser to concatenate at the end of the value of a duplicate key.
    /// </summary>
    End
}
