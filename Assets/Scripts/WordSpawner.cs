using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {

    #region VARIABLES
    public GameObject wordPrefab;
	public GameObject letterPrefab;
	public WordManager wordManager;

	private GameManager gameManager;

	// Level constants
	float wordDelay = 5f;
	private float nextWordTime = 0f;
	private bool canSpawn = false;
	private const float SPAWN_HEIGHT = 5.5f;

	// Letter stuff
	float letterDelay = 3f;
	private float nextLetterTime = 0f;
    #endregion


    private void Start()
    {
		Initialise();
	}


	private void Initialise()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (gameManager == null) Debug.Log("ERROR");

		SetSpawn(true);
		nextLetterTime = Time.time + gameManager.GetFallDelayTime("letter");
	}


    private void Update()
    {
		SpawnWord();
        SpawnLetter();
    }


	public void SetSpawn(bool b)
    {
		canSpawn = b;
    }



	public void GetDelayTimes()
    {
		wordDelay = gameManager.GetFallDelayTime("word");
		letterDelay = gameManager.GetFallDelayTime("letter");
	}


	private void SpawnWord()
	{
		if (!canSpawn) return;

		if (Time.time >= nextWordTime )
		{
			//Debug.Log("DROP WORD");
			wordManager.AddWord(GetWord());
			nextWordTime = Time.time + gameManager.GetFallDelayTime("word");
			//_wordCounter++;
		}
	}


	private void SpawnLetter()
	{
		Random.InitState(Random.Range(1,100));
		if (!canSpawn) return;

		bool prob = Random.Range(0, 10000) > 9990 ? true : false;
		//print ("Game Filled % "+gameManager.GetPercentageFilled()
		if (gameManager.GetPercentageFilled() < 25) return;

		//if (prob)
		if (Time.time >= nextLetterTime)
        {
			wordManager.AddLetter(GetLetter());
			nextLetterTime = Time.time + gameManager.GetFallDelayTime("letter");
		}
	}


	public GameObject GetWord()
    {

		//gameManager.GetDifficultyLevel();

        Vector3 randomPosition = new Vector3(Random.Range(-3.5f, 3.5f), SPAWN_HEIGHT);
        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity);
        return wordObj;
    }


	public GameObject GetLetter()
    {
		Vector3 randomPosition = new Vector3(Random.Range(-6f, 6f), SPAWN_HEIGHT);
		GameObject letterObj = Instantiate(letterPrefab, randomPosition, Quaternion.identity);
		return letterObj;
	}
}
