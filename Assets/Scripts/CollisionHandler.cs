using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    bool hasCrashed;

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(">>>>>>>Collide");
        if (collision.transform.CompareTag("Ground"))
        {
            print("Word Crashed Into Ground");
            hasCrashed = true;
        }
    }

    public bool HasCrashed()
    {
        return hasCrashed;
    }
}
