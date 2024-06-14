using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.DotEnvHelper;

namespace DotEnv.Core;

/// <summary>
/// Define methods that format error messages.
/// </summary>
internal class FormattingMessage
{
    /// <summary>
    /// Formats an error message in case the parser encounters errors.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="actualValue">The actual value that caused the error.</param>
    /// <param name="lineNumber">The line number that caused the error.</param>
    /// <param name="column">The column that caused the error.</param>
    /// <param name="envFileName">The name of the .env file that caused the error.</param>
    /// <returns>A formatted error message.</returns>
    public static string FormatParserExceptionMessage(
        string message, 
        object actualValue = null, 
        int? lineNumber = null, 
        int? column = null, 
        string envFileName = null)
    {
        if(AreNotNull(envFileName, lineNumber, column, actualValue))
            return $"{envFileName}:(line {lineNumber}, col {column}): error: {string.Format(message, actualValue)}";

        if(AreNotNull(lineNumber, column, actualValue))
            return $"Parsing error (line {lineNumber}, col {column}): error: {string.Format(message, actualValue)}";

        if(AreNotNull(envFileName, lineNumber, column))
            return $"{envFileName}:(line {lineNumber}, col {column}): error: {message}";

        if(AreNotNull(lineNumber, column))
            return $"Parsing error (line {lineNumber}, col {column}): error: {message}";

        if (envFileName is not null)
            return $"{envFileName}: error: {message}";

        return $"Parsing error: {message}";
    }
}
