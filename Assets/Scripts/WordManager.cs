using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordManager : MonoBehaviour
{
    GameManager gameManager;

    // WORD stuff
    public TMP_Text numberOfWords;
    //public TMP_Text scoreText;
    //public TMP_Text livesText;

    public List<GameObject> words;
    public List<GameObject> letters;

    //public WordSpawner wordSpawner;

    private bool hasActiveWord;
    private GameObject activeWord;

    private bool canTypeWord = true;

    //private int lives;
    //private int score = 0;



    bool allWordsDropped = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        RemoveWhenCrashed();
    }
    /// <summary>
    /// Add a word to a list
    /// </summary>
    /// <param name="_word"></param>
    public void AddWord(GameObject _word)
    {
        //GameObject word = wordSpawner.SpawnWord();
        words.Add(_word);
    }

    public void AddLetter(GameObject _letter)
    {
        //GameObject letter = wordSpawner.SpawnLetter();
        letters.Add(_letter);
        //canTypeWord = false;
    }

    public void TypeLetter(char typedLetter)
    {
        // WHEN KEYBOARD CHARACTER TYPED, CHECK IF IT'S THE FIRST LETTER OF A WORD
        // AND SET IT THE ACTIVE WORD
        if (hasActiveWord)
        {
            if (activeWord.GetComponent<Word>().GetNextLetter() == typedLetter)
            {
                activeWord.GetComponent<Word>().RemoveLetter(typedLetter);
            }
        }
        // WHEN KEYBOARD CHARACTER TYPED, SEE IF IT MATCHES FIRST LETTER
        // OF ANY WORD - THEN SET THAT WORD AS ACTIVE WORD
        else
        {
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].GetComponent<Word>().GetNextLetter() == typedLetter)
                {
                    activeWord = words[i];
                    hasActiveWord = true;
                    words[i].GetComponent<Word>().RemoveLetter(typedLetter);
                    break;
                }
            }
        }
        // WORD
        // Destroy word just typed and check if last word/letter
        if (hasActiveWord && activeWord.GetComponent<Word>().HasWordBeenTyped())
        {
            hasActiveWord = false;
        }

        // SPECIAL CHAR BOMBS
        //print("Num letters = " + letters.Count);
        // Letter bombs & check if last letter
        for (int i = 0; i < letters.Count; i++)
        {
            Letter l = letters[i].GetComponent<Letter>();
            //print("Letters " + i + " " + letters[i]);
            if (l.GetLetter() == typedLetter)
            {
                l.HandleExplosion();
                //Destroy(letters[i]);


                letters.RemoveAt(i);


                gameManager.SetScore(+10);
                //CheckIfAllDropped();
            }
        }
    }

    //void RemoveWordFromList()
    //{
    //    for (int i = 0; i < words.Count; i++)
    //    {
    //        Word w = words[i].GetComponent<Word>();

    //        if (activeWord = words[i])
    //        {
    //            print("Remove Word " + i);
    //        }
    //    }
    //}

    public void RemoveWhenCrashed()
    {
        // Remove word
        for (int i = 0; i < words.Count; i++)
        {
            Word w = words[i].GetComponent<Word>();

            if (w.HasCrashed())
            {
                print("Word HasCrashed");
                //w.HandleExplosion();
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
                //CheckIfAllDropped();
                //DecreaseLives();

            }
            if (w.HasWordBeenTyped())
            {
                print("Word HasWordBeenTyped");
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
                //CheckIfAllDropped();
                IncreaseScore();
            }
            if (w.GetWordHasBeenExploded())
            {
                print("Word GetWordHasBeenExploded");
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
                //CheckIfAllDropped();
                IncreaseScore();
            }
        }
        // Remove letter
        for (int i = 0; i < letters.Count; i++)
        {
            Letter letter = letters[i].GetComponent<Letter>();
            if (letter.HasCrashed())
            {
                Destroy(letters[i]);
                letters.RemoveAt(i);
                //hasActiveWord = false;
                //DecreaseLives();
                //CheckIfAllDropped();
            }
        }
        // check if all words done



    }


    //private void CheckIfAllDropped()
    //{
    //    //Debug.Log("CHECK IF ALL WORDS DROPPED");

    //    if (allWordsDropped && letters.Count == 0 && words.Count == 0)
    //    {
    //        Debug.Log("LEVEL COMPLETE");
    //    }
    //}

    public void SetAllDropped(bool b)
    {
        //Debug.Log("SET ALL DROPPED "+b);
        allWordsDropped = b;
    }


    //void DecreaseLives()
    //{
    //    gameManager.SetLives(-1);
    //}
    void IncreaseScore()
    {
        gameManager.SetScore(1);
    }

    //private void ShowLives()
    //{
    //    livesText.text = gameManager.GetLives().ToString();
    //}

    //private void ShowScore()
    //{
    //    scoreText.text = gameManager.GetScore().ToString();

    //}

    //private void ShowNumberOfWords()
    //{
    //    numberOfWords.text = words.Count.ToString();
    //}
    //private void ShowWords()
    //{

    //    //foreach(GameObject word in words)
    //    //{
    //    //    //scoreText.text = word.
    //    //}

    //}

}
