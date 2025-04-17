using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodedLetter : MonoBehaviour
{
    public GameObject explosionEffect;

    private bool canExplode;
    private bool isOffScreen;



    // Update is called once per frame
    void Update()
    {
        CheckIfOffScreen();
        RemoveIfOffScreen();
        HandleExplosion();
    }

    private void HandleExplosion()
    {
        if (!canExplode) return;

        if (explosionEffect != null)
        {
            Debug.Log("Explode letter "+transform.name);

            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }
        Destroy(transform.gameObject);
    }

    void CheckIfOffScreen()
    {
        if (transform.position.y < -6.5f)
        {
            isOffScreen = true;
        }
    }


    void RemoveIfOffScreen()
    {
        if (isOffScreen)  Destroy(transform.gameObject);
    }

    public void TriggerExplosion()
    {
        canExplode = true;
    }

}
