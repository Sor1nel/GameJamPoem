using UnityEngine;

public class MouseInteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer; // Set this to your Interactable layer
    [SerializeField] private float rayDistance = 100f;

    private InteractableOutline lastInteractable;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast against only the interactable layer
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            InteractableOutline interactable = hit.collider.GetComponent<InteractableOutline>();

            if (interactable != null)
            {
                // If it's a new object, disable the old outline
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
