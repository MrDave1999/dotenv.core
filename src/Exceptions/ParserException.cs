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
        private readonly object actualValue;
        private readonly int? currentLine;

        /// <summary>
        /// Allows access to the actual value that causes the exception.
        /// </summary>
        public object ActualValue => actualValue;

        /// <summary>
        /// Allows access to the current line that causes the exception.
        /// </summary>
        public int? CurrentLine => currentLine;

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
            this.actualValue = actualValue;
            this.currentLine = currentLine;
        }

        public override string Message
        {
            get
            {
                if (actualValue != null && currentLine != null)
                    return $"{base.Message} (Actual Value: {actualValue}, Line: {currentLine})";

                if (actualValue != null)
                    return $"{base.Message} (Actual Value: {actualValue})";

                if (currentLine != null)
                    return $"{base.Message} (Line: {currentLine})";

                return base.Message;
            }
        }
    }
}
