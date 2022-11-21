using System;
using System.Collections.Generic;

namespace DotEnv.Core;

/// <summary>
/// Represents the key of a .env file that is assigned to a property.
/// </summary>
public class EnvKeyAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the key the property is mapped to.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EnvKeyAttribute" /> class.
    /// </summary>
    public EnvKeyAttribute()
    {

    }
    /// <summary>
    /// Initializes a new instance of the <see cref="EnvKeyAttribute" /> class with the name of the key.
    /// </summary>
    /// <param name="name">The name of the key the property is mapped to.</param>
    public EnvKeyAttribute(string name)
    {
        Name = name;
    }
}