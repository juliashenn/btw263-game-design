using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private bool canInteract = true;
    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        } else
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        containerGameObject.SetActive(canInteract);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }

    public void disable()
    {
        canInteract = false;
        containerGameObject.SetActive(false);
    }
}
