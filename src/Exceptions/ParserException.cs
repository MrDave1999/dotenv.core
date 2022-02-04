using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// The exception that is thrown when the parser encounters one or more errors.
    /// </summary>
    public class ParserException : Exception
    {
        private readonly object _actualValue;
        private readonly int? _currentLine;

        /// <summary>
        /// Allows access to the actual value that causes the exception.
        /// </summary>
        public object ActualValue => _actualValue;

        /// <summary>
        /// Allows access to the current line that causes the exception.
        /// </summary>
        public int? CurrentLine => _currentLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException" /> class with the a specified error message, the actual value and the current line number.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="actualValue">The actual value that caused the exception.</param>
        /// <param name="currentLine">The current line that caused the exception.</param>
        public ParserException(
            string message, 
            object actualValue = null, 
            int? currentLine = null) : base(message)
        {
            _actualValue = actualValue;
            _currentLine = currentLine;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message => FormatErrorMessage(base.Message, _actualValue, _currentLine);

        /// <summary>
        /// Formats an error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="actualValue">The actual value that caused the error.</param>
        /// <param name="lineNumber">The line number that caused the error.</param>
        /// <param name="envFileName">The name of the .env file that caused the error.</param>
        /// <returns>A formatted error message.</returns>
        internal static string FormatErrorMessage(string message, object actualValue = null, int? lineNumber = null, string envFileName = null)
        {
            if(actualValue != null && lineNumber != null && envFileName != null)
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
    }
}
