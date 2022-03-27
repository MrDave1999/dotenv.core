using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.EnvFileNames;

namespace DotEnv.Core
{
    /// <summary>
    /// Define methods that format error messages.
    /// </summary>
    public class FormattingMessage
    {
        /// <summary>
        /// Formats an error message in case the local file is not present in any directory.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="environmentName">The name of the current environment.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatLocalFileNotPresentMessage(string message = null, string environmentName = null)
        {
            message = message ?? "error: Any of these .env files must be present in the root directory of your project:";
            return environmentName != null ? $"{message} .env.{environmentName}.local or {EnvLocalName}" : $"{message} {EnvDevelopmentLocalName} or {EnvDevLocalName} or {EnvLocalName}";
        }

        /// <summary>
        /// Formats an error message in case the parser encounters errors.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="actualValue">The actual value that caused the error.</param>
        /// <param name="lineNumber">The line number that caused the error.</param>
        /// <param name="column">The column that caused the error.</param>
        /// <param name="envFileName">The name of the .env file that caused the error.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatParserExceptionMessage(string message, object actualValue = null, int? lineNumber = null, int? column = null, string envFileName = null)
        {
            if (envFileName != null && lineNumber != null && column != null && actualValue != null)
                return $"{envFileName}:(line {lineNumber}, col {column}): error: {string.Format(message, actualValue)}";

            if (lineNumber != null && column != null && actualValue != null)
                return $"Parsing error (line {lineNumber}, col {column}): error: {string.Format(message, actualValue)}";

            if (envFileName != null)
                return $"{envFileName}: error: {message}";

            return $"Parsing error: {message}";
        }
    }
}
