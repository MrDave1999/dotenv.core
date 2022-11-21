using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

internal class EnvironmentNames
{
    public static string[] Development { get; } = new[] { "development", "dev"};
    public static string[] Test { get; } = new[] { "test" };
    public static string[] Staging { get; } = new[] { "staging" };
    public static string[] Production { get; } = new[] { "production", "prod"};
}
