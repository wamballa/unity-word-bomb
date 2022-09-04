using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private TMP_Text highscoreText;
    private int highscore;
    private bool isMuted = false;
    public Toggle toggle;
    bool canUpdatePrefs = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            highscoreText = GameObject.Find("HighScoreValue").GetComponent<TMP_Text>();
        }
        catch
        {
            print("ERROR: can't find high score text!");
        }
        LoadSettings();
        UpdateUI();
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Start");
    }

    void UpdateUI()
    {

        highscoreText.text = highscore.ToString();
        // update toggle
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (isMuted)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }
        canUpdatePrefs = true;
    }

    void LoadSettings()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
    }

    public void ToggleIsMuted()
    {
        if (canUpdatePrefs)
        {
            isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
            print("is muted " + isMuted);
            isMuted = !isMuted;
            PlayerPrefs.SetInt("isMuted", (isMuted ? 1 : 0));
            print("Is mute = " + (PlayerPrefs.GetInt("isMuted") != 0));
        }

    }
}
