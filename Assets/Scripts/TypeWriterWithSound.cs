using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterWithSound : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueEntry
    {
        [TextArea(3, 10)]
        public string dialogueText;    // Text to display for this audio clip
        public AudioClip audioClip;    // Audio clip to play
        public Transform targetPosition; // Position where the dialogue box should appear
    }

    public DialogueEntry[] dialogueEntries; // Array of audio clips, texts, and positions
    public float defaultTypingSpeed = 0.05f; // Fallback typing speed if audio is missing
    public Text uiText;                     // Reference to UI Text element
    public RectTransform dialogueBox;       // Reference to the dialogue box RectTransform
    public AudioSource audioSource;         // Reference to AudioSource component

    private Coroutine typingCoroutine;

    void Start()
    {
        // Ensure required components are assigned
        if (uiText == null)
        {
            Debug.LogError("UI Text component is not assigned.");
            return;
        }

        if (dialogueBox == null)
        {
            Debug.LogError("Dialogue Box RectTransform is not assigned.");
            return;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing and not assigned.");
                return;
            }
        }

        // Start the dialogue sequence
        StartDialogueSequence();
    }

    public void StartDialogueSequence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(PlayDialogueSequence());
    }

    private IEnumerator PlayDialogueSequence()
    {
        uiText.text = ""; // Clear the text initially
        dialogueBox.gameObject.SetActive(true); // Ensure dialogue box is visible

        // Iterate through each dialogue entry
        foreach (DialogueEntry entry in dialogueEntries)
        {
            if (entry.dialogueText == null || entry.dialogueText.Length == 0)
            {
                Debug.LogWarning("Dialogue text is empty for an entry.");
                continue;
            }

            // Move dialogue box to the target position
            if (entry.targetPosition != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(entry.targetPosition.position);
                dialogueBox.position = screenPos;
            }
            else
            {
                Debug.LogWarning("Target position is not set for a dialogue entry.");
            }

            // Calculate typing speed based on audio duration
            float typingSpeed = defaultTypingSpeed;
            if (entry.audioClip != null)
            {
                float audioDuration = entry.audioClip.length;
                int charCount = entry.dialogueText.Length;
                if (charCount > 0 && audioDuration > 0)
                {
                    typingSpeed = audioDuration / charCount; // Time per character
                }
            }
            else
            {
                Debug.LogWarning("Audio clip is missing for a dialogue entry. Using default typing speed.");
            }

            // Play the audio clip
            if (entry.audioClip != null)
            {
                audioSource.PlayOneShot(entry.audioClip);
            }

            // Type the text
            yield return StartCoroutine(TypeText(entry.dialogueText, typingSpeed));

            // Wait for the audio to finish if it's still playing
            if (entry.audioClip != null)
            {
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
        }

        // Optionally hide the dialogue box when done
        dialogueBox.gameObject.SetActive(false);
    }

    private IEnumerator TypeText(string text, float typingSpeed)
    {
        uiText.text = ""; // Clear the text for this entry

        foreach (char letter in text)
        {
            uiText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}