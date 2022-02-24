using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents the options for configuring various behaviors of the validator.
    /// </summary>
    internal class EnvValidatorOptions
    {
        /// <summary>
        /// A value indicating whether the validator should throw an exception when it encounters one or more errors. Its default value is <c>true</c>.
        /// </summary>
        public bool ThrowException { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of required keys.
        /// </summary>
        public IEnumerable<string> RequiredKeys { get; set; }
    }
}