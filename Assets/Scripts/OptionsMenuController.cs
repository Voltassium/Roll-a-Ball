using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenuController : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource backgroundMusic; 

    void Start()
    {
        if (volumeSlider != null && backgroundMusic != null) 
        {
            volumeSlider.value = backgroundMusic.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
        }
    }
}