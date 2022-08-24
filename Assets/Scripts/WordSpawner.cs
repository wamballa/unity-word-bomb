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

	bool canDropLetterBomb = false;
	private int _wordCounter = 0;
	private int _letterCounter = 0;
	//private bool lastWordDropped = false;
	private bool lastLetterDropped = false;

	// Letter stuff
	bool canDropLetter = false;
	private float letterDelay = 3f;
	private float nextLetterTime = 0f;

	private void Start()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (gameManager == null) Debug.Log("ERROR");

		canDropLetter = true;

		canDropWord = true;
		wordDelay = 1f;

		//nextLetterTime = Time.time + letterDelay;
	}

    private void Update()
    {
		SpawnWord();
        SpawnLetter();
    }

	private void SpawnWord()
	{
		if (!canDropWord) return;

		if (Time.time >= nextWordTime )
		{
			//Debug.Log("DROP WORD");
			wordManager.AddWord(GetWord());
			nextWordTime = Time.time + wordDelay;
			_wordCounter++;
		}
	}
    public GameObject GetWord()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-3.5f, 3.5f), 7f);
        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity);
        return wordObj;
    }
    private void SpawnLetter()
	{
		if (!canDropLetter) return;

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
