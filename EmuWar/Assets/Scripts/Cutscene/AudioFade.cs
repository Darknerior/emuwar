using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    public AudioSource audioSource;         
    public float delayBeforeFade = 14f;       
    public float fadeDuration = 1f;        
    public float targetVolume = 0.1f;

    private void Start() {
        StartCoroutine(FadeOutVolume());
    }

    IEnumerator FadeOutVolume() {
        yield return new WaitForSeconds(delayBeforeFade);
        float currentTime = 0;
        float startVolume = audioSource.volume;
        
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
        
        audioSource.volume = targetVolume;
    }
}
