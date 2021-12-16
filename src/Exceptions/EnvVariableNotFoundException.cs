﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// The exception that is thrown when the environment variable is not found in the current process.
    /// </summary>
    public class EnvVariableNotFoundException : Exception
    {
        private readonly string variableName;

        /// <summary>
        /// Allows access to the name of the variable that causes the exception.
        /// </summary>
        public string VariableName => variableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvVariableNotFoundException" /> class with the a specified error message, the name and value of the variable.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="variableName">The variable name that caused the exception.</param>
        public EnvVariableNotFoundException(
            string message, 
            string variableName) : base(message)
        {
            this.variableName = variableName;
        }

        public override string Message => $"{base.Message} (Variable Name: {variableName})";
    }
}
