using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {

	public GameObject wordPrefab;
	public GameObject letterPrefab;


	public GameObject SpawnWord ()
	{
		Vector3 randomPosition = new Vector3(Random.Range(-3.5f, 3.5f), 7f);
        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity);
        return wordObj;
	}

	public GameObject SpawnLetter()
	{
		Vector3 randomPosition = new Vector3(Random.Range(-6f, 6f), 7f);
		GameObject letterObj = Instantiate(letterPrefab, randomPosition, Quaternion.identity);
		return letterObj;
	}

}
