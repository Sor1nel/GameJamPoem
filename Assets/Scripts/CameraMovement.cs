using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Edge Settings")]
    [SerializeField] float edgeOffset = 50f;        // Pixels from screen edge to start rotating
    [SerializeField] float rotationSpeed = 45f;     // Degrees per second

    [Header("Rotation Limits")]
    [SerializeField] float minYAngle = -45f;        // Minimum Y rotation
    [SerializeField] float maxYAngle = 45f;         // Maximum Y rotation

    private float currentYAngle;

    void Start()
    {
        currentYAngle = transform.eulerAngles.y;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;

        float rotationDelta = 0f;

        if (mousePos.x < edgeOffset)
        {
            rotationDelta = -rotationSpeed * Time.deltaTime;
        }
        else if (mousePos.x > screenWidth - edgeOffset)
        {
            rotationDelta = rotationSpeed * Time.deltaTime;
        }

        currentYAngle += rotationDelta;

        // Clamp angle between min and max
        currentYAngle = ClampAngle(currentYAngle, minYAngle, maxYAngle);

        // Apply rotation only on Y axis
        transform.rotation = Quaternion.Euler(0f, currentYAngle, 0f);
    }

    // Clamp method for 0-360 wraparound angles
    float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360f;
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }
}