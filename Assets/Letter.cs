using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    //public WordManager wordManager;

    private string letter;
    private int typeIndex;
    public TMP_Text text;


    float fallSpeed = 2f;
    bool isOffScreen;

    // Start is called before the first frame update
    void Start()
    {
        letter = LetterGenerator.GetRandomLetter();
        SetLetter();
    }

    private void SetLetter()
    {
        text.text = letter;
    }

    public char GetLetter()
    {
        return letter[0];
    }

    private void FixedUpdate()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);

        if (transform.position.y < -4f)
        {
            isOffScreen = true;
        }
    }

    public bool IsOffScreen()
    {
        return isOffScreen;
    }

}
