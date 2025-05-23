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
    [SerializeField] private Vector3 modelRotationOffset = Vector3.zero;

    [Header("Flashlight Control")]
    [SerializeField] private bool toggleWithF = true;

    private bool flashlightOn = true;

    private Ray GetRayFromRenderCamera()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 viewportPoint = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0f);
        return cam.ViewportPointToRay(viewportPoint);
    }

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
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Vector3 worldMousePos = cam.ScreenToWorldPoint(mousePos);

        Ray ray = GetRayFromRenderCamera();
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

        flashlight.transform.LookAt(targetPoint);

        if (flashlightModel != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - flashlightModel.position);
            targetRotation *= Quaternion.Euler(modelRotationOffset);

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
