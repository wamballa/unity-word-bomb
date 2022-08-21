using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int levelNumber = 0;
    private int score;
    private int lives = 100;
    [SerializeField] private TMP_Text levelText;

    // Bools
    bool isGameOver = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevelText();
        CheckLives();
    }
    private void UpdateLevelText()
    {
        levelText.text = levelNumber.ToString();
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
}
