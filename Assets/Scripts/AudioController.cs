using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    bool isMuted;
    float volume = 0;
    public Toggle toggle;
    bool canUpdatePrefs = false;

    void Start()
    {
        LoadSettings();
        SetAudio();
        //InitiateToggle();
    }
    void LoadSettings()
    {
        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
        print("Audio Controller isMuted = " + isMuted);
    }
    void SetAudio()
    {
        float _vol = (isMuted ? -80f : 0f);
        //print("Volume = " + _vol );
        audioMixer.SetFloat(
            "MasterVolume",
            _vol
            );
    }

    //void InitiateToggle()
    //{
    //    // update toggle
    //    if (SceneManager.GetActiveScene().buildIndex == 1)
    //    {
    //        if (isMuted)
    //        {
    //            toggle.isOn = true;
    //        }
    //        else
    //        {
    //            toggle.isOn = false;
    //        }
    //        canUpdatePrefs = true;
    //    }
    //}

    //public void ToggleIsMuted()
    //{
    //    //print("can update " + canUpdatePrefs);
    //    if (canUpdatePrefs)
    //    {
    //        isMuted = (PlayerPrefs.GetInt("isMuted") != 0);
    //        //print("is muted " + isMuted);
    //        isMuted = !isMuted;
    //        PlayerPrefs.SetInt("isMuted", (isMuted ? 1 : 0));
    //        //print("Is mute = " + (PlayerPrefs.GetInt("isMuted") != 0));
    //        SetAudio();
    //    }
    //}
}