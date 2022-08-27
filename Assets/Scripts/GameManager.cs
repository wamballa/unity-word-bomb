using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int levelNumber = 0;
    public int score;
    private int lives = 100;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;

    // Global variables
    [Range(0, 10)]
    [SerializeField] private float wordFallSpeed = 1;
    [Range(0, 10)]
    [SerializeField] private float letterFallSpeed = 1;
    [Range(0, 10)]
    [SerializeField] private float wordFallDelay = 1;
    [Range(0, 10)]
    [SerializeField] private float letterFallDelay = 1;

    // Bools
    bool isGameOver = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
   


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckIfFull());
        // Set initial values
        //if (wordFallSpeed == 0) wordFallSpeed = 2f;
        //if (letterFallSpeed == 0) letterFallSpeed = 0.5f;

        //if (wordFallDelay == 0) wordFallDelay = 1f;
        //if (letterFallDelay ==0 ) letterFallDelay = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        CheckLives();

    }
    IEnumerator CheckIfFull()
    {
        yield return new WaitForSeconds(1f);
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

        //print("Top Edge = " + topRange);

        if (highPoint > topRange) {
            StartCoroutine(GameOver());
        }

        //print("High Point " + highPoint);
        StartCoroutine(CheckIfFull());

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
    //public float GetFallDelay()
    //{
    //    return 1f;
    //}

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        print("GAME OVER");
    }
    private void UpdateUI()
    {
        levelText.text = levelNumber.ToString();
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
    }
    void CheckLives()
    {
        if (!isGameOver)
        {
            if (lives == 0)
            {
                isGameOver = true;
                CheckLives();
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public void NextScene()
    {
        levelNumber++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void GetLevelConfig(out int wordCount, out float wordDelay, out bool canDropWord, out int letterCount, out float letterDelay, out bool canDropLetter)
    {
        print("GETLEVELCONFIG - level number = " + levelNumber);
        switch (levelNumber)
        {
            case 1:
                wordCount = 5;
                wordDelay = 3f;
                canDropWord = true;
                letterCount = 1;
                letterDelay = 1f;
                canDropLetter = false;
                break;
            case 2:
                wordCount = 2;
                wordDelay = 0.5f;
                canDropWord = true;
                letterCount = 1;
                letterDelay = 1f;
                canDropLetter = false;
                break;
            default:
                //print("DEFAULT");
                wordCount = 2;
                wordDelay = 10f;
                canDropWord = true;
                letterCount = 1;
                letterDelay = 1f;
                canDropLetter = false;
                break;
        }


    }

    public void SetScore(int i)
    {
        score = score + i;
    }
    public int GetScore()
    {
        return score;
    }
    public void SetLives(int i)
    {
        lives = lives + i;
    }
    public int GetLives()
    {
        return lives;
    }
}
