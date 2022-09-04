using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour {

	//private static string[] letterList = {   "!", "@", "£", "$", "%",
 //                                   "&", "*", "(", ")"    };

	private static string[] letterList = {"1", "2", "3", "4", "5",
									"6", "7", "8", "9", "0"    };

	public static string GetRandomLetter ()
	{
		int randomIndex = Random.Range(0, letterList.Length);
		string randomLetter = letterList[randomIndex];

		return randomLetter;
	}

}
