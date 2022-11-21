using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Represents the variables provider using a dictionary.
/// </summary>
internal class DictionaryProvider : IEnvironmentVariablesProvider
{
    private readonly IDictionary<string, string> _keyValuePairs = new Dictionary<string, string>();

    /// <inheritdoc />
    public string this[string variable] 
    {
        get
        {
            _keyValuePairs.TryGetValue(variable, out var value);
            return value;
        }
        set
        {
            _ = variable ?? throw new ArgumentNullException(nameof(variable));
            _keyValuePairs[variable] = value;
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _keyValuePairs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
