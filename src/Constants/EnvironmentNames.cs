using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

internal class EnvironmentNames
{
    public static IReadOnlyList<string> Development { get; } = new[] { "development", "dev"};
    public static IReadOnlyList<string> Test { get; } = new[] { "test" };
    public static IReadOnlyList<string> Staging { get; } = new[] { "staging" };
    public static IReadOnlyList<string> Production { get; } = new[] { "production", "prod"};
}
