using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class LevelManager : MonoBehaviour
{
    // Scene Stuff
    private int sceneNumber;

    /////////// Start Scene Stuff
    
    [Header("Used for Start Scene")]
    public TMP_Text highscoreText;
    private int highscore;
    private bool isMuted = false;
    //public Toggle toggle;
    bool canUpdatePrefs = false;

    // Options Menu

    bool isOptionsOpen = false;
    // Help Menu
    [SerializeField] private GameObject helpPanel;
    bool isHelpOpen = false;

    // Audio Button
    [SerializeField] private GameObject audioOn;
    [SerializeField] private GameObject audioOff;

    /////////// MAIN SCENE STUFF
    // Game Over Menu
    [Header("Used for Main Scene")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject highscoreTextObject;
    bool hasHighScoreBeenSet;
    bool isGameOver;

    [Header("Used for both scenes")]
    [SerializeField] private GameObject optionsPanel;
    private GameManager gameManager;


    private void Awake()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    
    void Start()
    {

        LoadSettings();

        switch (sceneNumber)
        {
            case 0:
                //highscoreText = GameObject.Find("HighScoreValue").GetComponent<TMP_Text>();
                optionsPanel.SetActive(false);
                helpPanel.SetActive(false);
                UpdateStartSceneUI();
                break;
            case 1:
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

                gameOverMenu.SetActive(false);
                restartButton.SetActive(false);
                highscoreTextObject.SetActive(false);
                GameObject.Find("UIAppear").GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                break;
        }
    }


    private void Update()
    {
        HandleGameOverUI();
    }


    private void HandleGameOverUI()
    {
        if (!isGameOver) return;
        gameOverMenu.SetActive(true);
        restartButton.SetActive(true);
        highscoreTextObject.SetActive(true);
        SetHighScore();
    }


    void SetHighScore()
    {
        if (hasHighScoreBeenSet) return;
        hasHighScoreBeenSet = true;
        int highscore = PlayerPrefs.GetInt("highscore");
        int score = gameManager.GetScore();

        //print("Highscre = " + highscore);
        //print("Score " + score);

        GameObject.Find("HighScoreValue").GetComponent<TMP_Text>().text = highscore.ToString();


        if (score > highscore)
        {
            int delta = score - highscore;
            highscore = score;
            GameObject.Find("HighScoreValue").GetComponent<TMP_Text>().text = highscore.ToString();

            GameObject.Find("Delta").GetComponent<TMP_Text>().text = "+ " + delta.ToString();
            PlayerPrefs.SetInt("highscore", score);
        }
        else
        {
            GameObject.Find("Delta").GetComponent<TMP_Text>().text = "";
        }
        GameObject.Find("HighScoreFeedback").GetComponent<MMFeedbacks>()?.PlayFeedbacks();
    }


    public void SetGameOver()
    {
        isGameOver = true;
    }


    public void ClearHighScore()
    {
        highscore = 0;
        PlayerPrefs.SetInt("highscore", 0);
        UpdateStartSceneUI();
    }


    public void CloseOptions()
    {
        bool isEnabled = optionsPanel.activeSelf;
        if (isEnabled) optionsPanel.SetActive(false);
        isOptionsOpen = false;
    }


    public void ToggleMute()
    {
        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", (isMuted ? 1 : 0));
        if (isMuted)
        {
            audioOff.SetActive(true);
            audioOn.SetActive(false);
        }
        else
        {
            audioOff.SetActive(false);
            audioOn.SetActive(true);
        }
    }


    public void OpenHelpMenu()
    {
        if (isOptionsOpen) return;
        bool isEnabled = helpPanel.activeSelf;
        if (!isEnabled) helpPanel.SetActive(true);
        isHelpOpen = true;
    }


    public void CloseHelpMenu()
    {
        bool isEnabled = helpPanel.activeSelf;
        if (isEnabled) helpPanel.SetActive(false);
        isHelpOpen = false;
    }


    void UpdateStartSceneUI()
    {
        highscoreText.text = highscore.ToString();
        // update toggle
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (isMuted)
            {
                audioOff.SetActive(true);
                audioOn.SetActive(false);
            }
            else
            {
                audioOff.SetActive(false);
                audioOn.SetActive(true);
            }
        }
        canUpdatePrefs = true;
    }


    void LoadSettings()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
    }

    public void OpenOptions()
    {
        print("OPEN OPTIONS");
        if (isHelpOpen) return;
        bool isEnabled = optionsPanel.activeSelf;
        if (!isEnabled) optionsPanel.SetActive(true);
        isOptionsOpen = true;
    }


    public void GoToNextLevel()
    {
        if (isOptionsOpen) return;
        if (isHelpOpen) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void RestartGame()
    {
        gameOverMenu.SetActive(false);
        restartButton.SetActive(false);
        SceneManager.LoadScene("Start");
    }


    public void RateMyApp()
    {
#if UNITY_IOS
        Application.OpenURL("market://details?id=com.trollugames.caverun3d");

#endif


    }

}
