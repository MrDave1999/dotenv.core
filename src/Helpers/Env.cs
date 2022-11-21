using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Environment;

namespace DotEnv.Core;

/// <summary>
/// This class defines methods to perform checks with the current environment.
/// </summary>
public static class Env
{
    /// <summary>
    /// Gets or sets the current environment (dev, test, staging, or production).
    /// </summary>
    public static string CurrentEnvironment
    {
        get => GetEnvironmentVariable("DOTNET_ENV");
        set => SetEnvironmentVariable("DOTNET_ENV", value);
    }

    /// <summary>
    /// Checks if the current environment name is <c>development</c>, or <c>dev</c>.
    /// </summary>
    /// <returns><c>true</c> if the environment name is development or dev, otherwise <c>false</c>.</returns>
    public static bool IsDevelopment()
        => EnvironmentNames.Development.Contains(CurrentEnvironment, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the current environment name is <c>test</c>.
    /// </summary>
    /// <returns><c>true</c> if the environment name is test, otherwise <c>false</c>.</returns>
    public static bool IsTest()
        => EnvironmentNames.Test.Contains(CurrentEnvironment, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the current environment name is <c>staging</c>.
    /// </summary>
    /// <returns><c>true</c> if the environment name is staging, otherwise <c>false</c>.</returns>
    public static bool IsStaging()
        => EnvironmentNames.Staging.Contains(CurrentEnvironment, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the current environment name is <c>production</c>, or <c>prod</c>.
    /// </summary>
    /// <returns><c>true</c> if the environment name is production or prod, otherwise <c>false</c>.</returns>
    public static bool IsProduction()
        => EnvironmentNames.Production.Contains(CurrentEnvironment, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Compares the current environment name against the specified value.
    /// </summary>
    /// <param name="environmentName">The environment name to validate against.</param>
    /// <returns><c>true</c> if the specified name is the same as the current environment, otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><c>environmentName</c> is <c>null</c>.</exception>
    public static bool IsEnvironment(string environmentName)
    {
        _ = environmentName ?? throw new ArgumentNullException(nameof(environmentName));
        return string.Equals(CurrentEnvironment, environmentName, StringComparison.OrdinalIgnoreCase);
    }
}
