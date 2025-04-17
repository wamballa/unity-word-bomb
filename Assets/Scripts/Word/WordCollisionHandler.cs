using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WordCollisionHandler : MonoBehaviour
{

    public bool logToConsole = true;
    private FallingWord fallingWord;


    private void Awake()
    {
        fallingWord = GetComponent<FallingWord>();

    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground")
            || collision.transform.CompareTag("ExplodedLetter")
            || collision.transform.CompareTag("CrashedNumber"))
        {
            // Log($"{transform.name} collided with {collision.transform.tag}");

            if (fallingWord != null && fallingWord.state == FallingWord.FallingWordState.Falling)
            {
                // Log("[CollisionHandler] " + collision.transform.name);
                fallingWord.OnCrash(); // call refactored crash logic
            }

        }
    }

    void Log(object message)
    {
        if (logToConsole)
            Debug.Log("[CollisionHandler] " + message);
    }

    void LogError(object message)
    {
        if (logToConsole)
            Debug.LogError("[CollisionHandler] " + message);
    }

}
