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
        public const string KeyNotFoundMessage = "The key was not found in the dictionary.";
        public const string DataSourceIsEmptyOrWhitespaceMessage = "The data source (probably the env file) is empty or consists only in whitespace.";
        public const string KeyIsAnEmptyStringMessage = "The key name cannot be an empty string or consists only of white-space characters.";
        public const string VariableIsAnEmptyStringMessage = "The variable embedded in the value cannot be an empty string or consists only of white-space characters.";
        public const string LineHasNoKeyValuePairMessage = "The parser found a line that has no key-value pair format.";
        public const string FileNotFoundMessage = "The .env file could not be found.";
        public const string FileNotPresentLoadEnvMessage = "Any of these .env files must be present in the root directory of your project";
        public const string OptionInvalidMessage = "The option is invalid.";
        public const string ArgumentIsNullOrWhiteSpaceMessage = "The argument cannot be an empty string or consists only of white-space characters.";
    }
}
