using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

internal class EnvFileNames
{
    public const string EnvDevelopmentLocalName = ".env.development.local";
    public const string EnvDevLocalName         = ".env.dev.local";
    public const string EnvLocalName            = ".env.local";
    public const string EnvDevelopmentName      = ".env.development";
    public const string EnvDevName              = ".env.dev";
    public const string EnvName                 = ".env";

    /// <summary>
    /// Formats an error message in case the local file is not present in any directory.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="environmentName">The name of the current environment.</param>
    /// <returns>A formatted error message.</returns>
    public static string FormatLocalFileNotPresentMessage(string message = null, string environmentName = null)
    {
        message ??= "error: Any of these .env files must be present in the root directory of your project:";
        return environmentName is not null ?
            $"{message} .env.{environmentName}.local or {EnvLocalName}" :
            $"{message} {EnvDevelopmentLocalName} or {EnvDevLocalName} or {EnvLocalName}";
    }
}
