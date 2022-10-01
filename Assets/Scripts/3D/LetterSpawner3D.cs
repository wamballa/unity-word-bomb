using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSpawner3D : MonoBehaviour {

    #region VARIABLES
    //public GameObject wordPrefab;
	public GameObject letterPrefab;
	private WordManager3D wordManager;

	private GameManager gameManager;

	// Level constants
	//float wordDelay = 5f;
	//private float nextWordTime = 0f;
	private bool canSpawn = false;
	private const float SPAWN_HEIGHT = 5.5f;

	// Letter stuff
	float letterDelay = 3f;
	private float nextLetterTime = 0f;

	// Which way is object facing?
	private bool isFacingLeft = true;

    #endregion


    private void Start()
    {
		Initialise();
	}


	private void Initialise()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (gameManager == null) Debug.Log("ERROR");

		wordManager = GameObject.Find("WordManager").GetComponent<WordManager3D>();
		if (wordManager == null) Debug.Log("ERROR");

		SetSpawn(true);

		nextLetterTime = Time.time + gameManager.GetFallDelayTime("letter");
	}


    private void Update()
    {
		//SpawnWord();
        SpawnLetter();
    }


	public void SetSpawn(bool b)
    {
		canSpawn = b;
    }


	public void GetDelayTimes()
    {
		//wordDelay = gameManager.GetFallDelayTime("word");
		letterDelay = gameManager.GetFallDelayTime("letter");
	}


	private void SpawnLetter()
	{
		Random.InitState(Random.Range(1,100));
		if (!canSpawn) return;

		bool prob = Random.Range(0, 10000) > 9990 ? true : false;
		//print("Game Filled % " + gameManager.GetPercentageFilled());
        if (gameManager.GetPercentageFilled() < 25) return;

		//if (prob)
		if (Time.time >= nextLetterTime)
        {
			wordManager.AddLetter(GetLetter());
			nextLetterTime = Time.time + gameManager.GetFallDelayTime("letter");
		}
	}


	public GameObject GetLetter()
    {
		//Vector3 randomPosition = new Vector3(Random.Range(-6f, 6f), SPAWN_HEIGHT);
		//GameObject letterObj = Instantiate(letterPrefab, randomPosition, Quaternion.identity);
		//return letterObj;

		GameObject letterObj = Instantiate(letterPrefab);
		letterObj.GetComponent<Letter3D>().SetDirectionFacing(isFacingLeft);
		isFacingLeft = !isFacingLeft;
		return letterObj;


	}
}
