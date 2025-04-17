//using System;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class TouchInputHandler : MonoBehaviour
//{

//    public static event Action<string> OnKeyPressed;
//    private Button keyboardButton;
//    private string keyboardKey;

//    private void Awake()
//    {
//        keyboardKey = transform.GetComponentInChildren<TMP_Text>().text;

//        keyboardButton = GetComponent<Button>();
//        if (keyboardButton == null) Debug.LogError("[TouchInputHandler] No keyboard button found for "+transform.name);
//        keyboardButton.onClick.AddListener(() => OnKeyPressed?.Invoke(keyboardKey));

//        Debug.Log("Key setup " + transform.name + " ... Key text = " + keyboardKey);
//    }

//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
