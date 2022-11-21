using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// The exception that is thrown when the environment variable is not set to a specific provider (e.g., current process or a <see cref="Dictionary{TKey, TValue}" />).
/// </summary>
public class VariableNotSetException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableNotSetException" /> class.
    /// </summary>
    public VariableNotSetException()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableNotSetException" /> class with the a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public VariableNotSetException(string message) : base(message)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableNotSetException" /> class with the a specified error message, and the parameter name.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="paramName">The parameter name that caused the exception.</param>
    public VariableNotSetException(string message, string paramName) : base(message, paramName)
    {
    }
}
