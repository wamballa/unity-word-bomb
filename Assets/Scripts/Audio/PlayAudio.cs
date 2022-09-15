using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{

    public AudioClip clip;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
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
}
