using UnityEngine;

public class ClickInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float maxDistance = 100f;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
            {
                var interaction = hit.collider.GetComponent<InteractionType>();
                if (interaction != null)
                {
                    interaction.TriggerInteraction();
                }
            }
        }
    }
}
