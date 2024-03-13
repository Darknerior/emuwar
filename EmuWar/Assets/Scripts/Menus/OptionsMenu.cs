using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    private void Start() {
        if (audioMixer == null) {
            var audioMixers = FindObjectsOfType<AudioMixer>();
            if (audioMixers.Length > 0)audioMixer = audioMixers[0];
            else Debug.Log("No audio mixer found");
        }
        
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