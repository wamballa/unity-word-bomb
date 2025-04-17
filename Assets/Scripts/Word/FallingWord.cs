// state machine + high-level control

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.Events;


public class FallingWord : MonoBehaviour {

    public enum FallingWordState
    {
        Falling,
        Typed,
        Crashed,
        Exploded,
        Inactive
    }
    public FallingWordState state = FallingWordState.Falling;

    private float fallSpeed;
    private GameManager gameManager;

    private WordVisual visual;
    private WordTyping typing;
    private WordAudio audioPlayer;

    public char GetNextLetter() => typing.GetNextLetter();

    // WORD STUFF
    //private string _word;
    //private int typeIndex;
    //public TMP_Text text;
    //private CollisionHandler collisionHandler;
    //public GameObject letterPrefab;
    //private Vector2 crashPos;

    //bool isOffScreen;
    //bool hasCrashed;
    //bool hasBeenTyped;
    //bool isActiveWord;
    //bool canDestroyWord;
    ////bool hasExploded;

    //const float CHAR_WIDTH = 0.3f;
    //const float DESTROY_WORD_TIMER = 1f;
    //const float HEIGHT_FROM_TOP = 0.1f;
    //BoxCollider2D boxCollider;

    //// Number STUFF
    //private int _int;

    //// SOUNDS STUFF
    //public AudioClip[] audioClips;
    //private AudioClip wordSFX;
    //public AudioClip[] alphabet;
    //bool hasSpoken;
    //AudioSource audioSource;

    //// EXPLOSION STUFF
    //public GameObject vfxExplosion;
    //bool canExplode = false;

    //AudioPlayer playAudio;

    void Awake()
    {
        visual = GetComponent<WordVisual>();
        typing = GetComponent<WordTyping>();
        audioPlayer = GetComponent<WordAudio>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        fallSpeed = gameManager.GetFallSpeed("word");
        typing.SetTargetWord(WordGenerator.GetRandomWord(gameManager.GetWordDifficultyLevel()));
        visual.SetText(typing.GetCurrentWord());
        //Initialise();
    }

    void Update()
    {
        if (state == FallingWordState.Falling)
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

        SetState( FallingWordState.Crashed);
        visual.ExplodeToLetters();
        Destroy(gameObject, 0.0f);
    }

    public void OnLetterTyped(char typedLetter)
    {
        // add this
        if (typing.TryTypeLetter(typedLetter))
        {
            visual.RevealNextLetter(); // show red highlight
            audioPlayer.PlaySuccess(); // play feedback

            if (typing.IsComplete())
            {
                SetState(FallingWordState.Typed);
            }
        }
        //typing.OnLetterTyped(typedLetter); // renamed from RemoveLetter for clarity
    }

    public void SetState(FallingWordState newState)
    {
        state = newState;
    }


    ////private void FixedUpdate()
    ////{
    ////    HandleMovement();
    ////    CheckIfCrashed();
    ////    HandleCrash();
    ////    //HandleExplode();
    ////    //SpeakWord();
    ////    CheckIfOffScreen();
    ////}


    ////void Initialise()
    ////{
    ////    isActiveWord = false;

    ////    // Get fall speed
    ////    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    ////    if (gameManager == null) print("ERROR: no game manager found");
    ////    playAudio = transform.GetComponent<AudioPlayer>();
    ////    audioSource = gameObject.GetComponent<AudioSource>();

    ////    boxCollider = transform.GetComponent<BoxCollider2D>();
    ////    if (boxCollider == null) print("ERROR: no box collider");

    ////    collisionHandler = transform.GetComponent<CollisionHandler>();
    ////    if (collisionHandler == null) print("ERROR: no collision handler");

    ////    SetWordName();
    ////    SetNumber();
    ////    SetStartXPositioon();
    ////    SetText();
    ////    SetBoxColliderWidth();
    ////    SetFallSpeed();
    ////    //SetAudio();
    ////}


    //#region SETTERS

    //void SetWordName()
    //{

