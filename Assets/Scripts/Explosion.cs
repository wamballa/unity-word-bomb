using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    Collider2D[] inExplosionRadius = null;
    [SerializeField] private float explosionForceMulti = 5;
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private LayerMask layerMask;
    // sfx
    [SerializeField] private AudioClip explodeClip;
    private AudioSource audioSource;

    [SerializeField] private GameObject[] explosionsPF;

    bool canExplode;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleExplosion();
    }


    void HandleExplosion()
    {
        if (!canExplode) return;
        print("EXPLODE");
        PlaySFX();
        SpawnPrefab(transform.position, 10);
        inExplosionRadius = Physics2D.OverlapCircleAll(
            transform.position, explosionRadius, layerMask);

        foreach (Collider2D o in inExplosionRadius)
        {
            //print("Collided with " + o.transform.name);
            //Rigidbody2D o_rb = o.GetComponent<Rigidbody2D>();
            //if (o_rb != null)
            //{
            //    Vector2 dist = o.transform.position - transform.position;
            //    if (dist.magnitude > 0)
            //    {
            //        float explosionForce = explosionForceMulti / dist.magnitude;
            //        o_rb.AddForce(dist.normalized * explosionForce, ForceMode2D.Impulse);
            //    }
            //}
            Word w = o.transform.parent.GetComponent<Word>();
            //w = o.transform.parent.GetComponent<Word>();
            if (w != null)
            {
                w.SetWordHasBeenExploded();
                //SpawnPrefab(o.transform.position, 10);
            }
            if (o.transform.CompareTag("ExplodedLetter"))
            {
                Destroy(o.gameObject);
                //SpawnPrefab(o.transform.position, 10);
            }
        }

        canExplode = false;
    }
    public void SetCanExplode()
    {
        //print("SetCanExplode for "+transform.name);
        canExplode = true;
        //print("Can explode = " + canExplode);
    }

    void SpawnPrefab(Vector2 pos, float scale)
    {

        int rand = Random.Range(0, explosionsPF.Length-1);
        GameObject go = Instantiate(explosionsPF[rand], pos, Quaternion.identity);
        go.transform.localScale = Vector2.one * scale;
        Destroy(go, 0.5f);
        Destroy(transform.gameObject);
    }

    void PlaySFX()
    {
        print("SAY BOOOOOM");
        //float rand = Random.Range(0.5f, 1.5f);
        //audioSource.pitch = rand;
        audioSource.PlayOneShot(explodeClip);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
