using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordManager : MonoBehaviour
{
    public TMP_Text numberOfWords;
    public TMP_Text scoreText;
    public TMP_Text livesText;

    public List<GameObject> words;
    public List<GameObject> letters;

    public WordSpawner wordSpawner;

    private bool hasActiveWord;
    private GameObject activeWord;

    private bool canTypeWord = true;

    private int lives;
    private int score = 0;

    LevelManager levelManager;

    bool allWordsDropped = false;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        lives = levelManager.GetLives();
        score = levelManager.GetScore();

        ShowLives();
    }

    private void Update()
    {
        // Updates HUD stuff
        // Removes words off screen
        ShowNumberOfWords();
        //ShowScore();
        RemoveWhenOffScreen();
        ShowWords();
    }

    public void AddWord(GameObject _word)
    {
        //GameObject word = wordSpawner.SpawnWord();
        words.Add(_word);
    }

    public void AddLetter()
    {
        GameObject letter = wordSpawner.SpawnLetter();
        letters.Add(letter);
        canTypeWord = false;
    }

    public void TypeLetter(char letter)
    {

        if (hasActiveWord)
        {
            if (activeWord.GetComponent<Word>().GetNextLetter() == letter)
            {
                activeWord.GetComponent<Word>().RemoveLetter();
            }
        }
        else
        {
            foreach (GameObject word in words)
            {
                if (word.GetComponent<Word>().GetNextLetter() == letter)
                {
                    activeWord = word;
                    hasActiveWord = true;
                    word.GetComponent<Word>().RemoveLetter();
                    break;
                }
            }
        }
        // WORD
        // Destroy word just typed and check if last word/letter
        if (hasActiveWord && activeWord.GetComponent<Word>().WordTyped())
        {
            hasActiveWord = false;
            Destroy(activeWord);
            //words.Remove(activeWord);
            levelManager.SetScore(+1);
            //CheckIfAllDropped();
        }

        // SPECIAL CHAR BOMBS

        // Letter bombs & check if last letter
        for (int i = 0; i < letters.Count; i++)
        { 
            Letter l = letters[i].GetComponent<Letter>();

        
            if (l.GetLetter() == letter)
            {
                Destroy(letters[i]);
                letters.RemoveAt(i);
                levelManager.SetScore(+1);
                CheckIfAllDropped();
            }
        }
    }

    public void RemoveWhenOffScreen()
    {
        // Remove word
        for (int i = 0; i < words.Count; i++)
        {
            Word w = words[i].GetComponent<Word>();

            //Word w = words[i];

            if (w.IsOffScreen())
            {
                //Destroy(words[i]);
                //words.RemoveAt(i);
                //hasActiveWord = false;
                //CheckIfAllDropped();
                //DecreaseLives();
                //ShowLives();
            }
            if (w.HasCrashed())
            {
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
                CheckIfAllDropped();
                DecreaseLives();
                ShowLives();
            }
        }
        // Remove letter
        for (int i = 0; i < letters.Count; i++)
        {
            Letter letter = letters[i].GetComponent<Letter>();
            if (letter.IsOffScreen())
            {
                Destroy(letters[i]);
                letters.RemoveAt(i);
                //hasActiveWord = false;
                DecreaseLives();
                CheckIfAllDropped();
            }
        }
        // check if all words done



    }


    private void CheckIfAllDropped()
    {
        //Debug.Log("CHECK IF ALL WORDS DROPPED");

        if (allWordsDropped && letters.Count == 0 && words.Count == 0)
        {
            Debug.Log("LEVEL COMPLETE");
        }
    }

    public void SetAllDropped(bool b)
    {
        //Debug.Log("SET ALL DROPPED "+b);
        allWordsDropped = b;
    }


    void DecreaseLives()
    {
        levelManager.SetLives(-1);
    }

    private void ShowLives()
    {
        livesText.text = levelManager.GetLives().ToString();
    }

    private void ShowScore()
    {
        scoreText.text = levelManager.GetScore().ToString();

    }

    private void ShowNumberOfWords()
    {
        numberOfWords.text = words.Count.ToString();
    }
    private void ShowWords()
    {

        //foreach(GameObject word in words)
        //{
        //    //scoreText.text = word.
        //}

    }

}
