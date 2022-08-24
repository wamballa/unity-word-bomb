using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollisionHandler : MonoBehaviour
{
    bool hasCrashed;
    bool hasCrashedGround;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.CompareTag("Ground"))
    //    {
    //        hasCrashed = true;
    //    }
    //    if (collision.transform.CompareTag("ExplodedLetter"))
    //    {
    //        hasCrashed = true;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            hasCrashed = true;
        }
        if (collision.transform.CompareTag("ExplodedLetter"))
        {
            hasCrashed = true;
        }
    }
    public bool HasCrashed()
    {
        return hasCrashed;
    }

}
