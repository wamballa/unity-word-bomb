using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {

	public GameObject wordPrefab;
	public GameObject letterPrefab;
	public WordManager wordManager;

	private GameManager gameManager;

	// Level constants
	private int wordCount = 0;
	private int letterCount = 0;
	private float wordDelay = 5f;
	private float nextWordTime = 0f;
	private bool canDropWord = false;

	// Counters and timers
	private float letterDelay = 2f;
	private float nextLetterTime = 0f;
	bool canDropLetterBomb = false;
	private int _wordCounter = 0;
	private int _letterCounter = 0;
	private bool lastWordDropped = false;
	private bool lastLetterDropped = false;

	private void Start()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (gameManager == null) Debug.Log("ERROR");

		GetLevelConfig();

		canDropWord = true;
		nextWordTime = 1f;

	}

    private void Update()
    {
		CheckIfSpawnWord();

	}

	private void CheckIfSpawnWord()
	{
		if (!canDropWord) return;

		if (lastWordDropped) return;

		if (Time.time >= nextWordTime )
		{
			Debug.Log("DROP WORD");
			wordManager.AddWord(GetWord());
			nextWordTime = Time.time + wordDelay;
			_wordCounter++;
			//wordDelay *= .99f;
			if (_wordCounter == wordCount)
			{
				lastWordDropped = true;
				Debug.Log("LAST WORD DROPPED ");
			}
		}
	}

    public GameObject GetWord()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-3.5f, 3.5f), 7f);
        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity);
        return wordObj;
    }

    //public Word GetWord()
    //{
    //	Vector3 randomPosition = new Vector3(Random.Range(-3.5f, 3.5f), 7f);
    //	Word wordObj = new Word();
    //       return wordObj;
    //}

    public GameObject SpawnLetter()
	{
		Vector3 randomPosition = new Vector3(Random.Range(-6f, 6f), 7f);
		GameObject letterObj = Instantiate(letterPrefab, randomPosition, Quaternion.identity);
		return letterObj;
	}

	void GetLevelConfig()
	{

		gameManager.GetLevelConfig(
			out wordCount,
			out wordDelay,
			out canDropWord,
			out letterCount,
			out letterDelay,
			out canDropLetterBomb);
	}


}