    //    _word = WordGenerator.GetRandomWord(gameManager.GetWordDifficultyLevel());
    //    transform.name = _word;
    //}

    //public void SetIsActiveWord()
    //{
    //    isActiveWord = true;
    //}

    //public bool  GetIsActiveWord()
    //{
    //    return isActiveWord;
    //}


    //void SetNumber()
    //{
    //    _int = NumberGenerator.GetRandomNumber();
    //}

    //void SetStartXPositioon()
    //{
    //    float halfLength = (_word.Length * (CHAR_WIDTH + 0.1f)) / 2;

    //    Vector2 leftPoint = new Vector2(0, 0);
    //    Vector2 rightPoint = new Vector2(1, 0);
    //    Vector2 leftEdge = Camera.main.ViewportToWorldPoint(leftPoint);
    //    Vector2 rightEdge = Camera.main.ViewportToWorldPoint(rightPoint);

    //    float leftRange = leftEdge.x + halfLength;
    //    float rightRange = rightEdge.x - halfLength;

    //    //print("Left Range = " + leftRange);
    //    //print("Right Range = " + rightRange);

    //    Vector3 randomPosition = new Vector3(Random.Range(leftRange, rightRange), transform.position.y);
    //    transform.position = randomPosition;
    //}



    //private void SetBoxColliderWidth()
    //{
    //    boxCollider.size = new Vector2(_word.Length * CHAR_WIDTH, boxCollider.size.y);
    //}


    //public void SetFallSpeed()
    //{
    //    fallSpeed = gameManager.GetFallSpeed("word");
    //}

    //public void SetWordCanExplode()
    //{
    //    canExplode = true;
    //    //hasCrashed = false;
    //}

    //public void SetHasCrashed()
    //{
    //    hasCrashed = true;
    //}

    //private void SetText()
    //{
    //    text.text = _word;
    //}


    //void SetHasBeenTyped()
    //{
    //    hasBeenTyped = true;
    //}

    //#endregion
    //#region GETTERS


    //private float GetBoxColliderWidth()
    //{
    //    //print("WIDTH = " + boxCollider.size.x);
    //    return boxCollider.size.x;
    //}


    //public bool GetWordHasExploded()
    //{
    //    return canExplode;
    //}


    //public char GetNextLetter()
    //{
    //    return _word[typeIndex];
    //}


    //public string GetWord()
    //{
    //    return _word;
    //}


    //#endregion


    //void CheckIfOffScreen()
    //{
    //    if (isOffScreen) return;
    //    if (transform.position.y < -5.5f) isOffScreen = true;
    //}


    //void HandleMovement()
    //{
    //    if (hasCrashed) return;
    //    transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    //}


    //void CheckIfCrashed()
    //{
    //    if (hasCrashed) return;
    //    hasCrashed = collisionHandler.GetHasCrashed();
    //    crashPos = transform.position;
    //}

    //private void HandleExplode()
    //{
    //    if (!canExplode) return;
    //    print("Word to explode ");
    //    canExplode = true;
    //}

    //public bool GetCanDestroyWord()
    //{
    //    return canDestroyWord;
    //}


    //public void HandleCrash()
    //{
    //    // If the word has not crashed or cannot explode, exit the method
    //    if (!hasCrashed ) return;


    //    //print("Handle WORD crash triggered by can explod "+transform.name);
    //    //hasCrashed= false;
    //    // Prevent further explosions for this word
    //    canExplode = false;
    //    GameObject explosionInstance;

    //    // Calculate the starting position for the explosion
    //    float wordMidPoint = (_word.Length / 2) * CHAR_WIDTH;
    //    float xStartPos = crashPos.x - wordMidPoint;

    //    // Create an explosion effect for each letter in the word
    //    for (int i = 0; i < _word.Length; i++)
    //    {
    //        // Calculate the position for the current letter's explosion
    //        Vector2 newPos = new Vector2(xStartPos, crashPos.y);

    //        // Instantiate the explosion effect at the calculated position
    //        explosionInstance = Instantiate(letterPrefab, newPos, Quaternion.identity);

