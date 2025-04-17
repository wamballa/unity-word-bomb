using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject wordPrefab;
    public GameObject numberPrefab;
    public WordGameplayManager wordManager;
    public GameManager gameManager;

    //float wordDelay = 5f;

    public bool canSpawnWord = false;
    public bool canSpawnNumber = false;

    private const float SPAWN_HEIGHT = 5.5f;

    private float nextNumberTime = 0f;
    private float nextWordTime = 0f;

    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        nextWordTime = Time.time + gameManager.GetFallDelayTime("word");
        nextNumberTime = Time.time + gameManager.GetFallDelayTime("number");
        //canSpawnWord = true;
        //canSpawnNumber = true;
    }

    private void Update()
    {
        TrySpawnWord();
        TrySpawnNumber();
    }

    private void TrySpawnWord()
    {
        if (!canSpawnWord || Time.time < nextWordTime) return;

        GameObject newWord = Instantiate(wordPrefab, GetSpawnPositionForWord(), Quaternion.identity);
        wordManager.AddWordAsPrefab(newWord);
        nextWordTime = Time.time + gameManager.GetFallDelayTime("word");

    }

    private void TrySpawnNumber()
    {
        if (!canSpawnNumber || Time.time < nextNumberTime) return;

        GameObject newNumber = Instantiate(numberPrefab, GetSpawnPositionForNumber(), Quaternion.identity);
        wordManager.AddNumber(newNumber);
        float newTime = Time.time + gameManager.GetFallDelayTime("number");
        //print("<<< " + gameManager.GetFallDelayTime("number"));
        nextNumberTime = newTime;

    }

    private Vector3 GetSpawnPositionForWord()
    {
        // Get half the screen width in world units
        float halfWordWidth = 3.5f; // fallback default
        float charWidth = 0.3f;
        int difficulty = gameManager.GetWordDifficultyLevel();

        // Estimate average word length per difficulty
        int wordLength = Mathf.Clamp(difficulty + 2, 4, 12); // e.g., 5–12 letters
        float estimatedWidth = (wordLength * charWidth);

        float minX = Camera.main.ViewportToWorldPoint(Vector3.zero).x + estimatedWidth / 2f;
        float maxX = Camera.main.ViewportToWorldPoint(Vector3.right).x - estimatedWidth / 2f;

        return new Vector3(Random.Range(minX, maxX), SPAWN_HEIGHT, 0f);
    }

    private Vector3 GetSpawnPositionForNumber()
    {
        float minX = Camera.main.ViewportToWorldPoint(Vector3.zero).x + 0.5f;
        float maxX = Camera.main.ViewportToWorldPoint(Vector3.right).x - 0.5f;

        return new Vector3(Random.Range(minX, maxX), SPAWN_HEIGHT, 0f);
    }

    public void SetSpawn(bool enable)
    {
        canSpawnWord = enable;
    }
}
