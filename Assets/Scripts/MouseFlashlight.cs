using UnityEngine;

public class MouseFlashlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private Light flashlight;
    [SerializeField] private Transform flashlightModel;

    [Header("Aiming Settings")]
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private float maxDistance = 100f;

    [Header("Model Rotation Settings")]
    [SerializeField] private bool smoothModelAim = true;
    [SerializeField] private float modelAimSpeed = 5f;
    [SerializeField] private Vector3 modelRotationOffset = Vector3.zero; // <<<< NEW

    [Header("Flashlight Control")]
    [SerializeField] private bool toggleWithF = true;

    private bool flashlightOn = true;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        HandleToggle();

        if (!flashlightOn || flashlight == null)
            return;

        AimFlashlightAndModel();
    }

    private void HandleToggle()
    {
        if (toggleWithF && Input.GetKeyDown(KeyCode.F))
        {
            flashlightOn = !flashlightOn;
            flashlight.enabled = flashlightOn;
        }
    }

    private void AimFlashlightAndModel()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, maxDistance, aimLayerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxDistance;
        }

        // Aim the spotlight
        flashlight.transform.LookAt(targetPoint);

        // Aim the model with offset
        if (flashlightModel != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - flashlightModel.position);
            targetRotation *= Quaternion.Euler(modelRotationOffset); // <<< Apply rotation fix

            if (smoothModelAim)
            {
                flashlightModel.rotation = Quaternion.Slerp(flashlightModel.rotation, targetRotation, modelAimSpeed * Time.deltaTime);
            }
            else
            {
                flashlightModel.rotation = targetRotation;
            }
        }
    }
}
