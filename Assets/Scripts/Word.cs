using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Word : MonoBehaviour {
    
    // GLOBAL STUFF
    private GameManager gameManager;
    // WORD STUFF
    private string _word;
    private int typeIndex;
    public TMP_Text text;
    public CollisionHandler collisionHandler;
    public GameObject explodingPF;
    private Vector2 crashPos;
    private float fallSpeed;
    bool isOffScreen;
    bool hasCrashed;
    bool hasBeenTyped;
    bool wordHasExploded;
    float boxColliderWidth;
    float CHAR_WIDTH = 0.5f;
    float FORCE = 2f;

    // LETTER STUFF
    private string _char;

    // SOUNDS STUFF
    public AudioClip[] audioClips;
    private AudioClip wordSFX;
    bool hasSpoken;
    AudioSource audioSource;


    private void Start()
    {
        _word = WordGenerator.GetRandomWord();
        transform.name = _word;

        audioSource = gameObject.GetComponent < AudioSource >();

        _char = LetterGenerator.GetRandomLetter();

        BoxCollider2D bc = transform.GetComponentInChildren<BoxCollider2D>();

        bc.size = new Vector2(_word.Length * CHAR_WIDTH, bc.size.y);

        SetXPosition();
        SetText();

        // Get fall speed
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null) print("ERROR: no game manager found");
        SetFallSpeeds();

        SetAudio();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckIfCrashed();
        HandleExplosion();
        SpeakWord();
    }
    private void SpeakWord()
    {
        if (hasSpoken) return;

        if (IsVisible())
        {
            //print(transform.name + " is visible");
            audioSource.PlayOneShot(wordSFX);
            hasSpoken = true;
        }
    }
    private void SetAudio()
    {
        for (int i=0; i< audioClips.Length; i++)
        {
            if (audioClips[i].name == _word)
            {
                wordSFX = audioClips[i];
            }
        }
        // DEBUG
        if (wordSFX == null) wordSFX = audioClips[0];
    }
    private bool IsVisible()
    {
        Vector2 topPoint = new Vector2(0, 1);
        Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);

        float topRange = topEdge.y - 0.5f;
        return transform.position.y < topRange ;
    }
    public void SetFallSpeeds()
    {
        fallSpeed = gameManager.GetFallSpeed("word");
    }
    void SetXPosition()
    {
        float halfLength = (_word.Length * CHAR_WIDTH)/2;

        Vector2 leftPoint = new Vector2(0, 0);
        Vector2 rightPoint = new Vector2(1, 0);
        Vector2 leftEdge = Camera.main.ViewportToWorldPoint(leftPoint);
        Vector2 rightEdge = Camera.main.ViewportToWorldPoint(rightPoint);

        float leftRange = leftEdge.x + halfLength;
        float rightRange = rightEdge.x - halfLength;

        //print("Left Range = " + leftRange);
        //print("Right Range = " + rightRange);

        Vector3 randomPosition = new Vector3(Random.Range(leftRange, rightRange), 7f);
        transform.position = randomPosition;
    }
    void HandleMovement()
    {
        if (hasCrashed) return;
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    }
    public void HandleExplosion()
    {
        if (!hasCrashed) return;

        GameObject go;

        float wordMidPoint = (_word.Length / 2) * 0.5f;
        float xStartPos = crashPos.x - wordMidPoint;


        for (int i=0; i < _word.Length; i++)
        {
            //print(i + " " + _word[i]);

            //Vector2 newPos = new Vector2(xStartPos, crashPos.y + 0.5f);
            Vector2 newPos = new Vector2(xStartPos, crashPos.y + 0.0f);
            go = Instantiate(explodingPF, newPos, Quaternion.identity);
            go.GetComponentInChildren<TMP_Text>().text = _word[i].ToString();
            //go.GetComponentInChildren<Rigidbody2D>().AddForce(transform.up * FORCE);
            xStartPos += CHAR_WIDTH;
        }



    }
    void CheckIfCrashed()
    {
        if (hasCrashed) return;
        hasCrashed = collisionHandler.HasCrashed();
        crashPos = transform.position;
    }

    public bool HasWordBeenTyped()
    {
        bool wordTyped = (typeIndex >= _word.Length);
        return wordTyped;
    }

    public void SetWordHasBeenExploded()
    {
        wordHasExploded = true;
    }
    public bool GetWordHasBeenExploded()
    {
        return wordHasExploded;
    }

    public char GetNextLetter()
    {
        return _word[typeIndex];
    }

    public void TypeLetter()
    {
        typeIndex++;
        RemoveLetter();
    }

    public void RemoveLetter()
    {
        //text.text = text.text.Remove(0, 1);
        _word = _word.Remove(0, 1);
        text.color = Color.red;
        SetText();
    }

    public bool IsOffScreen()
    {
        return isOffScreen;
    }
    public bool HasCrashed()
    {
        return hasCrashed;
    }
    public string GetWord()
    {
        return _word;
    }
    private void SetText()
    {
        text.text = _word;
    }
}
