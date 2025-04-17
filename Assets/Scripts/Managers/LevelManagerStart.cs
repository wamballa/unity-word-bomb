using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using System;

public class LevelManagerStart : MonoBehaviour
{

    #region Variables

    // Scene Components
    public GameObject optionsPanel;
    public GameObject helpPanel;

    // Buttons
    public Button openOptionsButton;
    public Button openHelpButton;
    public Button closeOptionButton;
    public Button closeHelpButton;
    public Button playButton;

    /////////// Start Scene Stuff

    [Header("Used for Start Scene")]
  
    private int highscore;

    // Private Bools
    private bool canUpdatePrefs = false;
    private bool isMuted = false;

    #endregion

    private void Awake()
    {
    }

    void Start()
    {
        LoadSettings();
        Initiate();
    }

    private void Initiate()
    {
        if (openOptionsButton != null) openOptionsButton.onClick.AddListener(OnOpenOptionsButtonClicked);
        if (openHelpButton != null) openHelpButton.onClick.AddListener(OnOpenHelpButtonClicked);
        if (closeOptionButton != null) closeOptionButton.onClick.AddListener(ResetMenus);
        if (closeHelpButton != null) closeHelpButton.onClick.AddListener(ResetMenus);
        if (playButton != null) playButton.onClick.AddListener(OnPlayButtonPressed);

        ResetMenus();
    }

    private void Update()
    {
    }


    public void ClearHighScore()
    {
        highscore = 0;
        PlayerPrefs.SetInt("highscore", 0);
        //UpdateStartSceneUI();
    }

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void OnOpenOptionsButtonClicked()
    {
        if (helpPanel.activeSelf) helpPanel.SetActive(false);
        if (optionsPanel.activeSelf) return; else optionsPanel.SetActive(true);
    }

        public void OnOpenHelpButtonClicked()
    {
        if (optionsPanel.activeSelf) optionsPanel.SetActive(false);
        if (helpPanel.activeSelf) return; else helpPanel.SetActive(true);
    }
    public void ResetMenus()
    {
        optionsPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void ToggleMute()
    {
        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", (isMuted ? 1 : 0));
    }

    public bool GetIsMuted()
    {
        return isMuted;
    }

    void LoadSettings()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
    }

    #region Level Stuff

    public void GoToNextLevel()
    {
        //if (isOptionsOpen) return;
        //if (isHelpOpen) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void RestartGame()
    {
        //gameOverMenu.SetActive(false);
        //restartButton.SetActive(false);
        SceneManager.LoadScene("Start");
    }

    #endregion


    public void RateMyApp()
    {
#if UNITY_IOS
        Application.OpenURL("market://details?id=com.trollugames.caverun3d");

#endif


    }

}
