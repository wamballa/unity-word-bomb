// tracks progress & typing match

using UnityEngine;

public class WordTyping : MonoBehaviour
{
    private string currentWord = "";
    private int typedIndex = 0;

    public void SetTargetWord(string word)
    {
        currentWord = word;
        typedIndex = 0;
    }

    public string GetCurrentWord() => currentWord;

    public bool TryTypeLetter(char c)
    {
        if (typedIndex >= currentWord.Length) return false;

        if (char.ToLower(currentWord[typedIndex]) == char.ToLower(c))
        {
            typedIndex++;
            return true;
        }

        return false;
    }

    public char GetNextLetter()
    {
        if (typedIndex < currentWord.Length)
        {
            return currentWord[typedIndex];
        }
        return '\0'; // null character fallback
    }

    public void OnLetterTyped(char c)
    {
        TryTypeLetter(c); 
    }


    public bool IsComplete() => typedIndex >= currentWord.Length;
}