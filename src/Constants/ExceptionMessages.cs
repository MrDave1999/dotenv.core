using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Represents the messages of an exception.
/// </summary>
internal class ExceptionMessages
{
    public const string VariableNotSetMessage                   = "'{0}' is not set.";
    public const string DataSourceIsEmptyOrWhitespaceMessage    = "Data source is empty or consists only in whitespace.";
    public const string VariableIsAnEmptyStringMessage          = "'{0}' is an invalid interpolation expression.";
    public const string LineHasNoKeyValuePairMessage            = "'{0}' does not have the format of key-value pair.";
    public const string FileNotFoundMessage                     = "{0}: error: No such file or directory.";
    public const string OptionInvalidMessage                    = "The option is invalid.";
    public const string ArgumentIsNullOrWhiteSpaceMessage       = "The argument cannot be an empty string or consists only of white-space characters.";
    public const string RequiredKeysNotSpecifiedMessage         = "The keys required must be specified with the EnvValidator.SetRequiredKeys method before invoking the EnvValidator.Validate method.";
    public const string RequiredKeysNotPresentMessage           = "'{0}' is a key required by the application.";
    public const string LengthOfParamsListIsZeroMessage         = "The length of the params list is zero.";
    public const string EncodingNotFoundMessage                 = "'{0}' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";
    public const string PropertyDoesNotMatchConfigKeyMessage    = "The '{0}.{1}' property does not match any configuration key.";
    public const string KeyAssignedToPropertyIsNotSetMessage    = "Could not set the value in the '{0}.{1}' property because the '{2}' key is not set.";
    public const string FailedConvertConfigurationValueMessage  = "Failed to convert configuration value of '{0}' to type '{1}'. '{2}' is not a valid value for {3}.";
    public const string LineHasNoEndDoubleQuoteMessage          = "The line must end with a double-quoted at the end.";
    public const string LineHasNoEndSingleQuoteMessage          = "The line must end with a single-quoted at the end.";
}
