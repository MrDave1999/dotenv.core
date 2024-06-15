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

    /// <summary>
    /// Changes the data type of a value to another type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="conversionType">The type of object to return.</param>
    /// <returns>
    /// A result with the converted value, otherwise returns a failure result.
    /// </returns>
    private Result<object> ChangeType(string value, Type conversionType)
    {
        try
        {
            var convertedValue = DotEnvHelper.ChangeType(value, conversionType);
            return Result<object>.Success(convertedValue);
        }
        catch (Exception ex) 
            when (ex is FormatException || 
                  ex is ArgumentException)
        {
            return Result<object>.Failure();
        }
    }
}
