using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGenerator : MonoBehaviour
{

    //private static string[] letterList = {   "!", "@", "£", "$", "%",
    //                                   "&", "*", "(", ")"    };

    private static int[] numberList = {
    0,1,2,3,4,5,6,7,8,9
    };

    public static int GetRandomNumber()
    {
        int randomIndex = Random.Range(0, numberList.Length);
        int randomNumber = numberList[randomIndex];

        return randomNumber;
    }

}
