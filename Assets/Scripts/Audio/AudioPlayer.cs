using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public AudioClip clip;
    public AudioClip explosionClip;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        float randDelta = Random.Range(0, 0.5f);
        audioSource.pitch -= randDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Debug play sound");
            audioSource.PlayOneShot(clip);
            audioSource.pitch += 0.1f;
        }
    }

    public void Play()
    {
        audioSource.PlayOneShot(clip);
        audioSource.pitch += 0.1f;
    }

    public void PlayExplosion()
    {
        //int length = explosionClip.Length;
        //int index = Random.Range(0, length-1);
        float rand = Random.Range(-0.3f, 0.3f);
        audioSource.pitch -= rand;
        audioSource.PlayOneShot(explosionClip);
    }
}
