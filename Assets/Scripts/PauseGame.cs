using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{

    public Image pauseButtonImage;
    bool isPaused = false;
    public Sprite playImage;
    public Sprite pauseImage;

    public void TogglePause()
    {
        if (isPaused)
        {
            print("UN PAUSE GAME");
            pauseButtonImage.sprite = pauseImage;
            isPaused = false;
            Time.timeScale = 1;
        }
        else
        {
            print("PAUSE GAME");
            pauseButtonImage.sprite = playImage;
            isPaused = true;
            Time.timeScale = 0;
        }

    }

}
