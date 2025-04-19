using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WordLoader
{
    private static List<string> filteredWords;

    // Load and filter the word bank at startup
    public static void LoadWordBank()
    {
        if (filteredWords != null) return; // already loaded

        TextAsset wordData = Resources.Load<TextAsset>("WordBank");
        if (wordData == null)
        {
            Debug.LogError("WordBank.txt not found in Resources.");
            filteredWords = new List<string>();
            return;
        }

        filteredWords = wordData.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim().ToLower())
            .Where(w => w.Length >= 2 && w.Length <= 6 && w.All(char.IsLetter))
            .ToList();

        Debug.Log($"WordLoader: Loaded {filteredWords.Count} valid words.");
    }

    // Returns all words that can be built from the provided 6 letters (can reuse up to 2 chars)
    public static List<string> GetValidWordsFromLetters(string letters)
    {
        if (filteredWords == null) LoadWordBank();

        letters = letters.ToLower();
        Dictionary<char, int> letterCounts = letters
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        return filteredWords.Where(word => CanFormWord(word, letterCounts)).ToList();
    }

    private static bool CanFormWord(string word, Dictionary<char, int> availableLetters)
    {
        var wordLetterCounts = word.GroupBy(c => c)
                                   .ToDictionary(g => g.Key, g => g.Count());

        foreach (var pair in wordLetterCounts)
        {
            if (!availableLetters.ContainsKey(pair.Key) || availableLetters[pair.Key] < pair.Value)
                return false;
        }

        return true;
    }
}
