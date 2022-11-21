using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <inheritdoc cref="IEnvReader" />
public partial class EnvReader : IEnvReader
{
    private static readonly EnvReader s_instance = new();
    private readonly IEnvironmentVariablesProvider _envVars = new DefaultEnvironmentProvider();

    /// <summary>
    /// Gets an instance of type <see cref="EnvReader" />.
    /// </summary>
    /// <remarks>This method is thread-safe.</remarks>
    public static EnvReader Instance => s_instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvReader" /> class.
    /// </summary>
    public EnvReader()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvReader" /> class with environment variables provider.
    /// </summary>
    /// <param name="provider">The environment variables provider.</param>
    public EnvReader(IEnvironmentVariablesProvider provider)
    {
        _envVars = provider;
    }

    /// <inheritdoc />
    public virtual bool HasValue(string variable)
    {
        _ = variable ?? throw new ArgumentNullException(nameof(variable));
        var retrievedValue = _envVars[variable];
        return retrievedValue is not null;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the variables.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the variables.</returns>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _envVars.GetEnumerator();

    /// <inheritdoc cref="GetEnumerator" />
    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
