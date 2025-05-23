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
    public AudioClip jumpScareSound; // Sound for jump scare

    [Header("Timer Settings")]
    public float delayBetweenSequences = 10f;
    public float jumpScareTimerDuration = 10f; // 10-second timer for jump scare
    [SerializeField] private float debugTimer = 0f;
    [SerializeField] private float jumpScareTimer = 0f;

    [Header("Jump Scare")]
    public GameObject jumpScareObject; // Canvas object to activate

    private Vector3 initialWindowPos;
    private float moveProgress = 0f;
    private bool waitingToRestart = false;
    private bool jumpScareTimerActive = false;

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
            interactionScript.isWindowOpen = false;
            initialWindowPos = interactionScript.windowObject.localPosition;
        }

        if (handObject != null && pointB != null)
        {
            handObject.transform.position = pointB.position;
        }

        if (jumpScareObject != null)
        {
            jumpScareObject.SetActive(false); // Ensure jump scare object is inactive
        }
    }

    void Update()
    {
        // Handle sequence timer and restart on window close
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
                StartOpeningSequence();
            }
        }

        // Restart opening sequence immediately if window closes
        if (!interactionScript.isWindowOpen && (currentState != State.Idle || jumpScareTimerActive))
        {
            StartOpeningSequence();
        }

        // Handle jump scare timer
        if (interactionScript.isWindowOpen && !jumpScareTimerActive)
        {
            jumpScareTimerActive = true;
            jumpScareTimer = jumpScareTimerDuration; // Start 10-second timer
        }

        if (jumpScareTimerActive)
        {
            jumpScareTimer -= Time.deltaTime;
            if (jumpScareTimer <= 0f && interactionScript.isWindowOpen)
            {
                TriggerJumpScare();
                jumpScareTimerActive = false;
            }
        }

        // Handle state machine
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
                break;
        }
    }

    private void StartOpeningSequence()
    {
        debugTimer = 0f;
        waitingToRestart = false;
        moveProgress = 0f;
        jumpScareTimerActive = false; // Reset jump scare timer
        jumpScareTimer = 0f;
        if (interactionScript != null && interactionScript.windowObject != null)
        {
            initialWindowPos = interactionScript.windowObject.localPosition;
        }
        currentState = State.HandToA;

        if (audioSource && openSound)
            audioSource.PlayOneShot(openSound);
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
            interactionScript.isWindowOpen = true;
            moveProgress = 0f;
            currentState = nextState;
        }
    }

    private void TriggerJumpScare()
    {
        if (jumpScareObject != null)
        {
            jumpScareObject.SetActive(true);
        }
        if (audioSource && jumpScareSound)
        {
            audioSource.PlayOneShot(jumpScareSound);
        }
    }
}