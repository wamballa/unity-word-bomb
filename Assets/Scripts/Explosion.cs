using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

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
        audioSource = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleExplosion();
    }

    /// <summary>
    /// Hides the letter
    /// Plays an explosion sound
    /// Spawns the explosion prefab animation
    /// checks what objects in explosion radius
    /// - explodes words and letters only
    /// </summary>
    void HandleExplosion()
    {
        if (!canExplode) return;
        // run animation
        anim.SetBool("canExplode", true);
        HideLetter();
        PlaySFX();
        //SpawnPrefab(transform.position, explosionRadius);
        inExplosionRadius = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius/2,
            layerMask
            );

        foreach (Collider2D o in inExplosionRadius)
        {
            Word w = o.transform.parent.GetComponent<Word>();

            if (w != null)
            {
                w.SetWordHasBeenExploded();
            }
            if (o.transform.CompareTag("ExplodedLetter"))
            {
                Destroy(o.gameObject);
            }
        }
        canExplode = false;
    }
    /// <summary>
    /// 
    /// </summary>
    public void SetCanExplode()
    {
        canExplode = true;
    }
    /// <summary>
    /// Spawn the explosion animation with position and scale
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="scale"></param>
    void SpawnPrefab(Vector2 pos, float scale)
    {

        int rand = Random.Range(0, explosionsPF.Length-1);
        GameObject go = Instantiate(explosionsPF[rand], pos, Quaternion.identity);
        go.transform.localScale = Vector2.one * scale * 3;
        Destroy(go, 1f);
        Destroy(transform.gameObject, 1f);
    }
    /// <summary>
    /// Play boom when word explodes
    /// </summary>
    void PlaySFX()
    {
        //print("SAY BOOOOOM");
        float rand = Random.Range(0.8f, 1f);
        audioSource.pitch = rand;
        audioSource.PlayOneShot(explodeClip);
    }
    /// <summary>
    /// Hide the letter 
    /// </summary>
    void HideLetter()
    {
        sprite.SetActive(false);
    }
    /// <summary>
    /// Set the explosion radius
    /// </summary>
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
