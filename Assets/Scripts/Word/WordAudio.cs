// handles sounds

using UnityEngine;
using UnityEngine;

public class WordAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip successSound;

    public void PlaySuccess()
    {
        if (successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }
    }
}