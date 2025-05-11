using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public GameObject door;
    [SerializeField] private string interactText;

    public GameObject[] interactableItems;  
    private int interactedCount = 0;
    [SerializeField] private string[] dialogueText;
    [SerializeField] private Dialogue dialogue;

    private bool unlocked = false;
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        if (unlocked)
        {
            ToggleDoor();
        }
        else
        {
            if (!dialogue.isRunning())
            {
                if (dialogueText.Length > 0)
                {
                    InputSystem.DisableDevice(Keyboard.current);
                    dialogue.SetDialogueText(dialogueText);
                    dialogue.StartDialogue();
                }
            }
        }
    }

    public float openRot, closeRot, speed;
    public bool opening;
    void Update()
    {
        interactedCount = 0;
        foreach (var item in interactableItems)
        {
            if (item.GetComponent<Interactable>().isInteracted)
            {
                interactedCount++;
            }
        }

        if (interactedCount == interactableItems.Length)
        {
            unlocked = true;
        }

        if (unlocked)
        {
            Vector3 currentRot = door.transform.localEulerAngles;
            float newY = Mathf.LerpAngle(currentRot.y, opening ? openRot : closeRot, speed * Time.deltaTime);

            door.transform.localEulerAngles = new Vector3(currentRot.x, newY, currentRot.z);
        }
    }


    public void ToggleDoor()
    {
        opening = !opening;
    }


}
