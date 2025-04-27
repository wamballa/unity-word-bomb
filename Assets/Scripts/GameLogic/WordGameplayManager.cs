using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordGameplayManager : MonoBehaviour, IGameplayInputReceiver
{
    private GameManager gameManager;
    public List<GameObject> words = new List<GameObject>();
    public List<GameObject> numbers = new List<GameObject>();
    private List<GameObject> letters = new List<GameObject>();

    // Booleans for state
    private bool hasActiveWord;
    private GameObject activeWord;
    public GameObject explodingParticle;

    void Awake()
    {
        InputRouter.Receiver = this;
    }

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null) { Debug.LogError("ERROR: No Game Manager Found!"); }
    }

    private void Update()
    {
        RemoveItemWhenNotNeeded();
    }

    void OnEnable()
    {
        RadialSwipeDrawer.OnRadialPointerUp += HandleOnRadialPointerUp;
    }

    void OnDisable()
    {
        RadialSwipeDrawer.OnRadialPointerUp -= HandleOnRadialPointerUp;
    }


    public void AddWordAsPrefab(GameObject _word)
    {
        if (_word == null) Debug.LogError("Word is null");
        words.Add(_word);
    }


    public void AddNumber(GameObject _number)
    {
        numbers.Add(_number);
    }


    public void TypeLetter(char typedLetter)
    {
        typedLetter = char.ToLower(typedLetter);

        // Check if there is an active word
        if (hasActiveWord && activeWord != null)
        {
            if (activeWord == null) return;

            // If the typed letter matches the next letter in the active word, remove the letter from the word
            var fw = activeWord.GetComponent<FallingWord>();
            if (fw == null) return;

            if (fw.GetNextLetter() == typedLetter)
            {
                fw.OnLetterTyped(typedLetter);
            }

            // Check if word is complete after typing
            if (fw.state == FallingWord.FallingWordState.Typed || fw.state == FallingWord.FallingWordState.Crashed)
            {
                hasActiveWord = false;
                activeWord = null;
            }
            return;
        }

        // If there is no active word, find a word that starts with the typed letter and set it as active
        foreach (GameObject wordObj in words)
        {
            if (wordObj == null) continue;

            var fw = wordObj.GetComponent<FallingWord>();
            if (fw == null || fw.state != FallingWord.FallingWordState.Falling) continue;

            if (fw.GetNextLetter() == typedLetter)
            {
                activeWord = wordObj;
                hasActiveWord = true;
                fw.OnLetterTyped(typedLetter);
                break;
            }
        }
    }

    public void HandleOnRadialPointerUp()
    {
        if (hasActiveWord && activeWord != null)
        {
            if (activeWord == null) return;
            Debug.Log("[WGM] Reset Activated Word");
            var fw = activeWord.GetComponent<FallingWord>();
            if (fw == null) return;
            fw.OnResetWord();
            hasActiveWord = false;
            activeWord = null;
        }
    }

    public void TypeNumber(int typedNumber)
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            NumberController number = numbers[i].GetComponent<NumberController>();
            int fallingNumber = number.GetNumber();

            if (fallingNumber == typedNumber)
            {
                //Debug.Log(Time.time + "Number matched: falling/typed   " + fallingNumber + "/" + typedNumber);
                number.MarkAsTyped();
                break;
            }
        }
    }


    public void RemoveItemWhenNotNeeded()
    {
        // Remove words that have crashed, been typed, exploded, or gone off-screen
        for (int i = 0; i < words.Count; i++)
        {
            GameObject wordObj = words[i];
            if (wordObj == null) continue;

            FallingWord word = wordObj.GetComponent<FallingWord>();

            if (word == null) continue;

            switch (word.state)
            {
                case FallingWord.FallingWordState.Crashed:
                    // Optional: lose life or trigger penalty
                    break;
                case FallingWord.FallingWordState.Typed:
                    Debug.Log("[WordGameplayManager] State = Typed");

                    FeedbackManager.Instance.PlayWordExplode( word);
                    FeedbackManager.Instance.PlayCameraShake();

                    break;
                case FallingWord.FallingWordState.Exploded:
                    Debug.Log("[WordGameplayManager] State = Exploded");
                    Destroy(wordObj, 0.0f);
                    words.RemoveAt(i);
                    hasActiveWord = false;
                    IncreaseScore();
                    GameObject go = Instantiate (explodingParticle, transform);
                    Destroy (go, 2f);
                    break;
                case FallingWord.FallingWordState.Inactive:
                    Destroy(wordObj);
                    words.RemoveAt(i);
                    hasActiveWord = false;
                    break;
            }
        }

        // Remove numbers that have crashed or gone off-screen
        for (int i = 0; i < numbers.Count; i++)
        {
            var number = numbers[i].GetComponent<NumberController>();
            if (number == null || number.IsRemovable()) // add IsRemovable() in NumberController
            {
                Destroy(numbers[i].gameObject);
                numbers.RemoveAt(i);
            }
        }
    }

    void IncreaseScore()
    {
        gameManager.AddScore(1);
    }
}
