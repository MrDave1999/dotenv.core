﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Defines the methods that control the parser behavior.
    /// </summary>
    public interface IEnvParser
    {
        /// <summary>
        /// Start the parsing to extract the key-value pair from the .env file.
        /// </summary>
        /// <param name="input">The input to parsing.</param>
        /// <exception cref="ParserException">If the parser find an error during the parsing.</exception>
        void Parse(string input);

        /// <summary>
        /// Disables the trim at the starts of the values.
        /// This method will tell the parser not to remove leading white-spaces from the values.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimStartValues();

        /// <summary>
        /// Disables the trim at the end of the values.
        /// This method will tell the parser not to remove trailing white-spaces from the values.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimEndValues();

        /// <summary>
        /// Disables the trim at the starts of the keys.
        /// This method will tell the parser not to remove leading white-spaces from the keys.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimStartKeys();

        /// <summary>
        /// Disables the trim at the end of the keys.
        /// This method will tell the parser not to remove trailing white-spaces from the keys.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimEndKeys();

        /// <summary>
        /// Disables the trim at the starts of the comments.
        /// This method will tell the parser not to remove leading white-spaces from the comments.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimStartComments();

        /// <summary>
        /// Allows overwriting of existing environment variables.
        /// This method will tell the parser that it can overwrite the value of an existing variable, i.e. if the variable <c>KEY1</c> exists, its value can be overwritten.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser AllowOverwriteExistingVars();
    }
}