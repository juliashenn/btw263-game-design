using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            float interactRange = 0.7f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }

    public IInteractable GetInteractableObject()
    {
        float interactRange = 0.7f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                return interactable;
            }
        }
        return null;
    }
}
