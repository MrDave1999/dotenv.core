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
        public const string VariableNotSetMessage = "'{0}' is not set.";
        public const string DataSourceIsEmptyOrWhitespaceMessage = "Data source is empty or consists only in whitespace.";
        public const string VariableIsAnEmptyStringMessage = "'{0}' is an invalid interpolation expression.";
        public const string LineHasNoKeyValuePairMessage = "'{0}' does not have the format of key-value pair.";
        public const string FileNotFoundMessage = "{0}: error: No such file or directory.";
        public const string OptionInvalidMessage = "The option is invalid.";
        public const string ArgumentIsNullOrWhiteSpaceMessage = "The argument cannot be an empty string or consists only of white-space characters.";
        public const string RequiredKeysNotSpecifiedMessage = "The keys required must be specified with the EnvValidator.SetRequiredKeys method before invoking the EnvValidator.Validate method.";
        public const string RequiredKeysNotPresentMessage = "'{0}' is a key required by the application.";
        public const string LengthOfParamsListIsZeroMessage = "The length of the params list is zero.";
        public const string EncodingNotFoundMessage = "'{0}' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";
    }
}
