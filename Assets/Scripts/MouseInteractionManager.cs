using UnityEngine;

public class MouseInteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float rayDistance = 100f;

    private InteractableOutline lastInteractable;

    void Update()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            InteractableOutline interactable = hit.collider.GetComponent<InteractableOutline>();

            if (interactable != null)
            {
                if (interactable != lastInteractable)
                {
                    if (lastInteractable != null)
                        lastInteractable.DisableOutline();

                    lastInteractable = interactable;
                    lastInteractable.EnableOutline();
                }
            }
            else
            {
                ClearLastInteractable();
            }
        }
        else
        {
            ClearLastInteractable();
        }
    }

    void ClearLastInteractable()
    {
        if (lastInteractable != null)
        {
            lastInteractable.DisableOutline();
            lastInteractable = null;
        }
    }
}
