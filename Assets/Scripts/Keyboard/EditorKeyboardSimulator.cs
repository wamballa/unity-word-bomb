using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class EditorKeyboardSimulator : MonoBehaviour {



    public WordGameplayManager wordManager;

    private void Start()
    {
    }


        void Update()
        {
            foreach (char c in Input.inputString)
            {
                //Debug.Log("[EditorKeyboardSimulator] "+c);
                InputRouter.RouteKey(c.ToString());
            }
        }

        //    foreach (char inputChar in Input.inputString)
        //{
        //    if (char.IsDigit(inputChar))
        //    {
        //        int typedNumber = int.Parse(inputChar.ToString());
        //        wordManager.TypeNumber(typedNumber);
        //    }
        //    else if (char.IsLetter(inputChar))
        //    {
        //        wordManager.TypeLetter(inputChar);
        //    }
        //}
    

}
#endif