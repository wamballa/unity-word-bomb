using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberController : MonoBehaviour
{
    #region VARIABLES

    public enum NumberState
    {
        Falling,
        Crashed,
        Typed,
        Exploded,
        OffScreen
    }

    [Header("State")]
    public int number;
    public NumberState state = NumberState.Falling;

    [Header("Components")]
    public TMP_Text numberText;

    // GLOBAL STUFF


    private Rigidbody2D rb;
    private ExplosionHandler explosionHandler;
    private CollisionHandler collisionHandler;
    private GameManager gameManager;
    private BoxCollider2D boxCollider;

    [Header("Explosion Effect")]
    private GameObject vfxExplosionPrefab;

    float fallSpeed;
    float CHAR_WIDTH = 0.5f;
    private const float SPAWN_Y = 5.5f;

    #endregion


    void Start()
    {
        Initialise();
    }


    void Initialise()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        SetFallSpeed();

        collisionHandler = GetComponent<CollisionHandler>();
        explosionHandler = GetComponent<ExplosionHandler>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (!rb) Debug.LogError("Missing Rigidbody2D");
        if (!explosionHandler) Debug.LogError("Missing ExplosionHandler");
        if (!collisionHandler) Debug.LogError("Missing CollisionHandler");
        if (!gameManager) Debug.LogError("Missing GameManager");
        if (!boxCollider) Debug.LogError("Missing boxCollider");


        GenerateNumber();
        SetRandomXPosition();

    }


    private void Update()
    {

        CheckIfOffScreen();

        switch (state)
        {
            case NumberState.Falling:
                transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
                break;

            case NumberState.Crashed:
                // Debug.Log("[NumberController] NumberState.Crashed = " + state.ToString());
                transform.tag = "CrashedNumber";
                gameObject.layer = LayerMask.NameToLayer("CrashedNumber");
                //boxCollider.isTrigger = false;


                numberText.color = Color.yellow;
                //rb.linearVelocity = Vector2.zero; // ✅ Safe fallback
                rb.gravityScale = 1f;
                //rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.WakeUp();
                // Debug.Log($"[NumberController] {transform.name} .... GravityScale = " + rb.gravityScale + " | Velocity = " + rb.linearVelocity);
                break;

            case NumberState.Typed:
                Debug.Log("[NumberController] Setting CanExplode = true on: " + explosionHandler?.gameObject.name);

                explosionHandler.CanExplode = true;
                //state = NumberState.Exploded;
                break;

                case NumberState.Exploded:
                Debug.Log("[NumberController] State = Exploded");
                break;

            case NumberState.OffScreen:
                // Is Removable now
                break;
        }
    }

    private void CheckIfOffScreen()
    {
        if (transform.position.y < -5.5f)
        {
            state = NumberState.OffScreen;
        }
    }

    private void GenerateNumber()
    {
        number = NumberGenerator.GetRandomNumber();
        transform.name = number.ToString();
        numberText.text = number.ToString();
    }

    void SetRandomXPosition()
    {
        float halfWidth = CHAR_WIDTH / 2f;
        float minX = Camera.main.ViewportToWorldPoint(Vector3.zero).x + halfWidth;
        float maxX = Camera.main.ViewportToWorldPoint(Vector3.right).x - halfWidth;
        float randomX = Random.Range(minX, maxX);
        transform.position = new Vector3(randomX, SPAWN_Y, 0f);
    }

    public void SetFallSpeed()
    {
        fallSpeed = gameManager.GetFallSpeed("number");
        rb.gravityScale = 0;
    }

    public void MarkAsTyped()
    {
        if (state == NumberState.Falling || state == NumberState.Crashed)
        {
            state = NumberState.Typed;
        }
    }

    public void MarkAsExploded()
    {
        state = NumberState.Exploded;
    }



    public bool IsRemovable()
    {
        return state == NumberState.OffScreen || state == NumberState.Exploded;
    }

    public bool GetIsOffScreen() => state == NumberState.OffScreen;
    public bool GetHasCrashed() => state == NumberState.Crashed;

    public bool GetHasExploded() => state == NumberState.Exploded;
    public int GetNumber() => number;

}
