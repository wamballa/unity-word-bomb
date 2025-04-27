using System.Collections.Generic;
using System.Linq;

public static class WordSelector
{
    public static string GetWordMatchingRadialSet(HashSet<char> radialLetters, List<string> wordList)
    {
        var matches = wordList
            .Where(word => word.All(c => radialLetters.Contains(char.ToLower(c))))
            .ToList();

        if (matches.Count == 0) return string.Empty;

        return matches[UnityEngine.Random.Range(0, matches.Count)];
    }
}
