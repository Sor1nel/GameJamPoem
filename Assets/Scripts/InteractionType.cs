using UnityEngine;

public class InteractionType : MonoBehaviour
{
    public enum Interaction
    {
        MoveThere,
        CloseWindow,
        ChooseRadio,
        ToggleObject
    }

    public Interaction interactionType;

    [Header("Move There")]
    public Transform moveTarget;

    [Header("Close Window")]
    public Transform windowObject;
    public float openDistance = 1f;

    [Header("Choose Radio")]
    public AudioSource audioSource;
    [Range(1, 4)] public int channel = 1;
    public AudioClip[] radioClips = new AudioClip[4];

    [Header("Toggle Object")]
    public GameObject toggleTarget;
    private bool toggleState = true;

    public void TriggerInteraction()
    {
        switch (interactionType)
        {
            case Interaction.MoveThere:
                if (moveTarget != null) MovePlayerToTarget();
                break;

            case Interaction.CloseWindow:
                if (windowObject != null) CloseTheWindow();
                break;

            case Interaction.ChooseRadio:
                if (audioSource != null && radioClips.Length >= channel) PlayRadioChannel();
                break;

            case Interaction.ToggleObject:
                if (toggleTarget != null) ToggleTargetObject();
                break;
        }
    }

    private void MovePlayerToTarget()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.transform.position = moveTarget.position;
    }

    private void CloseTheWindow()
    {
        windowObject.localPosition -= new Vector3(0, openDistance, 0);
    }

    private void PlayRadioChannel()
    {
        if (channel >= 1 && channel <= radioClips.Length)
        {
            audioSource.clip = radioClips[channel - 1];
            audioSource.Play();
        }
    }

    private void ToggleTargetObject()
    {
        toggleState = !toggleState;
        toggleTarget.SetActive(toggleState);
    }
}
