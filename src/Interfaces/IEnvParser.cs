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
        /// <param name="dataSource">The data source to parsing.</param>
        /// <param name="result">The result contains the errors found by the parser.</param>
        /// <inheritdoc cref="Parse(string)" />
        IDictionary<string, string> Parse(string dataSource, out EnvValidationResult result);

        /// <summary>
        /// Start the parsing to extract the key-value pair from a data source.
        /// </summary>
        /// <param name="dataSource">The data source to parsing.</param>
        /// <exception cref="ParserException">
        /// If the parser encounters one or more errors.
        /// This exception is only thrown if the <see cref="EnvParserOptions.ThrowException" /> property is set to <c>true</c>.
        /// </exception>
        /// <returns>
        /// A dictionary if the environment cannot be modified, or <c>null</c> if the environment can be modified.
        /// </returns>
        IDictionary<string, string> Parse(string dataSource);

        /// <summary>
        /// Disables the trim at the start of the values.
        /// This method will tell the parser not to remove leading white spaces from the values.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimStartValues();

        /// <summary>
        /// Disables the trim at the end of the values.
        /// This method will tell the parser not to remove trailing white spaces from the values.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimEndValues();

        /// <summary>
        /// Disables the trim at the start and end of the values.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimValues();

        /// <summary>
        /// Disables the trim at the start of the keys.
        /// This method will tell the parser not to remove leading white spaces from the keys.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimStartKeys();

        /// <summary>
        /// Disables the trim at the end of the keys.
        /// This method will tell the parser not to remove trailing white spaces from the keys.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimEndKeys();

        /// <summary>
        /// Disables the trim at the start and end of the keys.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimKeys();

        /// <summary>
        /// Disables the trim at the start of the comments.
        /// This method will tell the parser not to remove leading white spaces from the comments.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableTrimStartComments();

        /// <summary>
        /// Allows overwriting the existing variables of an environment or dictionary (in case the environment cannot be modified).
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
        /// Sets the delimiter that separates an assignment of a value to a key.
        /// </summary>
        /// <param name="separator">The character that separates the key-value pair.</param>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser SetDelimiterKeyValuePair(char separator);

        /// <summary>
        /// Allows concatenating the values of the duplicate keys.
        /// </summary>
        /// <param name="option">The option indicates whether the concatenation is at the start or at the end of the value.</param>
        /// <exception cref="ArgumentException"><c>option</c> is not one of the <see cref="ConcatKeysOptions" /> values.</exception>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser AllowConcatDuplicateKeys(ConcatKeysOptions option = ConcatKeysOptions.End);

        /// <summary>
        /// Disables/ignores <see cref="ParserException" />. This method tells the parser not to throw an exception when it encounters one or more errors.
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser DisableParserException();

        /// <summary>
        /// Avoids modifying the environment of the current process. 
        /// This method tells the parser not to modify the environment, instead, it will return a dictionary with the keys read from a data source (such as an .env file).
        /// </summary>
        /// <returns>An instance implementing the fluent interface.</returns>
        IEnvParser AvoidModifyEnvironment();
    }
}
