using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterExplosionHandler : MonoBehaviour
{
    [Header("Explosion Settings")]
    private float explosionRadius = 5f;
    public LayerMask layerMask;
    public AudioClip explodeClip;
    public bool CanExplode { get; set; }

    public GameObject vfxExplosionPrefab;

    private AudioSource audioSource;
    private GameManager gameManager;
    private Collider2D[] objectsInExplosionRadius = null;
    private Animator animator;



    //private GameObject[] explosionsPF;
    //private GameObject sprite;

    //bool canExplode;




    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();

        if (!animator) Debug.LogError("Missing Animator on ExplosionHandler.");
        if (!gameManager) Debug.LogError("Missing GameManager reference.");

        CanExplode = false;
    }

    private void Update()
    {
        if (CanExplode) HandleExplosion();
    }

    public void HandleExplosion()
    {
        // Debug.Log($"[ExplosionHandler] HandleExplosion called on {gameObject.name}");

        CanExplode = false;

        // animator.SetBool("canExplode", true);

        PlaySFX();

        Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius / 2, layerMask);

        CreateExplosionVFX();

        /// do something

    }
    void PlaySFX()
    {

        if (explodeClip == null) { Debug.LogError("No audio clip assigned to Number!"); return; }

        audioSource.PlayOneShot(explodeClip);
    }

    private void CreateExplosionVFX()
    {
        if (vfxExplosionPrefab)
        {
            GameObject fx = Instantiate(vfxExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 1f);
        }
    }

    //public void animstart() => Debug.Log("");
    //public void animstop() => Debug.Log("");

    public void animstart() => Debug.Log($"[explosionhandler] explosion animation started for {transform.name}.");
    public void animstop() => Debug.Log($"[explosionhandler] explosion animation ended for {transform.name}.");

    public float GetRadius() => explosionRadius;

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, explosionRadius / 2);
    }
}
