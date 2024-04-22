using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads a scene using timer delay
/// </summary>
public class SceneLoader : MonoBehaviour {
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private string sceneToLoad = "Main";
    [SerializeField] private bool skippable;

    private void Start() {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private void Update()
    {
        if(!skippable)return;
        if(Input.anyKeyDown)SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator LoadSceneAfterDelay() {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
