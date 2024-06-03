using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

internal class EnvironmentNames
{
    public static IReadOnlyList<string> Development { get; } = ["development", "dev"];
    public static IReadOnlyList<string> Test { get; } = ["test"];
    public static IReadOnlyList<string> Staging { get; } = ["staging"];
    public static IReadOnlyList<string> Production { get; } = ["production", "prod"];
}
