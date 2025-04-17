using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region VARIABLES
    public bool logToConsole = true;
    public AudioController audioController;

    // Game State
    private int score;
    private int highscore;
    private bool isMuted = false;
    private bool isPaused = false;
    private bool isGameOver = false;
    private bool hasHighScoreBeenSet = false;

    // Gameplay Configs
    [Range(0, 20)] public float wordFallSpeed = 1;
    [Range(0, 20)] public float wordFallDelay = 1;
    [Range(0, 20)] public float letterFallSpeed = 1;
    [Range(0, 20)] public float letterFallDelay = 1;
    [Range(0, 20)] public float numberFallSpeed = 1;
    [Range(0, 20)] public float numberFallDelay = 1;

    [Header("Difficulty Settings")]
    [SerializeField] float wordDelayDecrement = 0.2f;
    [SerializeField] float letterDelayDecrement = 0.1f;
    [SerializeField] float wordSpeedIncrement = 0.2f;
    [SerializeField] float difficultyDuration = 20f;
    [SerializeField] int wordDifficultyLevel = 3;

    // GAME TIMER
    float startTime;

    // Debug
    public TMP_Text fillPercentText;

    [SerializeField] private float dangerThresholdPercent = 90f;
    public float GetDangerThresholdPercent() => dangerThresholdPercent;


    #endregion

    void Start()
    {
        Initiatiate();
    }
    
    void Initiatiate()
    {
        Log("Initiatiate");
        startTime = Time.time;
        LoadSettings();
        StartCoroutine(GameOverCheckLoop());
        StartCoroutine(DifficultyLoop());
    }

    void Update()
    {
        // CheckToReduceWordDifficulty();

        // debug
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Log("Debug Key Pressed");
        }
    }

    IEnumerator DifficultyLoop()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(difficultyDuration);
            IncreaseDifficulty();
        }
    }

    void IncreaseDifficulty()
    {
        if (wordFallDelay > 2) wordFallDelay -= wordDelayDecrement;
        if (letterFallDelay > 2) letterFallDelay -= letterDelayDecrement;
        if (wordFallSpeed < 0.8f) wordFallSpeed += wordSpeedIncrement;
        if (wordDifficultyLevel < 7) wordDifficultyLevel++;
    }

    IEnumerator GameOverCheckLoop()
    {
        while (!isGameOver)
        {
            float fillPercent = GetPercentageFilled();
            fillPercentText.text = "Fill % = "+ fillPercent.ToString();

            yield return new WaitForSeconds(0.5f);
            if (fillPercent > dangerThresholdPercent)
            {
                Log("GameOverCheckLoop. Fill = " + fillPercent);
                yield return new WaitForSeconds(3f);
                isGameOver = true;
                SetHighScore();

            }
        }
    }


    public float GetPercentageFilled()
    {
        GameObject[] letters = GameObject.FindGameObjectsWithTag("ExplodedLetter");
        float letterHighPoint = -20;

        foreach (GameObject go in letters)
        {
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            // Only consider letters that are nearly still (stacked)
            if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            {
                float y = go.transform.position.y;
                if (y > letterHighPoint)
                {
                    letterHighPoint = y;
                }
            }
        }

        if (letters.Length == 0 || letterHighPoint < -10) return 0;

        float screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
        float groundTop = GameObject.Find("GroundTop").transform.position.y;

        float heapHeight = letterHighPoint - groundTop;
        float heightOfPlayingArea = screenTop - groundTop;

        return (heapHeight / heightOfPlayingArea) * 100f;
    }


    void SetHighScore()
    {
        if (hasHighScoreBeenSet) return;
        hasHighScoreBeenSet = true;

        int currentHigh = PlayerPrefs.GetInt("highscore", 0);
        if (score > currentHigh)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > highscore) SetHighScore();
    }

    public void RestartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Start");
    } 

    public void RateMyApp()
    {
#if UNITY_IOS
        Application.OpenURL("market://details?id=com.trollugames.wordbomb");
#endif
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        if (audioController != null) audioController.AudioMute(isMuted);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    void LoadSettings()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        isMuted = PlayerPrefs.GetInt("isMuted") != 0;
    }


    // Public Getters
    public float GetFallSpeed(string type) => type == "word" ? wordFallSpeed : letterFallSpeed;
    public float GetFallDelayTime(string type)
    {
        return type switch
        {
            "word" => wordFallDelay,
            "letter" => Random.Range(2, 4) + letterFallDelay,
            "number" => numberFallDelay,
            _ => 1f,
        };
    }

    public int GetScore() => score;
    public int GetHighScore() => highscore;
    public bool GetIsMuted() => isMuted;
    public bool GetIsPaused() => isPaused;
    public bool GetIsGameOver() => isGameOver;
    public int GetWordDifficultyLevel() => wordDifficultyLevel;





    private IEnumerator GameOver()
    {
        // 
        //wordSpawner.SetSpawn(false);
        yield return new WaitForSeconds(1f);
        //levelManager.SetIsGameOver();
    }

    void Log(object message)
    {
        if (logToConsole)
            Debug.Log("[GameManager] " + message);
    }

    void LogError(object message)
    {
        if (logToConsole)
            Debug.LogError("[GameManager] " + message);
    }


}
