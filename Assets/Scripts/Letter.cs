using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    // GLOBAL STUFF
    private GameManager gameManager;

    private string letter;
    private int typeIndex;
    public TMP_Text text;
    float CHAR_WIDTH = 0.5f;

    // Collision
    bool hasCrashed;
    public LetterCollisionHandler letterCollisionHandler;
    float fallSpeed;
    Vector2 crashPos;
    public GameObject explodingLetterPF;
    private Rigidbody2D rigidBody;
    bool canExplode = true;

    // Explosion
    [SerializeField] private Explosion explosionScript;


    // Start is called before the first frame update
    void Start()
    {
        // Get rigidbody
        rigidBody = transform.GetComponentInChildren<Rigidbody2D>();
        if (rigidBody == null) print("ERROR: no rigidbody");
        letterCollisionHandler = transform.GetComponentInChildren<LetterCollisionHandler>();
        if (letterCollisionHandler == null) print("ERROR; no collision handler found");

        letter = LetterGenerator.GetRandomLetter();
        transform.name = letter;

        SetXPosition();
        SetLetter();

        // Get fall speed
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null) print("ERROR: no game manager found");
        SetFallSpeeds();
    }

    public void SetFallSpeeds()
    {
        fallSpeed = gameManager.GetFallSpeed("letter");
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckIfCrashed();
        HandleCrash();
    }
    void SetXPosition()
    {
        float halfLength = (1 * CHAR_WIDTH) / 2;

        Vector2 leftPoint = new Vector2(0, 0);
        Vector2 rightPoint = new Vector2(1, 0);
        Vector2 leftEdge = Camera.main.ViewportToWorldPoint(leftPoint);
        Vector2 rightEdge = Camera.main.ViewportToWorldPoint(rightPoint);
        float leftRange = leftEdge.x + halfLength;
        float rightRange = rightEdge.x - halfLength;
        Vector3 randomPosition = new Vector3(Random.Range(leftRange, rightRange), 5.5f);
        transform.position = randomPosition;
    }
    private void SetLetter()
    { 
        text.text = letter;
    }

    public char GetLetter()
    {
        return letter[0];
    }
    public void HandleExplosion()
    {
        explosionScript.SetCanExplode();
    }
    void HandleCrash()
    {
        if (!hasCrashed) return;
        if (!canExplode) return;

        GameObject go = Instantiate(explodingLetterPF, crashPos, Quaternion.identity);
        go.GetComponentInChildren<TMP_Text>().text = letter.ToString();
        go.GetComponentInChildren<TMP_Text>().color = Color.green;
        canExplode = false;
    }
    void HandleMovement()
    {
        if (hasCrashed) return;
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
    }
    void CheckIfCrashed()
    {
        if (hasCrashed) return;
        hasCrashed = letterCollisionHandler.HasCrashed();
        crashPos = transform.position;
    }
    public bool HasCrashed()
    {
        return hasCrashed;
    }

}
