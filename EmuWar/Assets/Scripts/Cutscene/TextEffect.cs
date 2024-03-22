using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public float delayBetweenCharacters = 0.1f;
    public string fullText = "Your text here";
    
    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        var typedText = "";
        foreach (var c in fullText)
        {
            if (c != '*') // Check if the character is not a new line character
            {
                typedText += c;
                textComponent.text = typedText;
                yield return new WaitForSeconds(delayBetweenCharacters);
            }
            else // If the character is a new line character, add it immediately
            {
                typedText += "\n";
                textComponent.text = typedText;
            }
        }
    }
}
