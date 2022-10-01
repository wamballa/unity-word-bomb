using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.Events;


public class Word3D : MonoBehaviour {

    // 3D LETTERS
    [SerializeField] GameObject[] lettersPF;
    [SerializeField] List<GameObject> wordPF = new List<GameObject>();
    [SerializeField] private GameObject letterCollisionPF;

    // COLLISION STUFF
    private CollisionHandler3D collisionHandler;
    private BoxCollider boxCollider;

    // GLOBAL STUFF
    private GameManager gameManager;

    // WORD STUFF
    private string _word;
    private int typeIndex;
    const float CHAR_WIDTH = 0.5f;
    const float DESTROY_WORD_TIMER = 1f;
    const float HEIGHT_FROM_TOP = 0.1f;

    // Bools
    private Vector3 crashPos;
    private float fallSpeed;
    bool isOffScreen;
    bool hasCollided;
    bool hasBeenTyped;
    bool wordHasExploded;


    // Orientation stuff
    bool isFacingLeft;
    [SerializeField] LayerMask layerMask;
    private const float SPAWN_HEIGHT = 5.5f;

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

    PlayAudio playAudio;


    private void Start()
    {
        Initialise();
    }


    void Initialise()
    {
        // Get dependencies
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null) print("ERROR: no game manager found");

        playAudio = transform.GetComponent<PlayAudio>();

        audioSource = gameObject.GetComponent<AudioSource>();

        //layerMask = transform.GetComponent<LayerMask>();

        // Get feedbacks
        cameraShakeFeedback = GameObject.Find("CameraShakeFeedback").GetComponent<MMFeedbacks>();

        boxCollider = transform.GetComponent<BoxCollider>();
        if (boxCollider == null) print("ERROR: no box collider");

        collisionHandler = transform.GetComponent<CollisionHandler3D>();
        if (collisionHandler == null) print("ERROR: no collision handler");

        SetRandomWord();
        SetupWord();
        SetBoxColliderWidth();
        SetStartPosition();
        SetFallSpeed();
        //SetLetter();




