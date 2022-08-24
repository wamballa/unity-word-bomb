using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour {

	//public WordManager wordManager;

	//private GameManager gameManager;

	//// Level constants
	//private int wordCount = 0;
	//private int letterCount = 0;
	//private float wordDelay = 5f;
	//private float nextWordTime = 0f;
	//private bool canDropWord = false;

	//// Counters and timers
	//private float letterDelay = 2f;
	//private float nextLetterTime = 0f;
	//bool canDropLetterBomb = false;
	//private int _wordCounter = 0;
	//private int _letterCounter = 0;
	//private bool lastWordDropped = false;
	//private bool lastLetterDropped = false;
	////private bool allLettersWordsDropped = false;

	//private void Start()
 //   {
	//	gameManager = GameObject.Find("LevelManager").GetComponent<GameManager>();
	//	if (gameManager == null) Debug.Log("ERROR");

	//	if (wordManager == null) Debug.Log("ERROR: Word Manager NULL");

	//	GetLevelConfig();
	//	//Debug.Log("Word Count / Letter Count = " + wordCount+" / "+letterCount);

	//}

 //   private void Update()
	//{


	//	//  spawn letters bombs
	//	if (Time.time >= nextLetterTime && canDropLetterBomb && !lastLetterDropped)
	//	{
	//		wordManager.AddLetter();
	//		nextLetterTime = Time.time + letterDelay;
	//		//letterDelay *= .99f;
	//		if (_letterCounter == letterCount - 1)
	//		{
	//			lastWordDropped = true;
	//		}
	//	}
	//	SetIfAllDropped();
	//}

	//private void WordDropHandler()
 //   {
	//	if (Time.time >= nextWordTime && canDropWord && !lastWordDropped)
	//	{
	//		Debug.Log("DROP WORD");
	//		//wordManager.AddWord();
	//		nextWordTime = Time.time + wordDelay;
	//		_wordCounter++;
	//		//wordDelay *= .99f;
	//		if (_wordCounter == wordCount)
	//		{
	//			lastWordDropped = true;
	//			Debug.Log("LAST WORD DROPPED ");
	//		}
	//	}
	//}

	//private void SetIfAllDropped()
	//{
	//	// Check letters - if none were set then defaults to dropped
	//	if (!canDropLetterBomb) lastLetterDropped = true;
	//	if (!canDropWord) lastWordDropped = true;

	//	if (lastWordDropped && lastLetterDropped)
	//	{
	//		wordManager.SetAllDropped(true);
	//	} 
 //   }

	//void GetLevelConfig()
 //   {

	//	gameManager.GetLevelConfig(
	//		out wordCount,
	//		out wordDelay,
	//		out canDropWord,
	//		out letterCount,
	//		out letterDelay,
	//		out canDropLetterBomb);

 //   }

}
