using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour {

	private static string[] letterList = {   "!", "@", "£", "$", "%",
                                    "&", "*", "(", ")"    };

	public static string GetRandomLetter ()
	{
		int randomIndex = Random.Range(0, letterList.Length);
		string randomLetter = letterList[randomIndex];

		return randomLetter;
	}

}
