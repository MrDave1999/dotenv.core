using System.Reflection;

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
}
