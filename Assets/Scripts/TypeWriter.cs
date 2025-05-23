using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    [TextArea(3, 10)]
    public string dialogueText;            // Full dialogue line

    public float typingSpeed = 0.05f;      // Time between letters
    public Text uiText;                    // Reference to UI Text element

    private Coroutine typingCoroutine;

    void Start()
    {
        // Optionally start immediately
        StartTyping(dialogueText);
    }

    public void StartTyping(string newText)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText = newText;
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        uiText.text = "";

        foreach (char letter in dialogueText)
        {
            uiText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}