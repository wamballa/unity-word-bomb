using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler3D : MonoBehaviour
{

    bool hasCrashed;

    private void Start()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            //print(transform.name + "Collided with GROUND");
            hasCrashed = true;
        }
        if (collision.transform.CompareTag("ExplodedLetter"))
        {
            //print("Collided with LETTER");
            hasCrashed = true;
        }
    }


    public bool HasCrashed()
    {
        return hasCrashed;
    }
}
