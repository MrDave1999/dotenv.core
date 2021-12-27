using System;
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
        /// Disables the trim at the start and end of the values.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimValues();

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
        /// Disables the trim at the start and end of the keys.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimKeys();

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

        /// <summary>
        /// Sets the character that will define the beginning of a comment.
        /// </summary>
        /// <param name="commentChar">The character that defines the beginning of a comment.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser SetCommentChar(char commentChar);

        /// <summary>
        /// Sets the delimiter that separates an assigment of a value to a key.
        /// </summary>
        /// <param name="separator">The character that separates the key-value pair.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser SetDelimiterKeyValuePair(char separator);

        /// <summary>
        /// Allows to concatenate the values of the duplicate keys.
        /// </summary>
        /// <param name="option">The option that indicates whether the concatenation is at the start or at the end of the value.</param>
        /// <exception cref="ArgumentException"><c>option</c> is not one of the <see cref="ConcatKeysOptions" /> values.</exception>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End);

        /// <summary>
        /// Ignores parser exceptions. By calling this method the parser will not throw any exceptions when it encounters an error.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser IgnoreParserExceptions();
    }
}
