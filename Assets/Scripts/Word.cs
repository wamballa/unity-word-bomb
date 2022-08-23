using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Word : MonoBehaviour {

    //public WordManager wordManager;

    private string _word;
    private int typeIndex;
    public TMP_Text text;

    // Explosion stuff
    public GameObject explodingPF;
    public Vector2 crashPos;

    private float fallSpeed = 1f;
    bool isOffScreen;
    bool hasCrashed;
    bool hasBeenTyped;

    public CollisionHandler collisionHandler;

    float boxColliderWidth;
    float CHAR_WIDTH = 0.5f;
    float FORCE = 2f;

    private void Start()
    {
        _word = WordGenerator.GetRandomWord();
        transform.name = _word;

        BoxCollider2D bc = transform.GetComponentInChildren<BoxCollider2D>();

        bc.size = new Vector2(_word.Length * CHAR_WIDTH, bc.size.y);

        SetText();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckIfCrashed();
        HandleExplosion();
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

            Vector2 newPos = new Vector2(xStartPos, crashPos.y + 0.5f);
            go = Instantiate(explodingPF, newPos, Quaternion.identity);
            go.GetComponentInChildren<TMP_Text>().text = _word[i].ToString();
            go.GetComponentInChildren<Rigidbody2D>().AddForce(transform.up * FORCE);
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
    //public bool HasBeenTyped()
    //{
    //    return hasBeenTyped;
    //}
    public string GetWord()
    {
        return _word;
    }
    private void SetText()
    {
        text.text = _word;
    }


}
