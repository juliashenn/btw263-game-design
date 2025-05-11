using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Playables; // Required for Timeline
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;


public class FinalScene : MonoBehaviour
{
    public GameObject[] interactableItems;
    public PlayableDirector timelineDirector; // Reference to the Timeline Director

    private int interactedCount = 0;
    private bool hasPlayed = false;

    public Image fadeImage;
    public float fadeDuration = 2f;
    public string titleSceneName = "Waiting By the Door";

    [SerializeField] private Dialogue dialogue;
    [SerializeField] private PlayerInteractUI ui;
    [SerializeField] private Light sun;
    [SerializeField] private DoorInteractable door;
    [SerializeField] private CutsceneTrigger prevCutscene;

    [SerializeField] private string[] dialogueText;
    [SerializeField] private Texture2D[] dialogueImages;
    [SerializeField] private string[] speakers;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip newaudio;


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

        // Trigger the cutscene only once when all items are interacted with
        if (!hasPlayed && interactedCount == interactableItems.Length && !dialogue.isRunning() && prevCutscene.IsTriggered())
        {
            if (!door.opening)
            {
                door.ToggleDoor();
            }
            ui.disable();
            Color hexColor = new Color(253f / 255f, 185f / 255f, 0f / 255f); // #FDB900
            sun.color = hexColor;
            timelineDirector.Play();
            audio.volume = 0.7f;
            audio.clip = newaudio;
            audio.Play();
            hasPlayed = true;
        }
    }

    public void TriggerDialogue()
    {
        if (!dialogue.isRunning())
        {
            if (dialogueText.Length > 0)
            {
                InputSystem.DisableDevice(Keyboard.current);
                dialogue.SetDialogueText(dialogueText);
                dialogue.setDialogueImages(dialogueImages);
                dialogue.setSpeakers(speakers);
                dialogue.StartDialogue();
            }
        }
    }

    public void TriggerFadeToBlack()
    {
        StartCoroutine(WaitForDialogueThenFade());
    }

    IEnumerator WaitForDialogueThenFade()
    {
        // Wait until the dialogue finishes
        while (dialogue.isRunning())
        {
            yield return null;
        }

        // Then start fading
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TitleScreen");
    }

    IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 1);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;
    }
}
