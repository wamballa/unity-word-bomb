using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class GameManager : MonoBehaviour
{

    #region VARIABLES

    // GAME TIMER
    float startTime;

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
    [Header("How much to increase the words spawning frequency")]
    [SerializeField] float wordDelayDecrement = 0.2f;
    [Header("How much to increase the LETTERS spawning frequency")]
    [SerializeField] float letterDelayDecrement = 0.1f;
    [Header("How much to increase the word speed")]
    [SerializeField] float wordSpeedIncrement = 0.2f;
    [Header("The duration between each difficulty levels")]
    [SerializeField] float difficultyDuration = 20f;

    int wordDifficultyLevel = 3;

    // DEBUG
    [SerializeField] TMP_Text percentText;


    #endregion

    void Start()
    {
        Initialise();
    }


    void Initialise()
    {
        startTime = Time.time;
        StartCoroutine(CheckForGameOver());
        StartCoroutine(SetDifficultyLevels());
    }


    void Update()
    {
        UpdateUI();
        CheckToReduceWordDifficulty();


        // debug
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            print("DEBUG");
            SetGroundScale();
        }
    }


    void CheckToReduceWordDifficulty()
    {
        if (GetPercentageFilled() > 80)
        {
            //print("reset world difficulty to 3");
            wordDifficultyLevel = 3;
        }
    }

    // GETTERS & SETTERS

    #region

    ///// GETTERS /////
    

    public int GetWordDifficultyLevel()
    {
        return wordDifficultyLevel;
    }


    public float GetFallSpeed(string _type)
    {
        switch (_type)
        {
            case "word":
                return wordFallSpeed;
                //break;
            case "letter":
                return letterFallSpeed;
                //break;
            default:
                return 1f;
                //break;
        }
    }


    public float GetFallDelayTime(string _type)
    {
        switch (_type)
        {
            case "word":
                return wordFallDelay;
                //break;
            case "letter":
                float rand = Random.Range(2, 4) + letterFallDelay;
                //print("Letter delay = " + rand);
                return rand;
                //break;
            default:
                return 1f;
                //break;
        }
    }


    public int GetScore()
    {
        return score;
    }


    ///// SETTERS /////


    IEnumerator SetDifficultyLevels()
    {
        yield return new WaitForSeconds(difficultyDuration);
        SetFallDelay();
        SetWordDifficulty();
        SetFallSpeed();
        SetGroundScale();
        StartCoroutine(SetDifficultyLevels());
    }


    public void SetFallSpeed()
    {
        if (wordFallSpeed < .8 ) wordFallSpeed += wordSpeedIncrement;
        //print("Fall Speed = " + wordFallSpeed);
    }


    private void SetFallDelay()
    {
        if (wordFallDelay > 2) wordFallDelay -= wordDelayDecrement;
        if (letterFallDelay > 2) letterFallDelay -= letterDelayDecrement;
        //print("////////////////////////////////");
        //print("Fall Delay = " + wordFallDelay);

    }


    private void SetWordDifficulty()
    {
        if (wordDifficultyLevel < 7) wordDifficultyLevel++;
        //print("Word Difficulty = " + wordDifficultyLevel);
    }


    private void SetGroundScale()
    {
        GameObject.Find("GroundScale").GetComponent<MMFeedbacks>().PlayFeedbacks();
    }


    public void SetScore(int i)
    {
        score = score + i;
    }


    #endregion


    //\\\ SETTERS \\\\\


    IEnumerator CheckForGameOver()
    {
        yield return new WaitForSeconds(0.5f);

        if (GetPercentageFilled() > 90)
        {
            StopCoroutine(CheckForGameOver());
            StartCoroutine(GameOver());
            StopCoroutine(SetDifficultyLevels());
        }
        StartCoroutine(CheckForGameOver());
    }


    private IEnumerator GameOver()
    {
        // 
        wordSpawner.SetSpawn(false);
        yield return new WaitForSeconds(1f);
        levelManager.SetGameOver();
    }


    private void UpdateUI()
    {
        scoreText.text = score.ToString();
    }


    public float GetPercentageFilled()
    {
        // Get LETTER HIGH POINT
        GameObject[] letters = GameObject.FindGameObjectsWithTag("ExplodedLetter");
        float letterHighPoint = -20;

        foreach (GameObject go in letters)
        {
            //print("Letter Height = " + go.transform.position.y.ToString());
            if (go.transform.position.y > letterHighPoint)
            {

                letterHighPoint = go.transform.position.y;
     
            }

        }

        //print("Number of letters = " + letters.Length);

        // Calculate screen heigh
        float percentageFilled = 0;

        if (letters.Length > 0)
        {
            // SCREEN TOP EDGE
            Vector2 topPoint = new Vector2(0, 1);
            Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);
            float screenTop = topEdge.y;

            // Get ground high point
            float groundY = GameObject.Find("Ground").GetComponent<Transform>().position.y;
            groundY += 0.25f;

            float groundTop = GameObject.Find("GroundTop").GetComponent<Transform>().position.y;


            float height = screenTop - groundY;

            float heightOfPlayingArea = screenTop - groundTop;

            //float heapHeight = highPoint - groundY;

            //float heapHeight = Mathf.Abs( letterHighPoint ) + Mathf.Abs( groundTop);

            float heapHeight = letterHighPoint - groundTop;

            //print("heightOfPlayingArea / heapHeight " + heightOfPlayingArea + " / " + heapHeight + " / " );

            //percentageFilled = (heapHeight / height) * 100;

            percentageFilled = (heapHeight / heightOfPlayingArea) * 100;
        }

        // DEBUG
        if (percentText != null)
        percentText.text = percentageFilled + " %";

        /////////////////////
        percentageFilled = 0;

        return percentageFilled;
    }





}
