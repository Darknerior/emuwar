using System.Collections;
using UnityEngine;
using TMPro;

public class CreditsScroll : MonoBehaviour {
    [SerializeField]private TMP_Text creditsText;       // The TextMeshPro component displaying the credits
    [SerializeField]private float scrollSpeed = 20f;    // Speed at which the credits scroll
    [SerializeField]private float delayBeforeStart = 15f; // Delay in seconds before the credits start scrolling
    private bool startScrolling = false;

    private void Start() {
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(delayBeforeStart);
        startScrolling = true;
    }

    private void Update() {
        if (startScrolling)creditsText.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
    }
}
