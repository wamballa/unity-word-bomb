using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private GameObject[] keyboard;
    private List<string> keys = new List<string>();
    const int NUM_KEYS = 12;
    const string BLANK_KEY_CHARACTER = ".";

    void Start()
    {
        ClearKeys();
        SetKeys("abcdefghijkl");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupKeyboard (string word)
    {
        print("SETUPKEYBOARD word = " + word);
        if (word.Length > 12) print("ERROR: word too long for keyboard");
        ClearKeys();
        SetKeys(word);
    }

    private void ClearKeys()
    {
        foreach(GameObject go in keyboard)
        {
            SetKeyString(go, BLANK_KEY_CHARACTER);
        }
    }
    private void SetKeys(string word)
    {
        int slotsFilled = 0;
        if (word.Length > NUM_KEYS) { print("ERROR: word longer than 12"); return; }

        // step 1 distribute word across keyboard

        for (int i = 0; i < word.Length; i++)
        {
            string letter = word[i].ToString();
            bool slotFound = false;
            //while (slotsFilled < 12)
            while (!slotFound)
            {
                int randomKey = Random.Range(0, keyboard.Length);
                string randomKeyLetter = GetKeyString(keyboard[randomKey]);

                if (randomKeyLetter == BLANK_KEY_CHARACTER)
                {
                    //print(">>> " + slotsFilled + " " + r + " " + s);
                    SetKeyString(keyboard[randomKey], letter);
                    slotFound = true;
                }

                slotsFilled++;
            }
        }

        // step 2 distribute random letter in blanks
        for (int i=0; i< NUM_KEYS; i++)
        {
            if (GetKeyString(keyboard[i]) == BLANK_KEY_CHARACTER)
            {
                SetKeyString(keyboard[i], "!");
            }
        }


        print("DONE KEYBOARD SET UP");


    }
    private void SetKeyString(GameObject key, string s)
    {
        key.GetComponentInChildren<TMP_Text>().text = s;
    }
    private string GetKeyString(GameObject key)
    {
        string c = key.GetComponentInChildren<TMP_Text>().text.ToString();
        return c;
    }

    public void TypedLetter( string s)
    {
        print("You typed " + s);
    }


}
