using System.Collections;
using UnityEngine;

/// <summary>
/// Fades an audio source
/// </summary>
public class AudioFade : MonoBehaviour {
    [SerializeField]private AudioSource audioSource;         
    [SerializeField]private float delayBeforeFade = 14f;       
    [SerializeField]private float fadeDuration = 1f;        
    [SerializeField]private float targetVolume = 0.1f;
    

    private void Start() {
        StartCoroutine(FadeOutVolume());
    }

    private IEnumerator FadeOutVolume() {
        yield return new WaitForSeconds(delayBeforeFade);
        float currentTime = 0;
        var startVolume = audioSource.volume;
        
        while (currentTime < fadeDuration){
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
        
        audioSource.volume = targetVolume;
    }
}
