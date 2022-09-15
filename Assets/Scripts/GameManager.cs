using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class GameManager : MonoBehaviour
{

    // FEEDBACKS
    //[SerializeField] private MMFeedbacks highScoreFeedback;

    //public int levelNumber = 0;
    public int score;
    //private int lives = 100;
    //[SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    //[SerializeField] private TMP_Text livesText;
    [SerializeField] private WordSpawner wordSpawner;
    [SerializeField] private LevelManager levelManager;

    // Global variables
    [Range(0, 20)]
    [SerializeField] private float wordFallSpeed = 1;
    [Range(0, 20)]
    [SerializeField] private float letterFallSpeed = 1;
    [Range(0, 20)]
    [SerializeField] private float wordFallDelay = 1;
    [Range(0, 20)]
    [SerializeField] private float letterFallDelay = 1;

    [SerializeField] float FALL_TIMER = 5;

    // Bools
    bool isGameOver = false;

    void Start()
    {
        //highScoreFeedback = GameObject.Find("HighScoreFeedback").GetComponent<MMFeedbacks>();
        //if (highScoreFeedback == null) Debug.LogError("ERROR: cant find High Score");
        StartCoroutine(CheckForGameOver());
        StartCoroutine(FallDelayTimers());
    }
    /// <summary>
    /// Update
    /// </summary>
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
        if (score > 10)
        {
            if (wordFallDelay > 2)
            {
                wordFallDelay -= 0.1f;
                if (wordFallDelay < 0) wordFallDelay = 0;
            }

        }

        StartCoroutine(FallDelayTimers());
    }
    /// <summary>
    /// CheckForGameOver
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForGameOver()
    {
        yield return new WaitForSeconds(0.5f);

        if (GetPercentageFilled()>99) {
            StopCoroutine(CheckForGameOver());
            StartCoroutine(GameOver());
        }
        StartCoroutine(CheckForGameOver());
    }

    private float GetPercentageFilled()
    {
        // LETTER HIGH POINT
        GameObject[] letters = GameObject.FindGameObjectsWithTag("ExplodedLetter");
        //print("Num exploded letters " + letters.Length);
        float highPoint = 0;
        float percentageFilled = 0;
        foreach (GameObject go in letters)
        {
            if (go.transform.position.y > highPoint)
                highPoint = go.transform.position.y;
        }

        if (letters.Length > 0)
        {
            // SCREEN TOP EDGE
            Vector2 topPoint = new Vector2(0, 1);
            Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);
            float screenTop = topEdge.y;

            // Get ground high point
            float groundY = GameObject.Find("Ground").GetComponent<Transform>().position.y;
            groundY += 0.25f;

            float height = screenTop - groundY;
            float heapHeight = highPoint - groundY;

            percentageFilled = (heapHeight / height) * 100;
        }
        return percentageFilled;
    }

    /// <summary>
    /// GameOver
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// UpdateUI
    /// </summary>
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
    /// <summary>
    /// GetFallSpeed
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public float GetFallDelayTime(string _type)
    {
        switch (_type)
        {
            case "word":
                float r = Random.Range(0, 5);

                r += wordFallDelay;
                print("Word delay = " + r);
                return r;
                break;
            case "letter":
                float rand = Random.Range(5, 10)+ letterFallDelay;
                print("Letter delay = " + rand);
                return rand;
                break;
            default:
                return 1f;
                break;
        }
    }



}
