using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Edge Settings")]
    [SerializeField] float edgeOffset = 50f;        // Pixels from screen edge to start rotating
    [SerializeField] float rotationSpeed = 45f;     // Degrees per second

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

        // No clamping — allow infinite rotation
        transform.rotation = Quaternion.Euler(0f, currentYAngle, 0f);
    }
}
