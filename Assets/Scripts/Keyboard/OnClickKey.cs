using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnClickKey : MonoBehaviour
{
    WordManager wordManager;

    // Start is called before the first frame update
    void Start()
    {
        Button button = transform.GetComponent<Button>();
        button.onClick.AddListener(RunThisTask);
        wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        if (wordManager == null) print("ERROR: no word manager found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RunThisTask()
    {
        //print("Button pressed was "+ GetLetter());
        //wordManager.TypeLetter(GetLetter()[0], this);
        wordManager.TypeLetter(GetLetter()[0]);
    }

    string GetLetter()
    {
        string keyedLetter = transform.GetComponentInChildren<TMP_Text>().text;
        return keyedLetter;
    }

    public void DisableKey()
    {
        print("Key Disabled");
        gameObject.SetActive(false);
    }

}
