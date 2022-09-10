using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {

	public GameObject wordPrefab;
	public GameObject letterPrefab;
	public WordManager wordManager;

	private GameManager gameManager;

	// Level constants
	public float wordDelay = 5f;
	private float nextWordTime = 0f;
	private bool canSpawn = false;

	// Letter stuff
	//bool canDropLetter = false;
	public float letterDelay = 3f;
	private float nextLetterTime = 0f;

	private void Start()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (gameManager == null) Debug.Log("ERROR");

		//canDropLetter = true;
		SetSpawn(true);

		//wordDelay = 1f;

		//Get delay times
		SetDelayTimes();

		nextLetterTime = Time.time + letterDelay;
	}

    private void Update()
    {
		SpawnWord();
        SpawnLetter();
		SetDelayTimes();
    }

	public void SetSpawn(bool b)
    {
		canSpawn = b;
    }

	/// <summary>
    /// Get delay timers from GameManager
    /// </summary>
	public void SetDelayTimes()
    {
		wordDelay = gameManager.GetFallDelayTime("word");
		letterDelay = gameManager.GetFallDelayTime("letter");
	}
	/// <summary>
    /// Spawns a word
    /// </summary>
	private void SpawnWord()
	{
		if (!canSpawn) return;

		if (Time.time >= nextWordTime )
		{
			//Debug.Log("DROP WORD");
			wordManager.AddWord(GetWord());
			nextWordTime = Time.time + wordDelay;
			//_wordCounter++;
		}
	}
	/// <summary>
    /// Gets the current word
    /// </summary>
    /// <returns></returns>
    public GameObject GetWord()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-3.5f, 3.5f), 7f);
        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity);
        return wordObj;
    }
    private void SpawnLetter()
	{
		if (!canSpawn) return;

		if (Time.time >= nextLetterTime)
		{
			wordManager.AddLetter(GetLetter());
			nextLetterTime = Time.time + letterDelay;
		}
	}
	public GameObject GetLetter()
    {
		Vector3 randomPosition = new Vector3(Random.Range(-6f, 6f), 7f);
		GameObject letterObj = Instantiate(letterPrefab, randomPosition, Quaternion.identity);
		return letterObj;
	}
}
