using System.Collections;
using UnityEngine;
using TMPro; // Add TextMeshPro namespace

public class TypeWriterWithSound : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueEntry
    {
        [TextArea(3, 10)]
        public string dialogueText;    // Text to display
        public AudioClip audioClip;    // Audio clip to play
        public Transform audioPosition; // Position to play sound from
    }

    public DialogueEntry[] dialogueEntries;
    public float defaultTypingSpeed = 0.05f;
    public TMP_Text uiText;                // Reference to TMP_Text component
    public RectTransform dialogueBox;      // Reference to dialogue box RectTransform
    public AudioSource audioSource;        // Reference to AudioSource component

    private Coroutine typingCoroutine;

    void Start()
    {
        if (uiText == null) { Debug.LogError("TMP Text not assigned."); return; }
        if (dialogueBox == null) { Debug.LogError("Dialogue Box not assigned."); return; }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) { Debug.LogError("AudioSource not assigned."); return; }
        }

        StartDialogueSequence();
    }

    public void StartDialogueSequence()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(PlayDialogueSequence());
    }

    private IEnumerator PlayDialogueSequence()
    {
        uiText.text = "";
        dialogueBox.gameObject.SetActive(true);

        foreach (DialogueEntry entry in dialogueEntries)
        {
            if (string.IsNullOrEmpty(entry.dialogueText))
            {
                Debug.LogWarning("Dialogue text is empty.");
                continue;
            }

            float typingSpeed = defaultTypingSpeed;
            if (entry.audioClip != null)
            {
                float audioDuration = entry.audioClip.length;
                int charCount = entry.dialogueText.Length;
                if (charCount > 0 && audioDuration > 0)
                    typingSpeed = audioDuration / charCount;
            }
            else
            {
                Debug.LogWarning("Audio clip missing. Using default typing speed.");
            }

            if (entry.audioClip != null)
            {
                if (entry.audioPosition != null)
                    audioSource.transform.position = entry.audioPosition.position;
                audioSource.Stop();
                audioSource.PlayOneShot(entry.audioClip);
            }

            yield return StartCoroutine(TypeText(entry.dialogueText, typingSpeed));

            if (entry.audioClip != null)
                while (audioSource.isPlaying) yield return null;
        }

        dialogueBox.gameObject.SetActive(false);
    }

    private IEnumerator TypeText(string text, float typingSpeed)
    {
        uiText.text = "";
        foreach (char letter in text)
        {
            uiText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}