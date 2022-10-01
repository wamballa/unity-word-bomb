using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput3D : MonoBehaviour {

#if UNITY_EDITOR

    private WordManager3D wordManager;
    //float typeDelay = 0.01f;
    //bool canType = true;

    private void Start()
    {
        wordManager = GameObject.Find("WordManager").GetComponent<WordManager3D>();
        if (wordManager == null) print("ERROR: ");
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
