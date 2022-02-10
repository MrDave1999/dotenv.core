using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Splits a string into a maximum number of substrings based on a specified delimiting character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">A character that delimits the substrings in this instance.</param>
        /// <param name="count">The maximum number of elements expected in the array.</param>
        /// <returns>An array that contains at most count substrings from this instance that are delimited by separator.</returns>
        public static string[] Split(this string str, char separator, int count)
            => str.Split(new[] { separator }, count);
    }
}
