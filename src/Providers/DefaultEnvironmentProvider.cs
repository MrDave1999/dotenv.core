using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
///  Represents the variables provider using the environment of the current process.
/// </summary>
internal class DefaultEnvironmentProvider : IEnvironmentVariablesProvider
{
    /// <inheritdoc />
    public string this[string variable] 
    {
        get => Environment.GetEnvironmentVariable(variable);
        set
        {
            _ = variable ?? throw new ArgumentNullException(nameof(variable));
            Environment.SetEnvironmentVariable(variable, value);
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            yield return new KeyValuePair<string, string>((string)de.Key, (string)de.Value);
    }

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
