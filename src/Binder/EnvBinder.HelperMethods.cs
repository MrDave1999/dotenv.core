using System.Reflection;
using System.Text;

namespace DotEnv.Core;

// This class defines the helper private methods.
public partial class EnvBinder
{
    /// <summary>
    /// Checks whether the property is read-only or write-only.
    /// </summary>
    /// <returns><c>true</c> if the property is read-only or write-only, or <c>false</c> if the property is read-write.</returns>
    private bool IsReadOnlyOrWriteOnly(PropertyInfo property)
        => !property.CanRead || !property.CanWrite;

    /// <summary>
    /// Converts the variable name to UpperCaseSnakeCase.
    /// </summary>
    /// <param name="variableName">The name of the variable to convert.</param>
    /// <param name="newKey">Save a new key with the 'UpperCaseSnakeCase' convention.</param>
    /// <returns>The variable name using the 'UpperCaseSnakeCase' convention.</returns>
    /// <example>
    /// This is the 'UpperCaseSnakeCase' convention:
    /// VARIABLE_NAME
    /// </example>
    private string ConvertToUpperCaseSnakeCase(ref string variableName, StringBuilder newKey)
    {
        int len = variableName.Length;
        newKey.Append(variableName[0]);
        for (int i = 1; i < len; i++)
            newKey.Append(char.IsUpper(variableName[i]) ? $"_{variableName[i]}" : char.ToUpper(variableName[i]));

        variableName = newKey.ToString();
        return variableName;
    }
}
