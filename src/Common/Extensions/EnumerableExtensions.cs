using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotEnv.Core;

internal static class EnumerableExtensions
{
    /// <summary>
    /// Checks if the collection is empty.
    /// </summary>
    /// <returns>true if the collecion is empty, or false.</returns>
    public static bool IsEmpty<T>(this IEnumerable<T> list)
        => !list.Any();
}