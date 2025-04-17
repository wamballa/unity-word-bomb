using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool logToConsole = true;
    public GameManager gameManager;

    // UI References
    public TMP_Text scoreText;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Transform dangerLine;

    public Button restartButton;
    public Button restartInGameOverButton;
    public Button openPausePanelButton;
    public Button closePausePanelButton;
    public Button toggleMuteButton;
    public Button shareButton;

    // Audio
    public GameObject audioOn;
    public GameObject audioOff;

    void Start()
    {
        Initiate();
    }

    private void Initiate()
    {
        Log("Initiate");
        FeedbackManager.Instance.PlayScoreMove();
        FeedbackManager.Instance.PlayGroundMove();
        FeedbackManager.Instance.PlayPauseMove();
        FeedbackManager.Instance.PlayKeyboardMove();

        openPausePanelButton.onClick.AddListener(HandlePausePanel);
        closePausePanelButton.onClick.AddListener(HandlePausePanel);
        toggleMuteButton.onClick.AddListener(ToggleMute);
        restartButton.onClick.AddListener(HandleRestartGame);
        restartInGameOverButton.onClick.AddListener(HandleRestartGame);
        shareButton.onClick.AddListener(HandleSharing);

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        UpdateDangerLine();
    }

    void Update()
    {
        UpdateScoreText();
        HandleGameOverUI();
    }

    public void UpdateDangerLine()
    {
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
        float groundTop = GameObject.Find("GroundTop").transform.position.y;

        float heightOfPlayArea = screenTop - groundTop;
        float threshold = gameManager.GetDangerThresholdPercent();

        float dangerLineY = groundTop + (heightOfPlayArea * (threshold / 100f));

        Vector3 pos = dangerLine.position;
        pos.y = dangerLineY;
        dangerLine.position = pos;
    }


    private void ToggleMute()
    {
        gameManager.ToggleMute();
        bool isMuted = gameManager.GetIsMuted();
        audioOff.SetActive(isMuted);
        audioOn.SetActive(!isMuted);
    }

    private void UpdateScoreText()
    {
        scoreText.text = gameManager.GetScore().ToString();
    }

    private void HandleGameOverUI()
    {
        if (!gameManager.GetIsGameOver()) return;

        if (!gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            pausePanel.SetActive(true);
        }
    }

    private void HandlePausePanel()
    {
        gameManager.TogglePause();
        bool isPaused = gameManager.GetIsPaused();
        pausePanel.SetActive(isPaused);
    }

    private void HandleRestartGame()
    {
        Log("Handle Restart Game");
        gameManager.RestartGame();
    }

    private void HandleSharing()
    {
        Log("Handle Sharing");
    }

    void Log(object message)
    {
        if (logToConsole)
            Debug.Log("[UIManager] " + message);
    }

    void LogError(object message)
    {
        if (logToConsole)
            Debug.LogError("[UIManager] " + message);
    }

}
