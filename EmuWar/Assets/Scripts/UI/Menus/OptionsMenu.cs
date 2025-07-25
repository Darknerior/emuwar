using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
[AddComponentMenu(" Game Scripts / UI / Menu / Options Menu",2)]
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

    public void SetVolume() {
        var volume = volumeSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetFullscreen() {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
}