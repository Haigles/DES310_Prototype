using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject audioMusicObject;
    public Slider volumeSlider;

    private float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        audioMusicObject = GameObject.FindWithTag("GameMusic");
        musicVolume = PlayerPrefs.GetFloat("volume");
        audioSource.volume = musicVolume;
        volumeSlider.value = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("volume", musicVolume);
    }

    public void UpdateVolume(float volume)
    {
        musicVolume = volumeSlider.value;
    }
}
