using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private string[] dialogueText;
    [SerializeField] private Dialogue dialogue;
    public bool isInteracted = false;

    [SerializeField] private Texture2D[] dialogueImages; 

    public void Interact()
    {
        isInteracted = true;
        if (!dialogue.isRunning())
        {
            if (dialogueText.Length > 0)
            {
                InputSystem.DisableDevice(Keyboard.current);
                dialogue.SetDialogueText(dialogueText);
                dialogue.setDialogueImages(dialogueImages);
                dialogue.StartDialogue();
            }
        }
       
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
