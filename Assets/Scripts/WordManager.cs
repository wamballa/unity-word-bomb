using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordManager : MonoBehaviour
{
    GameManager gameManager;

    // WORD stuff
    //public TMP_Text numberOfWords;
    //public TMP_Text scoreText;
    //public TMP_Text livesText;

    public List<GameObject> words;
    public List<GameObject> letters;

    //public WordSpawner wordSpawner;

    private bool hasActiveWord;
    private GameObject activeWord;

    //private bool canTypeWord = true;
    //bool allWordsDropped = false;

    // Keyboard stuff
    //[SerializeField] Keyboard keyboard;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        ///
        ///
        ///
        ///
        RemoveWhenCrashed();
        ///
        ///
        ///
    }


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
        typedLetter = char.ToLower(typedLetter);
        //print("WORD TYPED " + typedLetter);
        //print("has active = " + hasActiveWord);

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
                //print("Next letter: word / letter " + words[i].name + " " + words[i].GetComponent<Word>().GetNextLetter());
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

                gameManager.SetScore(+1);
                //CheckIfAllDropped();
            }
        }
    }


    public void RemoveWhenCrashed()
    {
        // Remove word
        for (int i = 0; i < words.Count; i++)
        {
            Word w = words[i].GetComponent<Word>();
            //print("Num / Word = " + " / " + w.name);
            if (w.HasCrashed())
            {
                //print("Word HasCrashed");
                //w.HandleExplosion();
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
                //CheckIfAllDropped();
                //DecreaseLives();

            }
            if (w.HasWordBeenTyped())
            {
                //print("Word HasWordBeenTyped");
                Destroy(words[i],1);
                //StartCoroutine(DestroyWordCoRoutine(i));
                words.RemoveAt(i);
                hasActiveWord = false;
                //CheckIfAllDropped();
                IncreaseScore();
            }
            if (w.GetWordHasBeenExploded())
            {
                //print("Word GetWordHasBeenExploded");
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
                //CheckIfAllDropped();
                IncreaseScore();
            }
            if (w.IsOffScreen())
            {
                Destroy(words[i]);
                words.RemoveAt(i);
                hasActiveWord = false;
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
            if (letter.GetIsOffScreen())
            {
                Destroy(letters[i]);
                letters.RemoveAt(i);
            }
        }
        // check if all words done
    }

    //IEnumerator DestroyWordCoRoutine(int index)
    //{
    //    yield return new WaitForSeconds(1f);

    //}

    public void SetAllDropped(bool b)
    {
        //Debug.Log("SET ALL DROPPED "+b);
        //allWordsDropped = b;
    }

    void IncreaseScore()
    {
        gameManager.SetScore(1);
    }
}
