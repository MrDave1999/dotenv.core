using System.Globalization;

namespace DotEnv.Core;

/// <summary>
/// Represents the Main Helper of DotEnv.
/// </summary>
internal class DotEnvHelper
{
    /// <summary>
    /// Returns an object of the specified type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="conversionType">The type of object to return.</param>
    /// <returns>An object whose type is <c>conversionType</c>.</returns>
    /// <exception cref="ArgumentException">
    /// <c>value</c> is either an empty string ("") or only contains white space.
    /// -or-
    /// <c>value</c> is a name, but not one of the named constants defined for the enumeration.
    /// </exception>
    /// <exception cref="FormatException">
    /// <c>value</c> is not in a format recognized by <c>conversionType</c>.
    /// </exception>
    public static object ChangeType(string value, Type conversionType)
        => conversionType.IsEnum ? 
                Enum.Parse(conversionType, value, ignoreCase: true) : 
                Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);

    /// <summary>
    /// Checks if the passed elements are not null.
    /// </summary>
    /// <param name="elements">The elements to validate.</param>
    /// <returns>true if all elements are not null, or false.</returns>
    public static bool AreNotNull(params object[] elements)
        => !elements.Contains(null);
}
