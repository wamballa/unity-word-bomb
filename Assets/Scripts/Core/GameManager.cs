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
    [SerializeField] private LevelConfig levelConfig;

    // Game State
    private int score;
    private int highscore;
    private bool isMuted = false;
    private bool isPaused = false;
    private bool isGameOver = false;
    private bool hasHighScoreBeenSet = false;

    // Legacy fallback configs. Runtime tuning is copied from LevelConfig when assigned.
    [Range(0, 20)] public float wordFallSpeed = 0.4f;
    [Range(0, 20)] public float wordFallDelay = 3.2f;
    [Range(0, 20)] public float letterFallSpeed = 0.6f;
    [Range(0, 20)] public float letterFallDelay = 5f;
    [Range(0, 20)] public float numberFallSpeed = 4.6f;
    [Range(0, 20)] public float numberFallDelay = 3.1f;

    [Header("Difficulty Settings")]
    [SerializeField] float wordDelayDecrement = 0.2f;
    [SerializeField] float letterDelayDecrement = 0.1f;
    [SerializeField] float wordSpeedIncrement = 0f;
    [SerializeField] float difficultyDuration = 30f;
    [SerializeField] int wordDifficultyLevel = 3;
    [SerializeField] int maxWordDifficultyLevel = 7;

    private List<string> radialLetterSets = new List<string>
{
    "abeort",
    "aefnos",
    "aeglsx",
    "eflort",
    "ehirsu",
    "einrst",
    "acelot",
    "adoprt",
    "acirst"
};

    private int currentSetIndex = 0;
    private List<string> runtimeRadialLetterSets = new List<string>();
    private float runtimeWordFallSpeed;
    private float runtimeWordFallDelay;
    private float runtimeLetterFallSpeed;
    private float runtimeLetterFallDelay;
    private float runtimeNumberFallSpeed;
    private float runtimeNumberFallDelay;
    private float runtimeWordDelayDecrement;
    private float runtimeLetterDelayDecrement;
    private float runtimeWordSpeedIncrement;
    private float runtimeDifficultyDuration;
    private float runtimeDangerThresholdPercent;
    private int runtimeWordDifficultyLevel;
    private int runtimeMaxWordDifficultyLevel;
    private bool hasAppliedLevelConfig;

    // GAME TIMER
    float startTime;

    // Debug
    public TMP_Text fillPercentText;

    [SerializeField] private float dangerThresholdPercent = 80f;
    public float GetDangerThresholdPercent() => hasAppliedLevelConfig ? runtimeDangerThresholdPercent : dangerThresholdPercent;

    #endregion

    void Awake()
    {
        ApplyLevelConfig();
    }

    void Start()
    {
        Initiatiate();
    }

    void Initiatiate()
    {
        Log("Initiatiate");
        startTime = Time.time;
        LoadSettings();
        string currentSet = GetCurrentRadialLetterSet();
        FindFirstObjectByType<RMF_RadialMenu>()?.SetLetters(currentSet);

        StartCoroutine(GameOverCheckLoop());
        StartCoroutine(DifficultyLoop());
    }

    void Update()
    {
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
            yield return new WaitForSeconds(runtimeDifficultyDuration);
            IncreaseDifficulty();
        }
    }

    void IncreaseDifficulty()
    {
        if (runtimeWordFallDelay > 2) runtimeWordFallDelay -= runtimeWordDelayDecrement;
        if (runtimeLetterFallDelay > 2) runtimeLetterFallDelay -= runtimeLetterDelayDecrement;
        if (runtimeWordFallSpeed < 0.8f) runtimeWordFallSpeed += runtimeWordSpeedIncrement;
        if (runtimeWordDifficultyLevel < runtimeMaxWordDifficultyLevel) runtimeWordDifficultyLevel++;
    }

    IEnumerator GameOverCheckLoop()
    {
        while (!isGameOver)
        {
            float fillPercent = GetPercentageFilled();
            fillPercentText.text = "Fill % = " + fillPercent.ToString();

            yield return new WaitForSeconds(0.5f);
            if (fillPercent > runtimeDangerThresholdPercent)
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

    public void RestartGame()
    {
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

    void ApplyLevelConfig()
    {
        if (levelConfig == null)
        {
            LogError("No LevelConfig assigned. Using legacy GameManager fallback tuning.");
            ApplyLegacyFallbackConfig();
            return;
        }

        runtimeWordFallSpeed = levelConfig.WordFallSpeed;
        runtimeWordFallDelay = levelConfig.WordFallDelay;
        runtimeLetterFallSpeed = levelConfig.LetterFallSpeed;
        runtimeLetterFallDelay = levelConfig.LetterFallDelay;
        runtimeNumberFallSpeed = levelConfig.NumberFallSpeed;
        runtimeNumberFallDelay = levelConfig.NumberFallDelay;
        runtimeWordDelayDecrement = levelConfig.WordDelayDecrement;
        runtimeLetterDelayDecrement = levelConfig.LetterDelayDecrement;
        runtimeWordSpeedIncrement = levelConfig.WordSpeedIncrement;
        runtimeDifficultyDuration = levelConfig.DifficultyDuration;
        runtimeWordDifficultyLevel = levelConfig.StartingWordLength;
        runtimeMaxWordDifficultyLevel = Mathf.Max(levelConfig.StartingWordLength, levelConfig.MaxWordLength);
        runtimeDangerThresholdPercent = levelConfig.DangerThresholdPercent;

        runtimeRadialLetterSets = new List<string>(levelConfig.RadialLetterSets);
        if (runtimeRadialLetterSets.Count == 0)
        {
            LogError("Assigned LevelConfig has no radial letter sets. Using legacy radial letter fallback.");
            runtimeRadialLetterSets = new List<string>(radialLetterSets);
        }

        currentSetIndex = Mathf.Clamp(levelConfig.StartingRadialSetIndex, 0, runtimeRadialLetterSets.Count - 1);
        hasAppliedLevelConfig = true;
    }

    void ApplyLegacyFallbackConfig()
    {
        runtimeWordFallSpeed = wordFallSpeed;
        runtimeWordFallDelay = wordFallDelay;
        runtimeLetterFallSpeed = letterFallSpeed;
        runtimeLetterFallDelay = letterFallDelay;
        runtimeNumberFallSpeed = numberFallSpeed;
        runtimeNumberFallDelay = numberFallDelay;
        runtimeWordDelayDecrement = wordDelayDecrement;
        runtimeLetterDelayDecrement = letterDelayDecrement;
        runtimeWordSpeedIncrement = wordSpeedIncrement;
        runtimeDifficultyDuration = difficultyDuration;
        runtimeWordDifficultyLevel = wordDifficultyLevel;
        runtimeMaxWordDifficultyLevel = Mathf.Max(wordDifficultyLevel, maxWordDifficultyLevel);
        runtimeDangerThresholdPercent = dangerThresholdPercent;
        runtimeRadialLetterSets = new List<string>(radialLetterSets);
        currentSetIndex = Mathf.Clamp(currentSetIndex, 0, runtimeRadialLetterSets.Count - 1);
        hasAppliedLevelConfig = true;
    }


    // Public Getters
    public float GetFallSpeed(string type)
    {
        return type switch
        {
            "word" => runtimeWordFallSpeed,
            "number" => runtimeNumberFallSpeed,
            _ => runtimeLetterFallSpeed,
        };
    }

    public float GetFallDelayTime(string type)
    {
        return type switch
        {
            "word" => runtimeWordFallDelay,
            "letter" => Random.Range(2, 4) + runtimeLetterFallDelay,
            "number" => runtimeNumberFallDelay,
            _ => 1f,
        };
    }

    public int GetScore() => score;
    public int GetHighScore() => highscore;
    public bool GetIsMuted() => isMuted;
    public bool GetIsPaused() => isPaused;
    public bool GetIsGameOver() => isGameOver;
    public int GetWordDifficultyLevel() => runtimeWordDifficultyLevel;
    public string GetCurrentRadialLetterSet() => runtimeRadialLetterSets[currentSetIndex];

    public void SetRadialSetByIndex(int index)
    {
        if (index >= 0 && index < runtimeRadialLetterSets.Count)
            currentSetIndex = index;
    }

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
