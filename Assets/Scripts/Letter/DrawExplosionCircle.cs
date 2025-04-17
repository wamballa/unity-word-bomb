using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawExplosionCircle : MonoBehaviour
{

    public LetterExplosionHandler explosionHandler;
    private float radius;
    public Transform explosionCircle;

    // Start is called before the first frame update
    void Start()
    {
        //explosionCircle = gameObject.GetComponentInChildren<SpriteRenderer>().transform;
        if (explosionCircle.gameObject.activeSelf)
        {
            //explosionHandler = transform.GetComponent<ExplosionHandler>();
            radius = explosionHandler.GetRadius();
            float currentScale = transform.localScale.x;
            explosionCircle.localScale = new Vector2(radius, radius);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
