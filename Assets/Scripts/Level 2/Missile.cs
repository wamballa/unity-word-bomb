using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody2D rb;
    float force;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        force = 300f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Add force");
            Vector2 forceDir = new Vector2(0, 1);
            forceDir = Vector2.up + Vector2.left;
            rb.AddForce(forceDir * force);
        }
    }
}
