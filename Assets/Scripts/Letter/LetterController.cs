using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterController : MonoBehaviour
{


    bool isOffScreen;

    // Update is called once per frame
    void Update()
    {
        CheckIfOffScreen();
        DeleteIfOffScreen();
    }


    void CheckIfOffScreen()
    {
        if (transform.position.y < -5.5f)
        {
            isOffScreen = true;
        }
    }


    void DeleteIfOffScreen()
    {
        if (!isOffScreen) return;

        Destroy(transform.gameObject);

    }


}
