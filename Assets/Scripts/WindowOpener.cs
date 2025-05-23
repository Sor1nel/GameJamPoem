using UnityEngine;

public class WindowOpener : MonoBehaviour
{
    [Header("References")]
    public InteractionType interactionScript;
    public GameObject handObject;
    public Transform pointA; // Hand moves down to here
    public Transform pointB; // Hand starts/ends here

    [Header("Speeds (seconds to complete)")]
    public float handToADuration = 1f;
    public float windowLiftDuration = 1f;
    public float handBackToBDuration = 1f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip openSound;

    [Header("Timer Settings")]
    public float delayBetweenSequences = 10f; // Changed to 10 seconds
    [SerializeField] private float debugTimer = 0f;

    private Vector3 initialWindowPos;
    private float moveProgress = 0f;
    private bool waitingToRestart = false;

    private enum State
    {
        Idle,
        HandToA,
        WindowUp,
        HandBackToB
    }

    private State currentState = State.Idle;

    void Start()
    {
        if (interactionScript != null && interactionScript.windowObject != null)
        {
            interactionScript.isWindowOpen = false; // Force it closed at start
            initialWindowPos = interactionScript.windowObject.localPosition;
        }

        if (handObject != null && pointB != null)
        {
            handObject.transform.position = pointB.position;
        }
    }

    void Update()
    {
        // Start the timer when the window is closed and idle
        if (!interactionScript.isWindowOpen && !waitingToRestart && currentState == State.Idle)
        {
            waitingToRestart = true;
            debugTimer = 0f;
        }

        if (waitingToRestart)
        {
            debugTimer += Time.deltaTime;

            if (debugTimer >= delayBetweenSequences)
            {
                debugTimer = 0f;
                waitingToRestart = false;
                moveProgress = 0f;
                initialWindowPos = interactionScript.windowObject.localPosition; // Update to new position
                currentState = State.HandToA;

                if (audioSource && openSound)
                    audioSource.PlayOneShot(openSound);
            }
        }

        switch (currentState)
        {
            case State.HandToA:
                MoveHand(pointB.position, pointA.position, handToADuration, State.WindowUp);
                break;

            case State.WindowUp:
                MoveWindowAndHandUp(windowLiftDuration, State.HandBackToB);
                break;

            case State.HandBackToB:
                MoveHand(handObject.transform.position, pointB.position, handBackToBDuration, State.Idle);
                break;

            case State.Idle:
                break; // isWindowOpen set in MoveWindowAndHandUp
        }
    }

    private void MoveHand(Vector3 from, Vector3 to, float duration, State nextState)
    {
        moveProgress += Time.deltaTime / duration;
        handObject.transform.position = Vector3.Lerp(from, to, moveProgress);

        if (moveProgress >= 1f)
        {
            handObject.transform.position = to;
            moveProgress = 0f;
            currentState = nextState;
        }
    }

    private void MoveWindowAndHandUp(float duration, State nextState)
    {
        moveProgress += Time.deltaTime / duration;

        float liftAmount = interactionScript.windowLowerAmount;

        Vector3 windowFrom = initialWindowPos;
        Vector3 windowTo = windowFrom + new Vector3(0, liftAmount, 0);

        Vector3 handFrom = pointA.position;
        Vector3 handTo = handFrom + new Vector3(0, liftAmount, 0);

        interactionScript.windowObject.localPosition = Vector3.Lerp(windowFrom, windowTo, moveProgress);
        handObject.transform.position = Vector3.Lerp(handFrom, handTo, moveProgress);

        if (moveProgress >= 1f)
        {
            interactionScript.windowObject.localPosition = windowTo;
            handObject.transform.position = handTo;
            interactionScript.isWindowOpen = true; // Set bool to true when opening completes
            moveProgress = 0f;
            currentState = nextState;
        }
    }
}