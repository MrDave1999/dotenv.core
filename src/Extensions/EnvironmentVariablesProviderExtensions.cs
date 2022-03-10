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
        /// <param name="provider">The environment variables provider.</param>
        /// <returns>An instance that implements the <see cref="IEnvValidator" /> interface.</returns>
        public static IEnvValidator CreateValidator(this IEnvironmentVariablesProvider provider)
            => new EnvValidator(provider);

        /// <summary>
        /// Creates an instance that implements the <see cref="IEnvReader" /> interface.
        /// </summary>
        /// <param name="provider">The environment variables provider.</param>
        /// <returns>An instance that implements the <see cref="IEnvReader" /> interface.</returns>
        public static IEnvReader CreateReader(this IEnvironmentVariablesProvider provider)
            => new EnvReader(provider);

        /// <summary>
        /// Converts the environment variables provider to a dictionary.
        /// </summary>
        /// <param name="provider">The environment variables provider.</param>
        /// <returns>A dictionary with the environment variables.</returns>
        public static Dictionary<string, string> ToDictionary(this IEnvironmentVariablesProvider provider)
        {
            var keyValuePairs = new Dictionary<string, string>();
            foreach (var keyValuePair in provider)
                keyValuePairs[keyValuePair.Key] = keyValuePair.Value;
            return keyValuePairs;
        }
    }
}
