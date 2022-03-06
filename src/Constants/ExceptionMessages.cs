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
        public const string VariableNotSetMessage = "The variable is not set.";
        public const string InterpolatedVariableNotFoundMessage = "The interpolated variable is not set.";
        public const string DataSourceIsEmptyOrWhitespaceMessage = "The data source (probably the env file) is empty or consists only in whitespace.";
        public const string KeyIsAnEmptyStringMessage = "The key name cannot be an empty string or consists only of white-space characters.";
        public const string VariableIsAnEmptyStringMessage = "The interpolated variable in the value cannot be an empty string or consists only of white-space characters.";
        public const string LineHasNoKeyValuePairMessage = "The parser found a line that has no key-value pair format.";
        public const string FileNotFoundMessage = "The .env file could not be found.";
        public const string FileNotPresentLoadEnvMessage = "Any of these .env files must be present in the root directory of your project";
        public const string OptionInvalidMessage = "The option is invalid.";
        public const string ArgumentIsNullOrWhiteSpaceMessage = "The argument cannot be an empty string or consists only of white-space characters.";
        public const string RequiredKeysNotAddedMessage = "The keys required must be added with the EnvValidator.AddRequiredKeys method before invoking the EnvValidator.Validate method.";
        public const string RequiredKeysNotPresentMessage = "is a key required by the application.";
        public const string LengthOfParamsListIsZeroMessage = "The length of the params list is zero.";
        public const string EncodingNotFoundMessage = "is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";
    }
}
