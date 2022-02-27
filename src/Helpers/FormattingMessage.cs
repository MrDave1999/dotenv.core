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
        /// Formats an error message in case the parser encounters errors.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="actualValue">The actual value that caused the error.</param>
        /// <param name="lineNumber">The line number that caused the error.</param>
        /// <param name="envFileName">The name of the .env file that caused the error.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatParserExceptionMessage(string message, object actualValue = null, int? lineNumber = null, string envFileName = null)
        {
            if (actualValue != null && lineNumber != null && envFileName != null)
                return $"{message} (Actual Value: {actualValue}, Line: {lineNumber}, FileName: {envFileName})";

            if (actualValue != null && lineNumber != null)
                return $"{message} (Actual Value: {actualValue}, Line: {lineNumber})";

            if (envFileName != null && lineNumber != null)
                return $"{message} (Line: {lineNumber}, FileName: {envFileName})";

            if (lineNumber != null)
                return $"{message} (Line: {lineNumber})";

            if (actualValue != null)
                return $"{message} (Actual Value: {actualValue})";

            if (envFileName != null)
                return $"{message} (FileName: {envFileName})";

            return message;
        }

        /// <summary>
        /// Formats an error message in case the .env file is not found in any directory.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="envFileName">The .env file name that caused the error.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatFileNotFoundExceptionMessage(string message, string envFileName = null)
            => envFileName != null ? $"{message} (FileName: {envFileName})" : message;

        /// <summary>
        /// Formats an error message in case the variable is not found in the current environment.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="variableName">The variable name that caused the error.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatEnvVariableNotFoundExceptionMessage(string message, string variableName = null)
            => variableName != null ? $"{message} (Variable Name: {variableName})" : message;

        /// <summary>
        /// Formats an error message in case the encoding is not supported.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="encodingName">The encoding name that caused the error.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatEncodingNotFoundMessage(string message, string encodingName = null)
            => encodingName != null ? $"'{encodingName}' {message}" : message;

        /// <summary>
        /// Format an error message in case the .env file is not present.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="environment">The name of the environment.</param>
        /// <returns>A formatted error message.</returns>
        public static string FormatFileNotPresentLoadEnvMessage(string message, string environment = null)
            => environment != null ? $"{message}: .env.{environment}.local or {EnvLocalName}" : $"{message}: {EnvDevelopmentLocalName} or {EnvDevLocalName} or {EnvLocalName}";
    }
}
