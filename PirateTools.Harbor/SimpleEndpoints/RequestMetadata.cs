using System.Collections.Generic;

namespace PirateTools.Harbor.SimpleEndpoints;

public class RequestMetadata {
    private readonly Dictionary<string, object> _metadata = [];

    public void Set(string key, object value) => _metadata[key] = value;

    public string GetString(string key) => (string)_metadata[key];

    public int GetInt(string key) => (int)_metadata[key];

    public bool TryGetString(string key, out string str) {
        if (!_metadata.TryGetValue(key, out var value)) {
            str = "";
            return false;
        }

        if (value is not string s) {
            str = "";
            return false;
        }

        str = s;
        return true;
    }
}