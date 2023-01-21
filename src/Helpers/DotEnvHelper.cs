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
    public static object ChangeType(string value, Type conversionType)
    {
        if(conversionType.IsEnum)
        {
            try
            {
                return Enum.Parse(conversionType, value, ignoreCase: true);
            }
            catch(ArgumentException)
            {
                throw new FormatException();
            }
        }
        return Convert.ChangeType(value, conversionType);
    }

    /// <summary>
    /// Checks if the passed elements are not null.
    /// </summary>
    /// <param name="elements">The elements to validate.</param>
    /// <returns>true if all elements are not null, or false.</returns>
    public static bool AreNotNull(params object[] elements)
        => !elements.Contains(null);
}
