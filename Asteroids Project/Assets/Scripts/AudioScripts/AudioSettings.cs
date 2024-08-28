using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
    }

    public void SetLevel(float sliderVal) {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("MasterVol", sliderVal);
    }
}
