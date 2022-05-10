using System;
using System.Collections.Generic;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents the options for configuring various behaviors of the binder.
    /// </summary>
    public class EnvBinderOptions
    {
        /// <summary>
        /// Gets or sets the environment variables provider.
        /// </summary>
        public IEnvironmentVariablesProvider EnvVars { get; set; } = new DefaultEnvironmentProvider();
    }
}