    //        //print("Explosion Instance = " + explosionInstance);


    //        //if (explosionInstance.GetComponentInChildren<TMP_Text>() != null) print("Text Child Found");

    //        // Set the text of the explosion effect to the current letter
    //        explosionInstance.GetComponentInChildren<TMP_Text>().text = _word[i].ToString();
    //        explosionInstance.name = _word[i].ToString();

    //        // Move to the next position
    //        xStartPos += CHAR_WIDTH;
    //    }

    //    canDestroyWord = true;
    //}

    //public bool HasWordBeenTyped()
    //{
    //    return hasBeenTyped;
    //}

    //public void RemoveLetter(char typedLetter)
    //{
    //    // If the word has already been typed or is not visible, exit the method
    //    if (hasBeenTyped || !IsVisible()) return;

    //    // Spawn explosion effect and play camera shake feedback
    //    SpawnCharacterExplosion();

    //    FeedbackManager.Instance.PlayCameraShake();

    //    // Play audio for removing a letter
    //    playAudio.Play();

    //    // Remove the first letter from the word
    //    _word = _word.Remove(0, 1);

    //    // Change the text color to red to indicate the letter has been typed
    //    text.color = Color.red;

    //    // Update the displayed text and adjust the collider width
    //    SetText();
    //    SetBoxColliderWidth();

    //    // If the word is completely typed, set the hasBeenTyped flag and play explosion audio
    //    if (_word.Length == 0)
    //    {
    //        playAudio.PlayExplosion();
    //        SetHasBeenTyped();
    //    }
    //}




    //private void SpawnCharacterExplosion()
    //{
    //    // Calculate the half length of the word in world units
    //    float halfLength = (_word.Length * CHAR_WIDTH) / 2;

    //    // Calculate the left edge position of the word
    //    float leftEdge = transform.position.x - halfLength;

    //    // Set the new position for the explosion slightly below the current position
    //    Vector2 newPos = new Vector2(leftEdge, transform.position.y - 0.25f);

    //    // Instantiate the explosion effect at the calculated position
    //    GameObject explosion = Instantiate(vfxExplosion, newPos, Quaternion.identity);

    //    // Destroy the explosion effect after 1 second to clean up
    //    Destroy(explosion, 1f);
    //}



    //public bool IsOffScreen()
    //{
    //    return isOffScreen;
    //}


    //public bool GetHasCrashed()
    //{
    //    return hasCrashed;
    //}

    //private bool IsVisible()
    //{
    //    // Define a point at the top of the viewport
    //    Vector2 topPoint = new Vector2(0, 1);

    //    // Convert the top viewport point to world coordinates
    //    Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);

    //    // Calculate the top range position by subtracting a constant height from the top edge
    //    float topRange = topEdge.y - HEIGHT_FROM_TOP;

    //    // Check if the object's position is below the calculated top range
    //    return transform.position.y < topRange;
    //}

    //#region Spoken Words
    //private void SetAudio()
    //{
    //    // Loop through the audio clips to find a match for the current word
    //    for (int i = 0; i < audioClips.Length; i++)
    //    {
    //        // If an audio clip's name matches the word, assign it to wordSFX
    //        if (audioClips[i].name == _word)
    //        {
    //            wordSFX = audioClips[i];
    //            break; // Exit the loop once the matching clip is found
    //        }
    //    }

    //    // If no matching audio clip is found, assign the first audio clip as a default
    //    if (wordSFX == null)
    //    {
    //        wordSFX = audioClips[0];
    //    }
    //}

    //private void SpeakWord()
    //{
    //    if (hasSpoken) return;

    //    if (IsVisible())
    //    {
    //        //print(transform.name + " is visible");
    //        audioSource.PlayOneShot(wordSFX);
    //        hasSpoken = true;
    //    }
    //}
    //void SpeakLetter(char typedLetter)
    //{
    //    int index = (int)typedLetter % 32 - 1;
    //    //print("Letter " + typedLetter + " is index " + index);
    //    audioSource.PlayOneShot(alphabet[index]);
    //}

    //#endregion 


}
