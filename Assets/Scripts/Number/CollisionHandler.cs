using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionHandler : MonoBehaviour
{

    public bool logToConsole = true;

    private NumberController numberController;

    private void Awake()
    {

        numberController = GetComponentInParent<NumberController>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground")
            || collision.transform.CompareTag("ExplodedLetter")
            || collision.transform.CompareTag("CrashedNumber"))
        {

            if (numberController != null && numberController.state == NumberController.NumberState.Falling)
            {
                Log($"{transform.name} collided with {collision.transform.name}");
                numberController.state = NumberController.NumberState.Crashed;
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
