﻿namespace DotEnv.Core.Tests;

internal class CustomEnvironmentVariablesProvider : IEnvironmentVariablesProvider
{
    private readonly Dictionary<string, string> _keyValuePairs = new();

    public string this[string variable]
    {
        get => _keyValuePairs.ContainsKey(variable) ? _keyValuePairs[variable] : null;
        set => _keyValuePairs[variable] = value;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _keyValuePairs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
