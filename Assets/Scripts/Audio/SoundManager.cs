using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            LoadAudioPreferences();
        }

        else
        {
            LoadAudioPreferences();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    private void LoadAudioPreferences()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void SaveAudioPreferences()
    {
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
    }
}
