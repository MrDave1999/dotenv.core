using System;
using System.Collections.Generic;

namespace DotEnv.Core;

/// <summary>
/// Represents the options for configuring various behaviors of the binder.
/// </summary>
internal class EnvBinderOptions
{
    /// <summary>
    /// A value indicating whether the binder should throw an exception when it encounters one or more errors. Its default value is <c>true</c>.
    /// </summary>
    public bool ThrowException { get; set; } = true;

    /// <summary>
    /// When <c>false</c> (the default), the binder will only to set public properties. 
    /// If <c>true</c>, the binder will to set all non-public properties.
    /// </summary>
    public bool BindNonPublicProperties { get; set; }

    /// <summary>
    /// Gets or sets the environment variables provider.
    /// </summary>
    public IEnvironmentVariablesProvider EnvVars { get; set; } = new DefaultEnvironmentProvider();
}