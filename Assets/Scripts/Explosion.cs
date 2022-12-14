using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    GameManager gameManager;
    Collider2D[] inExplosionRadius = null;
    //[SerializeField] private float explosionForceMulti = 5;
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private LayerMask layerMask;
    // Explosion stuff
    [SerializeField] private AudioClip explodeClip;
    private AudioSource audioSource;
    [SerializeField] private GameObject[] explosionsPF;
    [SerializeField] private GameObject sprite;

    Animator anim;

    bool canExplode;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null) print("ERROR: game manager not found");
        audioSource = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleExplosion();
    }


    void HandleExplosion()
    {
        if (!canExplode) return;
        // run animation
        anim.SetBool("canExplode", true);

        //HideLetter();

        PlaySFX();
        //SpawnPrefab(transform.position, explosionRadius);
        inExplosionRadius = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius/2,
            layerMask
            );

        foreach (Collider2D o in inExplosionRadius)
        {
            //Word w = o.transform.parent.GetComponent<Word>();
            //Word gw = o.gameObject.GetComponentInParent<Word>();

            //if (w != null)
            //if (o.gameObject.GetComponent<Word>() != null)
            if (o.transform.CompareTag("Word"))
            {
                //print("Word object called name " + o.name);
                gameManager.SetScore(2);
                o.transform.GetComponent<Word>().SetWordHasBeenExploded();
            }

            if (o.transform.CompareTag("ExplodedLetter"))
            {
                gameManager.SetScore(1);
                Destroy(o.gameObject);
            }
        }
        canExplode = false;
    }


    public void SetCanExplode()
    {
        canExplode = true;
    }


    void SpawnPrefab(Vector2 pos, float scale)
    {

        int rand = Random.Range(0, explosionsPF.Length-1);
        GameObject go = Instantiate(explosionsPF[rand], pos, Quaternion.identity);
        go.transform.localScale = Vector2.one * scale * 3;
        Destroy(go, 1f);
        Destroy(transform.gameObject, 1f);
    }


    void PlaySFX()
    {
        //print("SAY BOOOOOM");
        float rand = Random.Range(0.8f, 1f);
        audioSource.pitch = rand;
        audioSource.PlayOneShot(explodeClip);
    }


    void HideLetter()
    {
        sprite.SetActive(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius/2);
    }


    public float GetRadius()
    {
        return explosionRadius;
    }


    public void AnimationComplete()
    {
        GameObject go = transform.GetComponentInChildren<SpriteRenderer>().gameObject;
        Destroy(go);
        Destroy(gameObject);
    }
}
