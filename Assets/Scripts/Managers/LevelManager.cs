//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;
//using UnityEngine.UI;
//using UnityEditor;
//using System;

//public class LevelManager : MonoBehaviour
//{

//    #region Variables

//    // Scene Stuff
//    private int sceneNumber;

//    /////////// Start Scene Stuff

//    [Header("Used for Start Scene")]
  
//    private int highscore;

//    //public Toggle toggle;
//    bool canUpdatePrefs = false;

//    // Audio
//    private bool isMuted = false;

//    // Pause
//    private bool isPaused  = false;

//    [Header("Used for Main Scene")]
//    bool hasHighScoreBeenSet;
//    bool isGameOver;

//    [Header("Used for both scenes")]

//    private GameManager gameManager;

//    #endregion

//    private void Awake()
//    {
//        sceneNumber = SceneManager.GetActiveScene().buildIndex;
//    }

//    void Start()
//    {
//        LoadSettings();
//        Initiate();
//    }

//    private void Initiate()
//    {
//        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//        //switch (sceneNumber)
//        //{
//        //    case 0:
//        //        //highscoreText = GameObject.Find("HighScoreValue").GetComponent<TMP_Text>();
//        //        optionsPanel.SetActive(false);
//        //        helpPanel.SetActive(false);
//        //        UpdateStartSceneUI();
//        //        break;
//        //    case 1:
//        //        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

//        //        gameOverMenu.SetActive(false);
//        //        restartButton.SetActive(false);
//        //        highscoreTextObject.SetActive(false);
//        //        GameObject.Find("UIAppear").GetComponent<MMFeedbacks>()?.PlayFeedbacks();
//        //        break;
//        //}
//    }

//    private void Update()
//    {
//        HandleGameOver();
//    }

//    #region Game Over Stuff

//    public void SetIsGameOver()
//    {
//        isGameOver = true;
//    }

//    public bool GetIsGameOver()
//    {
//        return isGameOver;
//    }

//    private void HandleGameOver()
//    {
//        if (!isGameOver) return;
//        SetHighScore();
//    }

//    #endregion

//    #region Score Stuff

//    void SetHighScore()
//    {
//        if (hasHighScoreBeenSet) return;
//        hasHighScoreBeenSet = true;
//        int highscore = PlayerPrefs.GetInt("highscore");
//        int score = gameManager.GetScore();

//        GameObject.Find("HighScoreValue").GetComponent<TMP_Text>().text = highscore.ToString();


//        if (score > highscore)
//        {
//            int delta = score - highscore;
//            highscore = score;
//            GameObject.Find("HighScoreValue").GetComponent<TMP_Text>().text = highscore.ToString();

//            GameObject.Find("Delta").GetComponent<TMP_Text>().text = "+ " + delta.ToString();
//            PlayerPrefs.SetInt("highscore", score);
//        }
//        else
//        {
//            GameObject.Find("Delta").GetComponent<TMP_Text>().text = "";
//        }
//        //GameObject.Find("HighScoreFeedback").GetComponent<MMFeedbacks>()?.PlayFeedbacks();
//    }

//    public void ClearHighScore()
//    {
//        highscore = 0;
//        PlayerPrefs.SetInt("highscore", 0);
//        //UpdateStartSceneUI();
//    }

//    #endregion

//    //#region Options Menu
//    //public void OpenOptions()
//    //{
//    //    print("OPEN OPTIONS");
//    //    if (isHelpOpen) return;
//    //    bool isEnabled = optionsPanel.activeSelf;
//    //    if (!isEnabled) optionsPanel.SetActive(true);
//    //    isOptionsOpen = true;
//    //}

//    //public void CloseOptions()
//    //{
//    //    bool isEnabled = optionsPanel.activeSelf;
//    //    if (isEnabled) optionsPanel.SetActive(false);
//    //    isOptionsOpen = false;
//    //}

//    //#endregion

//    public void PauseGame()
//    {
//        isPaused = true;
//        Time.timeScale = 0;
//    }

//    public void UnPauseGame()
//    {
//        isPaused = false;
//        Time.timeScale = 1;
//    }

//    public bool GetIsPaused()
//    {
//        return isPaused;
//    }

//    public void ToggleMute()
//    {
//        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
//        isMuted = !isMuted;
//        PlayerPrefs.SetInt("isMuted", (isMuted ? 1 : 0));
//    }

//    public bool GetIsMuted()
//    {
//        return isMuted;
//    }



//    //void UpdateStartSceneUI()
//    //{
//    //    highscoreText.text = highscore.ToString();
//    //    // update toggle
//    //    if (SceneManager.GetActiveScene().buildIndex == 0)
//    //    {
//    //        if (isMuted)
//    //        {
//    //            audioOff.SetActive(true);
//    //            audioOn.SetActive(false);
//    //        }
//    //        else
//    //        {
//    //            audioOff.SetActive(false);
//    //            audioOn.SetActive(true);
//    //        }
//    //    }
//    //    canUpdatePrefs = true;
//    //}

//    void LoadSettings()
//    {
//        highscore = PlayerPrefs.GetInt("highscore", 0);
//        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
//    }

//    #region Level Stuff

//    public void GoToNextLevel()
//    {
//        //if (isOptionsOpen) return;
//        //if (isHelpOpen) return;

//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//    }


//    public void RestartGame()
//    {
//        //gameOverMenu.SetActive(false);
//        //restartButton.SetActive(false);
//        SceneManager.LoadScene("Start");
//    }

//    #endregion


//    public void RateMyApp()
//    {
//#if UNITY_IOS
//        Application.OpenURL("market://details?id=com.trollugames.caverun3d");

//#endif


//    }

//}
