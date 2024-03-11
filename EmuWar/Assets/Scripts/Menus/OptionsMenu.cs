using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    void Start()
    {
        // Set fullscreen toggle based on current screen mode
        fullscreenToggle.isOn = Screen.fullScreen;

        // Set volume slider value based on current volume
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}