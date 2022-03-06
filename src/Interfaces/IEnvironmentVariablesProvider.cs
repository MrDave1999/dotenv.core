using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents the environment variables provider.
    /// </summary>
    public interface IEnvironmentVariablesProvider : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        /// <param name="variable">The variable to get or set.</param>
        /// <exception cref="ArgumentNullException"><c>variable</c> is <c>null</c>.</exception>
        /// <remarks>The property gets a <c>null</c> value in case the <c>variable</c> is not found in the provider.</remarks>
        string this[string variable] { get; set; }
    }
}
