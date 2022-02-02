using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    internal static class ListExtension
    {
        /// <summary>
        /// Check if the .env file exists in any directory.
        /// </summary>
        /// <param name="envFiles">The collection of .env files.</param>
        /// <param name="envFileName">The name of the .env file.</param>
        /// <returns>true if the .env file does not exist, otherwise false.</returns>
        public static bool NotExists(this List<EnvFile> envFiles, string envFileName)
            => !envFiles.Find(envFile => envFile.Path.IndexOf(envFileName) != -1).Exists;
    }
}