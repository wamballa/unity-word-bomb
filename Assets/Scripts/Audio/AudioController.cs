using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        LoadSettings();
    }
    void LoadSettings()
    {
        bool isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
        AudioMute(isMuted);
    }

    public void AudioMute(bool isMuted)
    {
        float _vol = (isMuted ? -80f : 0f);
        audioMixer.SetFloat("MasterVolume", _vol);
    }

}