        //SetAudio();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckIfCollided();
        HandleWordCollision();
        ////SpeakWord();
        //CheckIfOffScreen();
    }

    #region SETTERS


    void SetRandomWord()
    {

        _word = WordGenerator.GetRandomWord(gameManager.GetWordDifficultyLevel());
        transform.name = _word;

    }


    private void SetupWord()
    {
        float wordLength = _word.Length * CHAR_WIDTH;
        float midPoint = wordLength / 2;
        float startX = transform.position.x -midPoint + CHAR_WIDTH/2;
        Vector3 pos = new Vector3(startX, transform.position.y, transform.position.z);

        for (int i=0; i < _word.Length; i++)
        {
            wordPF.Add( Get3DLetterPrefab(i));

            GameObject go = Instantiate( Get3DLetterPrefab(i), transform, false);

            go.transform.position = pos;
            pos = new Vector3(pos.x + CHAR_WIDTH, pos.y, pos.z);
        }

        // Rotate to face correct direction
        if (isFacingLeft)
        {
            transform.Rotate(0f, 45f, 0f);
            foreach (Transform childTransform in transform)
            {
                childTransform.gameObject.layer = LayerMask.NameToLayer("FacingLeft");
            }
            
        }
        else
        {
            transform.Rotate(0f, -45f, 0f);
            foreach (Transform childTransform in transform)
            {
                childTransform.gameObject.layer = LayerMask.NameToLayer("FacingRight");
            }

        }

    }


    public void HandleWordCollision()
    {
        if (!hasCollided) return;
        if (!canExplode) return;

        canExplode = false;

        float wordLength = _word.Length * CHAR_WIDTH;
        float midPoint = wordLength / 2;
        float startX = crashPos.x - midPoint + CHAR_WIDTH / 2;

        GameObject go;

        Quaternion rot = transform.rotation;



        //float wordMidPoint = (_word.Length / 2) * CHAR_WIDTH;
        //float xStartPos = crashPos.x - wordMidPoint;

        //for (int i = 0; i < _word.Length; i++)
        //{
        //    Vector3 newPos = new Vector3(xStartPos, crashPos.y + 0.0f, crashPos.z);
        //    go = Instantiate(letterCollisionPF, newPos, Quaternion.identity);
        //    go.GetComponentInChildren<TMP_Text>().text = _word[i].ToString();
        //    go.name = _word[i].ToString();
        //    xStartPos += CHAR_WIDTH;
        //}
        for (int i = 0; i < _word.Length; i++)
        {
            Vector3 newPos = new Vector3(startX, crashPos.y + 0.0f, crashPos.z);

            go = Instantiate(letterCollisionPF, crashPos, rot);
            GameObject g = Instantiate(Get3DLetterPrefab(i), go.transform );
            //go.transform.position = newPos;
            //go.transform.rotation = transform.rotation;

            //go.GetComponentInChildren<TMP_Text>().text = _word[i].ToString();
            go.name = _word[i].ToString();
            startX += CHAR_WIDTH;
        }
    }


    private void SetBoxColliderWidth()
    {
        float width = _word.Length * CHAR_WIDTH + CHAR_WIDTH;

        boxCollider.size = new Vector3(width, boxCollider.size.y, boxCollider.size.z);
    }

    private GameObject Get3DLetterPrefab(int c)
    {
        for (int i=0; i < lettersPF.Length; i++)
        {
            //print ("_word lettter to match "+_word[c].ToString()+ ".........letter PFs "+ lettersPF[i].name);
            if (_word[c].ToString() == lettersPF[i].name.ToLower())
            {
                return lettersPF[i];
            }
        }
        return null;
    }


    public void RemoveLetter(char typedLetter)
    {
        if (hasBeenTyped) return;
        if (!IsVisible()) return;

        SpawnExplosion();
        cameraShakeFeedback?.PlayFeedbacks();


        playAudio.Play();

        //SpeakLetter(typedLetter);

        _word = _word.Remove(0, 1);

        ///////////////////////////////////////////////////
        /// change color of word to red
        ///         text.color = Color.red;

        SetupWord();
        SetBoxColliderWidth();
        if (_word.Length == 0)
        {
            //hasBeenTyped = true;
            playAudio.PlayExplosion();
            //StartCoroutine(SetHasBeenTyped());
            SetHasBeenTyped();
        }
    }


    public char GetNextLetter()
    {
        return _word[typeIndex];
    }


    void SetLetter()
    {
        _char = LetterGenerator.GetRandomLetter();
    }


    private void SetAudio()
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == _word)
            {
                wordSFX = audioClips[i];
            }
        }
        // DEBUG
        if (wordSFX == null) wordSFX = audioClips[0];
    }


    public void SetDirectionFacing (bool b)
    {
        isFacingLeft = b;
    }



    public void SetFallSpeed()
    {
        fallSpeed = gameManager.GetFallSpeed("word");
    }


    void SetStartPosition()
    {
        // Y POSITION


        // X POSITION

        float halfLength = (_word.Length * (CHAR_WIDTH / 2)) +0.1f;

        Vector2 leftPoint = new Vector2(0, 0);
        Vector2 rightPoint = new Vector2(1, 0);
        Vector2 leftEdge = Camera.main.ViewportToWorldPoint(leftPoint);
        Vector2 rightEdge = Camera.main.ViewportToWorldPoint(rightPoint);

        float leftRange = leftEdge.x + halfLength;
        float rightRange = rightEdge.x - halfLength;

        //print("Left Range = " + leftRange);
        //print("Right Range = " + rightRange);

        Vector3 randomPosition = new Vector3(
            Random.Range(leftRange, rightRange),
            SPAWN_HEIGHT, transform.position.z
            );
        transform.position = randomPosition;
    }


    public void SetWordHasBeenExploded()
    {
        wordHasExploded = true;
    }





    void SetHasBeenTyped()
    {
        hasBeenTyped = true;
    }

    #endregion


    #region GETTERS








    public string GetWord()
    {
        return _word;
    }


    #endregion


    void CheckIfOffScreen()
    {
        if (isOffScreen) return;
        if (transform.position.y < -5.5f) isOffScreen = true;
    }


    void HandleMovement()
    {
        if (hasCollided) return;
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    }


    void CheckIfCollided()
    {
        if (hasCollided) return;
        hasCollided = collisionHandler.HasCrashed();
        crashPos = transform.position;
    }





    public bool HasWordBeenTyped()
    {
        return hasBeenTyped;
    }




    private float GetBoxColliderWidth()
    {
        //print("WIDTH = " + boxCollider.size.x);
        return boxCollider.size.x;
    }


    public bool GetWordHasBeenExploded()
    {
        return wordHasExploded;
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
        return hasCollided;
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


    private bool IsVisible()
    {
        Vector2 topPoint = new Vector2(0, 1);
        Vector2 topEdge = Camera.main.ViewportToWorldPoint(topPoint);

        float topRange = topEdge.y - HEIGHT_FROM_TOP;
        return transform.position.y < topRange;
    }



}
