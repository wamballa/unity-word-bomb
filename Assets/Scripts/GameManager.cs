using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class GameManager : MonoBehaviour
{

    // FEEDBACKS
    [SerializeField] private MMFeedbacks highScoreFeedback;

    //public int levelNumber = 0;
    public int score;
    //private int lives = 100;
    //[SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    //[SerializeField] private TMP_Text livesText;
    [SerializeField] private WordSpawner wordSpawner;
    [SerializeField] private LevelManager levelManager;

    // Global variables
    [Range(0, 10)]
    [SerializeField] private float wordFallSpeed = 1;
    [Range(0, 10)]
    [SerializeField] private float letterFallSpeed = 1;
    [Range(0, 10)]
    [SerializeField] private float wordFallDelay = 1;
    [Range(0, 10)]
    [SerializeField] private float letterFallDelay = 1;

    [SerializeField] float FALL_TIMER = 2;

    // Bools
    bool isGameOver = false;

    void Start()
    {
        //highScoreFeedback = GameObject.Find("HighScoreFeedback").GetComponent<MMFeedbacks>();
        //if (highScoreFeedback == null) Debug.LogError("ERROR: cant find High Score");
        StartCoroutine(CheckForGameOver());
        StartCoroutine(FallDelayTimers());
    }
    void Update()
    {
        UpdateUI();
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Period))
        {
            print("DeBUG - game over");
            StopCoroutine(CheckForGameOver());
            StartCoroutine(GameOver());
        }
    }
    /// <summary>
    /// Speed up the fall speed periodically
    /// </summary>
    /// <returns></returns>
    private IEnumerator FallDelayTimers()
    {
        yield return new WaitForSeconds(FALL_TIMER);
        wordFallDelay -= 0.5f;
        if (wordFallDelay < 0) wordFallDelay = 0;
        StartCoroutine(FallDelayTimers());
    }
    IEnumerator CheckForGameOver()
    {
        yield return new WaitForSeconds(0.5f);
        /////////////////////////////////////////////////////////////////////
        //GameObjects [] go = Fin
        GameObject[] letters = GameObject.FindGameObjectsWithTag("ExplodedLetter");
        float highPoint = 0;
        foreach(GameObject go in letters)
        {
            if (go.transform.position.y > highPoint)
                highPoint = go.transform.position.y;
        }

        Vector2 topPoint = new Vector2(0, 1);
        Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);
        float topRange = topEdge.y;

        if (highPoint > topRange) {
            StopCoroutine(CheckForGameOver());
            StartCoroutine(GameOver());
        }
        StartCoroutine(CheckForGameOver());
    }

    public float GetFallSpeed(string _type)
    {
        switch (_type)
        {
            case "word":
                return wordFallSpeed;
                break;
            case "letter":
                return letterFallSpeed;
                break;
            default:
                return 1f;
                break;
        }
    }
    public float GetFallDelayTime (string _type)
    {
        switch (_type)
        {
            case "word":
                return wordFallDelay;
                break;
            case "letter":
                return letterFallDelay;
                break;
            default:
                return 1f;
                break;
        }
    }

    private IEnumerator GameOver()
    {
        // 
        wordSpawner.SetSpawn(false);
        yield return new WaitForSeconds(1f);
        levelManager.SetGameOver();
        //SaveHighScore();
        //highScoreFeedback?.PlayFeedbacks();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SaveHighScore()
    {
        int highscore = PlayerPrefs.GetInt("highscore");
        if (score > highscore)
        {
            int delta = score - highscore;
            GameObject.Find("Delta").GetComponent<TMP_Text>().text = "+ " + delta.ToString();
            PlayerPrefs.SetInt("highscore", score);
        }
        else
        {
            GameObject.Find("Delta").GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("highscore").ToString();
        }
    }

    private void UpdateUI()
    {
        //levelText.text = levelNumber.ToString();
        scoreText.text = score.ToString();
        //livesText.text = lives.ToString();
    }
    public void SetScore(int i)
    {
        score = score + i;
    }
    public int GetScore()
    {
        return score;
    }
}
