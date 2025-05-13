// state machine + high-level control
using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingWord : MonoBehaviour
{

    public bool debugMode = false;

    public enum FallingWordState
    {
        Falling,
        Typed,
        Crashed,
        Exploding,
        Exploded,
        Inactive
    }
    public FallingWordState state = FallingWordState.Falling;

    private float fallSpeed;
    private GameManager gameManager;

    private WordVisual visual;
    private WordTyping typing;
    private WordAudio audioPlayer;

    private RadialSwipeDrawer radialSwipeDrawer;

    public MMF_Player wordExplodeStartFeedback;
    public MMF_Player wordExplodeFinishFeedback;
    public MMF_Player wordExplodeInactiveFeedback;

    public char GetNextLetter() => typing.GetNextLetter();

    void Awake()
    {
        visual = GetComponent<WordVisual>();
        typing = GetComponent<WordTyping>();
        audioPlayer = GetComponent<WordAudio>();
        gameManager = FindFirstObjectByType<GameManager>();
        radialSwipeDrawer = FindFirstObjectByType<RadialSwipeDrawer>();
    }

    private void Start()
    {
        fallSpeed = gameManager.GetFallSpeed("word");
        var radialLoader = FindFirstObjectByType<RadialWordLoader>();
        if (!radialLoader) Debug.LogError("No radial loader");
        string currentLetterSet = gameManager.GetCurrentRadialLetterSet(); // you'll need to track this
        int desiredLength = gameManager.GetWordDifficultyLevel();

        List<string> possibleWords = radialLoader.GetWordsForLetterSet(currentLetterSet, desiredLength);
        // Debug.Log("[FallingWord] num possible words = "+possibleWords.Count);
        if (possibleWords.Count > 0)
        {
            string currentWord = possibleWords[UnityEngine.Random.Range(0, possibleWords.Count)];
            // Debug.Log("Possible words = "+word);
            typing.SetTargetWord(currentWord);
        }
        else
        {
            Debug.LogWarning("No words found for the current radial letter set and length.");
        }


        // typing.SetTargetWord(WordGenerator.GetRandomWord(gameManager.GetWordDifficultyLevel()));
        visual.SetText(typing.GetCurrentWord());
        //Initialise();
    }

    void Update()
    {
        if (state == FallingWordState.Falling && !debugMode)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // Check if off screen
            if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y - 1f)
            {
                OnCrash();
            }
        }
    }

    public void OnCrash()
    {
        if (state != FallingWordState.Falling) return;

        SetState(FallingWordState.Crashed);
        visual.ExplodeToLetters();
        Destroy(gameObject, 0.0f);
    }

    public void OnResetWord()
    {
        visual.ResetWord();
        typing.ResetWord();
    }

    public void OnLetterTyped(char typedLetter)
    {
        // add this
        if (typing.TryTypeLetter(typedLetter))
        {
            string targetWord = typing.GetCurrentWord();
            string typed = radialSwipeDrawer.GetTypedSequence;

            if (targetWord.StartsWith(typed, System.StringComparison.OrdinalIgnoreCase))
            {
                visual.RevealNextLetter(); // show red highlight
                audioPlayer.PlaySuccess(); // play feedback

                if (typed.Equals(targetWord, System.StringComparison.OrdinalIgnoreCase))
                {
                    SetState(FallingWordState.Typed);
                }
            }
        }
    }

    public void SetState(FallingWordState newState)
    {
        state = newState;
    }

    public void SetStateExploded()
    {
        SetState(FallingWordState.Exploded);
    }


}
