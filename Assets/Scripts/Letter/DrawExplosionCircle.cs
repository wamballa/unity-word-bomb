using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawExplosionCircle : MonoBehaviour
{

    Explosion explosionHandler;
    float radius;
    private Transform explosionCircle;

    // Start is called before the first frame update
    void Start()
    {
        explosionHandler = transform.GetComponent<Explosion>();
        explosionCircle = gameObject.GetComponentInChildren<SpriteRenderer>().transform;
        radius = explosionHandler.GetRadius();
        explosionCircle.localScale = new Vector2 (radius, radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
