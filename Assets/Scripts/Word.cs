using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.Events;


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

    const float CHAR_WIDTH = 0.2f;
    const float DESTROY_WORD_TIMER = 1f;
    const float HEIGHT_FROM_TOP = 0.5f;
    BoxCollider2D boxCollider;

    // LETTER STUFF
    private string _char;

    // SOUNDS STUFF
    public AudioClip[] audioClips;
    private AudioClip wordSFX;
    public AudioClip[] alphabet;
    bool hasSpoken;
    AudioSource audioSource;

    // EXPLOSION STUFF
    public GameObject vfxExplosion;
    bool canExplode = true;
    [SerializeField] private MMFeedbacks cameraShakeFeedback;


    private void Start()
    {
        _word = WordGenerator.GetRandomWord();
        transform.name = _word;

        audioSource = gameObject.GetComponent < AudioSource >();

        _char = LetterGenerator.GetRandomLetter();

        boxCollider = transform.GetComponentInChildren<BoxCollider2D>();

        SetStartXPositioon();
        SetText();
        SetBoxColliderWidth();

        // Get fall speed
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null) print("ERROR: no game manager found");

        // Get feedbacks
        cameraShakeFeedback = GameObject.Find("CameraShakeFeedback").GetComponent<MMFeedbacks>();


        SetFallSpeed();

        SetAudio();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckIfCrashed();
        HandleExplosion();
        SpeakWord();
    }
    /// <summary>
    /// SPEAKS WORD
    /// </summary>
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
    private void SetBoxColliderWidth()
    {
        boxCollider.size = new Vector2(_word.Length * CHAR_WIDTH, boxCollider.size.y);
    }
    private float GetBoxColliderWidth()
    {
        //print("WIDTH = " + boxCollider.size.x);
        return boxCollider.size.x;
    }
    /// <summary>
    /// Can the word be seen?
    /// </summary>
    /// <returns></returns>
    private bool IsVisible()
    {
        Vector2 topPoint = new Vector2(0, 1);
        Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);

        float topRange = topEdge.y - HEIGHT_FROM_TOP;
        return transform.position.y < topRange ;
    }
    public void SetFallSpeed()
    {
        fallSpeed = gameManager.GetFallSpeed("word");
    }
    /// <summary>
    /// 
    /// </summary>
    void SetStartXPositioon()
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
    /// <summary>
    /// 
    /// </summary>
    void HandleMovement()
    {
        if (hasCrashed) return;
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    }
    /// <summary>
    /// HandleExplosion()
    /// </summary>
    public void HandleExplosion()
    {
        if (!hasCrashed) return;
        if (!canExplode) return;

        canExplode = false;
        GameObject go;

        float wordMidPoint = (_word.Length / 2) * CHAR_WIDTH;
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
    /// <summary>
    /// Check if word has crashed into stuff
    /// </summary>
    void CheckIfCrashed()
    {
        if (hasCrashed) return;
        hasCrashed = collisionHandler.HasCrashed();
        crashPos = transform.position;
    }

    public bool HasWordBeenTyped()
    {
        return hasBeenTyped;
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
    /// <summary>
    /// Remove a letter from the word
    /// If no letters left then start a coroutine
    /// so WordManager doesnt kill the word before
    /// the last letter spoken
    /// </summary>
    /// <param name="typedLetter"></param>
    public void RemoveLetter(char typedLetter)
    {
        if (hasBeenTyped) return;
        if (!IsVisible()) return;

        SpawnExplosion();
        //print("cam shake = " + cameraShakeFeedback);
        cameraShakeFeedback?.PlayFeedbacks();
        SpeakLetter(typedLetter); 
        _word = _word.Remove(0, 1);
        text.color = Color.red;
        SetText();
        SetBoxColliderWidth();
        if (_word.Length == 0)
        {
            //hasBeenTyped = true;
            StartCoroutine(SetHasBeenTyped());
        }
    }

    IEnumerator SetHasBeenTyped()
    {
        yield return new WaitForSeconds(0.5f);
        hasBeenTyped = true;
    }
    void SpeakLetter(char typedLetter)
    {
        int index = (int)typedLetter % 32 -1;
        //print("Letter " + typedLetter + " is index " + index);
        audioSource.PlayOneShot(alphabet[index]);
    }
    private void SpawnExplosion()
    {
        //float wordMidPoint = (_word.Length / 2) * CHAR_WIDTH;
        ////print("OFFSET = " + wordMidPoint);

        //float xStartPos = transform.position.x - wordMidPoint;
        ////print("WORD START = " + xStartPos);

        float halfLength = (_word.Length * CHAR_WIDTH) / 2;

        float leftEdge = transform.position.x - (GetBoxColliderWidth() / 2);
        leftEdge = transform.position.x - halfLength;
        Vector2 newPos = new Vector2(leftEdge, transform.position.y - 0.25f) ;
        GameObject go = Instantiate(vfxExplosion, newPos, Quaternion.identity);
        Destroy(go, 1f);
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
