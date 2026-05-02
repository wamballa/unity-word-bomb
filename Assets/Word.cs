using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Word : MonoBehaviour {

    //public WordManager wordManager;

    private string word;
    private int typeIndex;
    public TMP_Text text;

    private float fallSpeed = 1f;
    bool isOffScreen;

    private void Start()
    {
        word = WordGenerator.GetRandomWord();
        SetWord();
    }

    private void SetWord()
    {
        text.text = word;
    }

    private void FixedUpdate()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);

        if (transform.position.y < -4f)
        {
            isOffScreen = true;
        }
    }

    public bool WordTyped()
    {
        bool wordTyped = (typeIndex >= word.Length);
        if (wordTyped)
        {
            //display.RemoveWord();
        }
        return wordTyped;
    }

    public char GetNextLetter()
    {
        return word[typeIndex];
    }

    public void TypeLetter()
    {
        typeIndex++;
        RemoveLetter();
    }

    public void RemoveLetter()
    {
        text.text = text.text.Remove(0, 1);
        text.color = Color.red;
    }

    public bool IsOffScreen()
    {
        return isOffScreen;
    }

    public string GetWord()
    {
        return word;
    }



}
