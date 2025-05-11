using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneTrigger : MonoBehaviour
{

    [SerializeField] public GameObject[] interactableItems;
    [SerializeField] private string[] dialogueText;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Texture2D[] dialogueImages;
    [SerializeField] private string[] speakers;

    private bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    public bool IsTriggered()
    {
        return triggered;
    }

    private int interactedCount = 0;
    void Update()
    {
        if (!triggered)
        {
            interactedCount = 0;
            foreach (var item in interactableItems)
            {
                if (item.GetComponent<Interactable>().isInteracted)
                {
                    interactedCount++;
                }
            }

            if (interactedCount == interactableItems.Length && !dialogue.isRunning())
            {
                if (dialogueText.Length > 0)
                {
                    InputSystem.DisableDevice(Keyboard.current);
                    dialogue.SetDialogueText(dialogueText);
                    dialogue.setDialogueImages(dialogueImages);
                    dialogue.setSpeakers(speakers);
                    dialogue.StartDialogue();
                    triggered = true;
                }
            }
        }
    }
}
