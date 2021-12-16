using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// The exception that is thrown when the parser finds an error during the parsing of the .env file.
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

        public override string Message
        {
            get
            {
                if (_actualValue != null && _currentLine != null)
                    return $"{base.Message} (Actual Value: {_actualValue}, Line: {_currentLine})";

                if (_actualValue != null)
                    return $"{base.Message} (Actual Value: {_actualValue})";

                if (_currentLine != null)
                    return $"{base.Message} (Line: {_currentLine})";

                return base.Message;
            }
        }
    }
}
