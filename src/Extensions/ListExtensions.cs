using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    internal static class ListExtensions
    {
        /// <summary>
        /// Checks if the collection is empty.
        /// </summary>
        /// <returns>true if the collecion is empty, or false</returns>
        public static bool IsEmpty<T>(this List<T> list)
            => list.Count == 0;
    }
}