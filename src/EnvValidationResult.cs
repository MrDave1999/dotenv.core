using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents a container for the results of a validation of the parser and loader.
    /// </summary>
    public class EnvValidationResult : IEnumerable<string>
    {
        /// <summary>
        /// Allows access to the errors collection.
        /// </summary>
        private readonly List<string> _errors = new List<string>();

        /// <summary>
        /// Check if there has been an error.
        /// </summary>
        /// <returns><c>true</c> if an error occurred, otherwise <c>false</c>.</returns>
        public bool HasError()
            => _errors.Count > 0;

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        public string ErrorMessages
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach(var error in _errors)
                    stringBuilder.Append(error + Environment.NewLine);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Adds the error message to the collection.
        /// </summary>
        /// <param name="errorMsg">The message that describes the error.</param>
        internal void Add(string errorMsg)
        {
            _errors.Add(errorMsg);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the errors contained in the container.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the errors contained in the container.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var error in _errors)
                yield return error;
        }

        /// <inheritdoc cref="GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
