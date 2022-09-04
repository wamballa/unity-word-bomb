using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput : MonoBehaviour {

#if UNITY_EDITOR

    public WordManager wordManager;
    //float typeDelay = 0.01f;
    //bool canType = true;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        //if (!canType) return;
        foreach (char letter in Input.inputString)
        {
            wordManager.TypeLetter(letter);
        }

    }

#endif

}
