using System;
using System.Collections.Generic;

namespace DotEnv.Core;

/// <summary>
/// The exception that is thrown when the binder encounters one or more errors.
/// </summary>
public class BinderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinderException" /> class.
    /// </summary>
    public BinderException()
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