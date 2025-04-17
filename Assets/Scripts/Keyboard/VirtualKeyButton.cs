using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class VirtualKeyButton : MonoBehaviour, IPointerClickHandler
{
    private string keyboardKey;
    private WordGameplayManager wordManager;
    public Color32 color;

    // Components
    Image imageComponent;
    TMP_Text textComponent;

    void Awake()
    {

        // Get the WordManager component from the scene
        wordManager = GameObject.Find("WordManager").GetComponent<WordGameplayManager>();
        if (wordManager == null) Debug.LogError("ERROR: no WordManager found");

        imageComponent = transform.GetComponent<Image>();
        textComponent = transform.GetComponentInChildren<TMP_Text>();

        keyboardKey = textComponent.text;

        SetColours();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string key = textComponent.text;
        InputRouter.RouteKey(key);
    }

    // Detect if a click occurs
    //public void OnPointerClick(PointerEventData pointerEventData)
    //{

    //    // Check if the user left-clicked on the Button
    //    if (pointerEventData.button == PointerEventData.InputButton.Left)
    //    {
    //        int number;
    //        if (int.TryParse(keyboardKey, out number))
    //        {
    //            Debug.Log("Number Pressed = " + number);
    //            wordManager.TypeNumber(number);
    //        }
    //        else
    //        {
    //            Debug.Log("Key Pressed = " + keyboardKey);
    //            wordManager.TypeLetter(keyboardKey[0]);
    //        }
    //    }
    //}

    public void SetColours()
    {
        int number;
        if (int.TryParse(keyboardKey, out number))
        {
            imageComponent.color = color;
            textComponent.color = color;
        }
        else
        {
            imageComponent.color = Color.white;
            textComponent.color = Color.white;
        }
    }
}
