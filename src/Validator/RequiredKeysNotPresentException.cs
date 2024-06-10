﻿using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core;

/// <summary>
/// The exception that is thrown when the required keys are not present in the application.
/// </summary>
public class RequiredKeysNotPresentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequiredKeysNotPresentException" /> class with a default message.
    /// </summary>
    public RequiredKeysNotPresentException() : base(RequiredKeysNotPresentDefaultMessage)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequiredKeysNotPresentException" /> class with the a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RequiredKeysNotPresentException(string message) : base(message)
    {

    }
}
