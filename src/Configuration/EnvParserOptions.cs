using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents the options for configuring various behaviors of the parser.
    /// </summary>
    public class EnvParserOptions
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
        /// A value indicating whether to overwrite the value of an existing environment variable. Its default value is <c>false</c>.
        /// </summary>
        public bool OverwriteExistingVars { get; set; }
    }
}
