using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEnv.Core;

internal static class TypeExtensions
{
    /// <summary>
    /// Returns all public and non-public properties.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>
    /// An array of <see cref="PropertyInfo" /> objects representing all public and non-public properties of the current <see cref="Type" />.
    /// </returns>
    public static PropertyInfo[] GetPublicAndNonPublicProperties(this Type type)
        => type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
}