using System.Collections.Generic;
using UnityEngine;

public static class JsonUtilityWrapper
{
    [System.Serializable]
    private class DictionaryWrapper
    {
        public List<Entry> entries = new();
    }

    [System.Serializable]
    private class Entry
    {
        public string key;
        public List<string> value;
    }

    public static Dictionary<string, List<string>> FromJson<T>(string json)
    {
        var wrapper = JsonUtility.FromJson<DictionaryWrapper>(json.Replace("\"", "\\\""));
        var dict = new Dictionary<string, List<string>>();
        foreach (var entry in wrapper.entries)
        {
            dict[entry.key] = entry.value;
        }
        return dict;
    }
}
