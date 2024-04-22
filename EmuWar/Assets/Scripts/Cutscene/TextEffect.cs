using System.Collections;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [SerializeField]private float delayBetweenCharacters = 0.1f;
    [SerializeField]private string fullText = "Your text here";
    [SerializeField]private float delayBeforeClearing = 15f;
    private TMP_Text textComponent;

    private void Awake() {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Start() {
        StartCoroutine(TypeText());
        StartCoroutine(ClearTextAfterDelay());
    }

    private IEnumerator TypeText() {
        var typedText = "";
        foreach (var c in fullText) {
            if (c != '*') {// Check if the character is not a new line character
            
                typedText += c;
                textComponent.text = typedText;
                yield return new WaitForSeconds(delayBetweenCharacters);
            }
            else { // If the character is a new line character, add it immediately
                typedText += "\n";
                textComponent.text = typedText;
            }
        }
    }

    private IEnumerator ClearTextAfterDelay() {
        yield return new WaitForSeconds(delayBeforeClearing); // Wait for the specified delay
        textComponent.text = ""; // Clear the text
    }
}
