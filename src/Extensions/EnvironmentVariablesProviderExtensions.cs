using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Extension methods for adding <see cref="IEnvironmentVariablesProvider"/>.
    /// </summary>
    public static class EnvironmentVariablesProviderExtensions
    {
        /// <summary>
        /// Creates an instance that implements the <see cref="IEnvValidator" /> interface.
        /// </summary>
        /// <param name="envVars">The environment variables provider.</param>
        /// <returns>An instance that implements the <see cref="IEnvValidator" /> interface.</returns>
        public static IEnvValidator CreateValidator(this IEnvironmentVariablesProvider envVars)
            => new EnvValidator(envVars);

        /// <summary>
        /// Creates an instance that implements the <see cref="IEnvReader" /> interface.
        /// </summary>
        /// <param name="envVars">The environment variables provider.</param>
        /// <returns>An instance that implements the <see cref="IEnvReader" /> interface.</returns>
        public static IEnvReader CreateReader(this IEnvironmentVariablesProvider envVars)
            => new EnvReader(envVars);

        /// <summary>
        /// Converts the environment variables provider to a dictionary.
        /// </summary>
        /// <param name="envVars">The environment variables provider.</param>
        /// <returns>A dictionary with the environment variables.</returns>
        public static Dictionary<string, string> ToDictionary(this IEnvironmentVariablesProvider envVars)
        {
            var keyValuePairs = new Dictionary<string, string>();
            foreach (var keyValuePair in envVars)
                keyValuePairs[keyValuePair.Key] = keyValuePair.Value;
            return keyValuePairs;
        }
    }
}
