using System.Collections.Generic;
using UnityEngine;

public class WordBankTester : MonoBehaviour
{
    [Tooltip("Set the current 6 letters used in the radial menu")]
    public string radialLetters = "atrine";

    void Start()
    {
        WordLoader.LoadWordBank();

        List<string> validWords = WordLoader.GetValidWordsFromLetters(radialLetters);

        if (validWords.Count == 0)
        {
            Debug.LogWarning("No valid words found with these letters: " + radialLetters);
        }
        else
        {
            Debug.Log($"Found {validWords.Count} words using '{radialLetters}':");
            for (int i = 0; i < Mathf.Min(10, validWords.Count); i++)
            {
                Debug.Log(validWords[i]);
            }
        }
    }
}
