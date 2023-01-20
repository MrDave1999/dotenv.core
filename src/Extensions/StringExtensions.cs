using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

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

    /// <summary>
    /// Determines whether this string instance starts with the specified character. 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value">The character to compare.</param>
    /// <returns>true if value matches the beginning of this string; otherwise, false.</returns>
    public static bool StartsWith(this string str, char value)
        => str.Length != 0 && str[0] == value;

    /// <summary>
    /// Determines whether the end of this string instance matches the specified character.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value">The character to compare to the character at the end of this instance.</param>
    /// <returns>true if value matches the end of this instance; otherwise, false.</returns>
    public static bool EndsWith(this string str, char value)
    {
        int len = str.Length;
        return len != 0 && str[len - 1] == value;
    }
}
