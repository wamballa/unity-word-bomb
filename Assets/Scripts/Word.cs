using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Word : MonoBehaviour {

    //public WordManager wordManager;

    private string _word;
    private int typeIndex;
    public TMP_Text text;

    private float fallSpeed = 1f;
    bool isOffScreen;
    bool hasCrashed;

    public CollisionHandler collisionHandler;

    private void Start()
    {
        _word = WordGenerator.GetRandomWord();
        SetText();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckIsGrounded();
    }
    void HandleMovement()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    }
    void CheckIsGrounded()
    {
        hasCrashed = collisionHandler.HasCrashed();
    }
    public bool WordTyped()
    {
        bool wordTyped = (typeIndex >= _word.Length);
        if (wordTyped)
        {
            //display.RemoveWord();
        }
        return wordTyped;
    }

    public char GetNextLetter()
    {
        return _word[typeIndex];
    }

    public void TypeLetter()
    {
        typeIndex++;
        RemoveLetter();
    }

    public void RemoveLetter()
    {
        //text.text = text.text.Remove(0, 1);
        _word = _word.Remove(0, 1);
        text.color = Color.red;
        SetText();
    }

    public bool IsOffScreen()
    {
        return isOffScreen;
    }
    public bool HasCrashed()
    {
        return hasCrashed;
    }
    public string GetWord()
    {
        return _word;
    }
    private void SetText()
    {
        text.text = _word;
    }


}
