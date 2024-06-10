﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotEnv.Core {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DotEnv.Core.Resources.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The argument cannot be an empty string or consists only of white-space characters..
        /// </summary>
        internal static string ArgumentIsNullOrWhiteSpaceMessage {
            get {
                return ResourceManager.GetString("ArgumentIsNullOrWhiteSpaceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred when binding configuration keys to a strongly typed object..
        /// </summary>
        internal static string BinderDefaultMessage {
            get {
                return ResourceManager.GetString("BinderDefaultMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data source is empty or consists only in whitespace..
        /// </summary>
        internal static string DataSourceIsEmptyOrWhitespaceMessage {
            get {
                return ResourceManager.GetString("DataSourceIsEmptyOrWhitespaceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method..
        /// </summary>
        internal static string EncodingNotFoundMessage {
            get {
                return ResourceManager.GetString("EncodingNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to convert configuration value of &apos;{0}&apos; to type &apos;{1}&apos;. &apos;{2}&apos; is not a valid value for {3}..
        /// </summary>
        internal static string FailedConvertConfigurationValueMessage {
            get {
                return ResourceManager.GetString("FailedConvertConfigurationValueMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: error: No such file or directory..
        /// </summary>
        internal static string FileNotFoundMessage {
            get {
                return ResourceManager.GetString("FileNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not set the value in the &apos;{0}.{1}&apos; property because the &apos;{2}&apos; key is not set..
        /// </summary>
        internal static string KeyAssignedToPropertyIsNotSetMessage {
            get {
                return ResourceManager.GetString("KeyAssignedToPropertyIsNotSetMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The length of the params list is zero..
        /// </summary>
        internal static string LengthOfParamsListIsZeroMessage {
            get {
                return ResourceManager.GetString("LengthOfParamsListIsZeroMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The line must end with a double-quoted at the end..
        /// </summary>
        internal static string LineHasNoEndDoubleQuoteMessage {
            get {
                return ResourceManager.GetString("LineHasNoEndDoubleQuoteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The line must end with a single-quoted at the end..
        /// </summary>
        internal static string LineHasNoEndSingleQuoteMessage {
            get {
                return ResourceManager.GetString("LineHasNoEndSingleQuoteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; does not have the format of key-value pair..
        /// </summary>
        internal static string LineHasNoKeyValuePairMessage {
            get {
                return ResourceManager.GetString("LineHasNoKeyValuePairMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The option is invalid..
        /// </summary>
        internal static string OptionInvalidMessage {
            get {
                return ResourceManager.GetString("OptionInvalidMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while parsing a data source with key-value pairs..
        /// </summary>
        internal static string ParserDefaultMessage {
            get {
                return ResourceManager.GetString("ParserDefaultMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &apos;{0}.{1}&apos; property does not match any configuration key..
        /// </summary>
        internal static string PropertyDoesNotMatchConfigKeyMessage {
            get {
                return ResourceManager.GetString("PropertyDoesNotMatchConfigKeyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The required keys are not present in the application..
        /// </summary>
        internal static string RequiredKeysNotPresentDefaultMessage {
            get {
                return ResourceManager.GetString("RequiredKeysNotPresentDefaultMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is a key required by the application..
        /// </summary>
        internal static string RequiredKeysNotPresentMessage {
            get {
                return ResourceManager.GetString("RequiredKeysNotPresentMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The keys required must be specified with the EnvValidator.SetRequiredKeys method before invoking the EnvValidator.Validate method..
        /// </summary>
        internal static string RequiredKeysNotSpecifiedMessage {
            get {
                return ResourceManager.GetString("RequiredKeysNotSpecifiedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is an invalid interpolation expression..
        /// </summary>
        internal static string VariableIsAnEmptyStringMessage {
            get {
                return ResourceManager.GetString("VariableIsAnEmptyStringMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Environment variable is not set..
        /// </summary>
        internal static string VariableNotSetDefaultMessage {
            get {
                return ResourceManager.GetString("VariableNotSetDefaultMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not set..
        /// </summary>
        internal static string VariableNotSetMessage {
            get {
                return ResourceManager.GetString("VariableNotSetMessage", resourceCulture);
            }
        }
    }
}
