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
    private readonly Dictionary<string, string> _keyValuePairs;

    public DictionaryProvider()
        => _keyValuePairs = [];

    public DictionaryProvider(StringComparer comparer)
        => _keyValuePairs = new(comparer);

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
            ThrowHelper.ThrowIfNull(variable, nameof(variable));
            _keyValuePairs[variable] = value;
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _keyValuePairs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
