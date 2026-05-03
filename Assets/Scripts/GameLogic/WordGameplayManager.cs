using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordGameplayManager : MonoBehaviour, IGameplayInputReceiver
{
    [SerializeField] private GameManager gameManager;
    public List<GameObject> words = new List<GameObject>();
    public List<GameObject> numbers = new List<GameObject>();
    private List<GameObject> letters = new List<GameObject>();

    // Booleans for state
    private bool hasActiveWord;
    private GameObject activeWord;
    public GameObject explodingParticle;

    void Awake() => InputRouter.Receiver = this;

    void OnEnable()
    {
        RadialSwipeDrawer.OnRadialPointerUp += HandleOnRadialPointerUp;
    }

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        if (gameManager == null)
        {
            Debug.LogError("ERROR: No Game Manager Found!");
        }
    }

    private void Update()
    {
        RemoveItemWhenNotNeeded();
    }



    void OnDisable()
    {
        RadialSwipeDrawer.OnRadialPointerUp -= HandleOnRadialPointerUp;
    }

    public int WordCount => CountUnique(words);
    public int NumberCount => CountUnique(numbers);
    public bool HasActiveWord => hasActiveWord && activeWord != null;
    public FallingWord ActiveFallingWord => activeWord ? activeWord.GetComponent<FallingWord>() : null;

    public void AddWordAsPrefab(GameObject w)
    {
        if (w && !words.Contains(w)) words.Add(w);
    }

    public void AddNumber(GameObject n)
    {
        if (n && !numbers.Contains(n)) numbers.Add(n);
    }

    public bool TryGetAutoplayTarget(out FallingWord target)
    {
        target = ActiveFallingWord;
        if (target != null && target.IsFalling && target.GetNextLetter() != '\0') return true;

        target = words
            .Where(wordObj => wordObj != null)
            .Distinct()
            .Select(wordObj => wordObj.GetComponent<FallingWord>())
            .Where(word => word != null && word.IsFalling && word.GetNextLetter() != '\0')
            .OrderBy(word => word.transform.position.y)
            .FirstOrDefault();

        return target != null;
    }

    public void ClearDebugTarget()
    {
        if (HasActiveWord)
        {
            ActiveFallingWord?.OnResetWord();
        }

        hasActiveWord = false;
        activeWord = null;
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
                    Debug.Log("[WordGameplayManager] State = Typed ");

                    FeedbackManager.Instance.PlayWordExplode(word);
                    FeedbackManager.Instance.PlayCameraShake();

                    word.SetState(FallingWord.FallingWordState.Exploding);

                    break;

                case FallingWord.FallingWordState.Exploding:
                    Debug.Log("[WordGameplayManager] State = Exploding");
                    break;

                case FallingWord.FallingWordState.Exploded:
                    Debug.Log("[WordGameplayManager] State = Exploded");
                    Destroy(wordObj, 0.0f);
                    words.RemoveAt(i);
                    hasActiveWord = false;
                    IncreaseScore();
                    GameObject go = Instantiate(explodingParticle, transform);
                    Destroy(go, 2f);
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

    private static int CountUnique(List<GameObject> items)
    {
        return items
            .Where(item => item != null)
            .Distinct()
            .Count();
    }
}
