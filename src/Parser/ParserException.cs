﻿using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core;

/// <summary>
/// The exception that is thrown when the parser encounters one or more errors.
/// </summary>
public class ParserException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParserException" /> class with a default message.
    /// </summary>
    public ParserException() : base(ParserDefaultMessage)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserException" /> class with the a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ParserException(string message) : base(message)
    {
    }
}
