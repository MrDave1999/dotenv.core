using System;
using System.Collections.Generic;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core;

/// <summary>
/// The exception that is thrown when the binder encounters one or more errors.
/// </summary>
public class BinderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinderException" /> class with a default message.
    /// </summary>
    public BinderException() : base(BinderDefaultMessage)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinderException" /> class with the a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BinderException(string message) : base(message)
    {
    }
}