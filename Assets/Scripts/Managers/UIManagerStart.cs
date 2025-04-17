//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEditor;
//using UnityEngine;
//using MoreMountains.Feedbacks;

//public class UIManagerStart : MonoBehaviour
//{
//    private LevelManager levelManager;
//    private GameManager gameManager;

//    // High Score
//    public TMP_Text highscoreText;

//    // Game Over Menu
//    public GameObject gameOverMenu;
//    public GameObject restartButton;
//    public GameObject highscoreTextObject;

//    // Option Menu
//    bool isOptionsOpen = false;
//    public GameObject optionsPanel;

//    // Audio
//    public GameObject audioOn;
//    public GameObject audioOff;

//    // Help Menu
//    public GameObject helpPanel;
//    bool isHelpOpen = false;

//    // Pause Menu
//    public GameObject pausePanel;
//    bool isPaused = false;
//    bool isPauseOpen = false;

//    void Start()
//    {
//        Initiate();
//    }

//    private void Initiate()
//    {
//        levelManager = FindAnyObjectByType<LevelManager>();

//        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

//        gameOverMenu.SetActive(false);
//        restartButton.SetActive(false);
//        highscoreTextObject.SetActive(false);
//        GameObject.Find("UIAppear").GetComponent<MMFeedbacks>()?.PlayFeedbacks();

//    }

//    void Update()
//    {
//        HandleGameOverUI();
//        HandleAudioMuteToggle();
//    }



//    private void HandleGameOverUI()
//    {
//        if (levelManager.GetIsGameOver()) return;
//        gameOverMenu.SetActive(true);
//        restartButton.SetActive(true);
//        highscoreTextObject.SetActive(true);
//    }

//    #region Pause Menu

//    public void OpenPause()
//    {
//        print("OPEN OPTIONS");
//        if (isPauseOpen) return;
//        bool isEnabled = pausePanel.activeSelf;
//        if (!isEnabled) pausePanel.SetActive(true);
//        isPauseOpen = true;
//        TogglePause();
//    }

//    public void ClosePause()
//    {
//        bool isEnabled = pausePanel.activeSelf;
//        if (isEnabled) pausePanel.SetActive(false);
//        isPauseOpen = false;
//        TogglePause();
//    }


//    public void TogglePause()
//    {
//        if (isPaused)
//        {
//            print("UN PAUSE GAME");
//            isPaused = false;
//            Time.timeScale = 1;
//        }
//        else
//        {
//            print("PAUSE GAME");
//            isPaused = true;
//            Time.timeScale = 0;
//        }

//    }

//    #endregion
//    public void HandleAudioMuteToggle()
//    {
//        if (levelManager.GetIsMuted())
//        {
//            audioOff.SetActive(true);
//            audioOn.SetActive(false);
//        }
//        else
//        {
//            audioOff.SetActive(false);
//            audioOn.SetActive(true);
//        }
//    }

//    #region Help Menu
//    public void OpenHelpMenu()
//    {
//        if (isOptionsOpen) return;
//        bool isEnabled = helpPanel.activeSelf;
//        if (!isEnabled) helpPanel.SetActive(true);
//        isHelpOpen = true;
//    }


//    public void CloseHelpMenu()
//    {
//        bool isEnabled = helpPanel.activeSelf;
//        if (isEnabled) helpPanel.SetActive(false);
//        isHelpOpen = false;
//    }

//    #endregion
//}
