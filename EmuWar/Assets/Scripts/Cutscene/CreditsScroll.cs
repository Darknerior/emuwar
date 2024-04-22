using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsScroll : MonoBehaviour
{
    public TMP_Text creditsText;       // The TextMeshPro component displaying the credits
    public float scrollSpeed = 20f;    // Speed at which the credits scroll
    public float delayBeforeStart = 15f; // Delay in seconds before the credits start scrolling

    private bool startScrolling = false;

    private void Start()
    {
        // Start the coroutine that handles the delayed start
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        startScrolling = true;
    }

    private void Update()
    {
        if (startScrolling)
        {
            // Move the text upward by changing its Y position
            creditsText.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
    }
}
