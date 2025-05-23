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
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float bobFrequency = 2f;
    [SerializeField] private float bobAmplitude = 0.05f;

    [Header("Close Window")]
    public Transform windowObject;
    public float windowLowerAmount = 1f;
    public float windowCloseSpeed = 1f;

    [SerializeField] public bool isWindowOpen = false;
    [HideInInspector] public Vector3 windowTargetPos;

    [Header("Choose Radio")]
    public AudioSource audioSource;
    private int currentChannel = 0; // Tracks the current song index
    public AudioClip[] radioClips = new AudioClip[4];

    [Header("Toggle Object")]
    public GameObject toggleTarget;
    private bool toggleState = true;

    private Camera mainCam;

    private bool isMovingCamera = false;
    private Vector3 moveStartPos;
    private float moveProgress = 0f;

    private bool isClosingWindow = false;
    private Vector3 windowStartPos;
    private float windowCloseProgress = 0f;

    private void Start()
    {
        mainCam = Camera.main;

        // Force the window to start CLOSED
        if (windowObject != null)
        {
            isWindowOpen = false;
            // Ensure window is at closed position
            windowObject.localPosition = windowObject.localPosition; // Use current position as closed
        }
    }

    private void Update()
    {
        if (isMovingCamera)
            MoveCameraWithBob();

        if (isClosingWindow)
            CloseWindowSmoothly();
    }

    public void TriggerInteraction()
    {
        switch (interactionType)
        {
            case Interaction.MoveThere:
                if (moveTarget != null)
                    StartCameraMove();
                break;

            case Interaction.CloseWindow:
                if (windowObject != null && isWindowOpen) // Only close if it is open
                {
                    windowStartPos = windowObject.localPosition;
                    windowTargetPos = windowStartPos - new Vector3(0, windowLowerAmount, 0);
                    windowCloseProgress = 0f;
                    isClosingWindow = true;
                }
                break;

            case Interaction.ChooseRadio:
                if (audioSource != null && radioClips.Length > 0)
                {
                    // Cycle to the next song
                    currentChannel = (currentChannel + 1) % radioClips.Length;
                    audioSource.clip = radioClips[currentChannel];
                    audioSource.Play();
                }
                break;

            case Interaction.ToggleObject:
                if (toggleTarget != null)
                {
                    toggleState = !toggleState;
                    toggleTarget.SetActive(toggleState);
                }
                break;
        }
    }

    private void StartCameraMove()
    {
        if (mainCam == null) return;
        moveStartPos = mainCam.transform.position;
        moveProgress = 0f;
        isMovingCamera = true;
    }

    private void MoveCameraWithBob()
    {
        if (mainCam == null || moveTarget == null)
        {
            isMovingCamera = false;
            return;
        }

        moveProgress += Time.deltaTime * moveSpeed;

        Vector3 targetPosition = Vector3.Lerp(moveStartPos, moveTarget.position, moveProgress);
        float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        Vector3 bobbedPosition = targetPosition + new Vector3(0, bobOffset, 0);

        mainCam.transform.position = bobbedPosition;
        Vector3 lookDirection = moveTarget.position - bobbedPosition;
        if (lookDirection != Vector3.zero)
            mainCam.transform.rotation = Quaternion.LookRotation(lookDirection);

        if (moveProgress >= 1f)
        {
            mainCam.transform.position = moveTarget.position;
            mainCam.transform.LookAt(moveTarget.position);
            isMovingCamera = false;
        }
    }

    private void CloseWindowSmoothly()
    {
        if (windowObject == null)
        {
            isClosingWindow = false;
            return;
        }

        windowCloseProgress += Time.deltaTime * windowCloseSpeed;
        windowObject.localPosition = Vector3.Lerp(windowStartPos, windowTargetPos, windowCloseProgress);

        if (windowCloseProgress >= 1f)
        {
            windowObject.localPosition = windowTargetPos;
            isClosingWindow = false;
            isWindowOpen = false; // Ensure bool is set to false when closing completes
        }
    }
}