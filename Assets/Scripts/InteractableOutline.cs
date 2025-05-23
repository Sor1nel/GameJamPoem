using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableOutline : MonoBehaviour
{
    private AudioSource m_AudioSourceVoices;
    private bool aplaying;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        m_AudioSourceVoices = GetComponent<AudioSource>();
        DisableOutline();
    }

    public void EnableOutline()
    {
        if(m_AudioSourceVoices)
        {
            m_AudioSourceVoices.Play();
        }

        if (outline != null)
            outline.enabled = true;
    }

    public void DisableOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }
}
