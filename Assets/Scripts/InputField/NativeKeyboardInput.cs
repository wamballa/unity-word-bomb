using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NativeKeyboardInput : MonoBehaviour
{

//#if UNITY_IOS

    TMP_InputField MyInputfield;
    //public TMP_Text text;
    //TouchScreenKeyboard keyboard;
    WordManager wordManager;

    /// <summary>
    /// First things first
    /// </summary>
    private void Awake()
    {
        MyInputfield = GameObject.Find("NativeInputField").GetComponent<TMP_InputField>();
        if (MyInputfield == null) Debug.LogError("ERROR: no input field found");
    }
    void Start()
    {
        MyInputfield.onSubmit.AddListener(DoStuffWhenSubmitted);
        MyInputfield.onValueChanged.AddListener(DoStuffWhenValueChanged);
        MyInputfield.Select();

        wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        if (wordManager == null) print("ERROR: missing word manager");
    }

    void DoStuffWhenSubmitted(string input)
    {
        // do stuff here
        print("Input submitted = " + input);
    }
    void DoStuffWhenValueChanged(string input)
    {
        if (input != "")
        {
            //print("Input change = " + input);
            input = input.ToLower();
            char letter = input[0];
            //print("Input letter = " + letter);
            wordManager.TypeLetter(letter);
            MyInputfield.text = "";
        }

    }

    private void Update()
    {
        if (!MyInputfield.isFocused)
        {
            //print("LOST FOCUS");
            MyInputfield.Select();
        }
    }
//#endif
}
