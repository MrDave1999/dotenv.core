using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core
{
    /// <summary>
    /// Represents the messages of an exception.
    /// </summary>
    public class ExceptionMessages
    {
        #pragma warning disable CS1591
        public const string VariableNotFoundMessage = "The environment variable was not found in the current process.";
        public const string InputIsEmptyOrWhitespaceMessage = "The data source (probably the env file) is empty or consists only in whitespace.";
        public const string KeyIsAnEmptyStringMessage = "The parser found an empty key (\"\").";
        public const string LineHasNoKeyValuePairMessage = "The parser found a line that has no key-value pair format.";
    }
}
