using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI speakerComponent;
    public string[] speakers;
    public string[] lines;
    public float textSpeed;
    public Texture2D[] images;
    public RawImage imageComponent;

    private bool speakersSet = false;
    private int index;
    private bool canRun;
    private Coroutine typingCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        containerGameObject.SetActive(false);
        textComponent.text = string.Empty;
        speakerComponent.text = string.Empty;
        speakerComponent.gameObject.SetActive(false);
        canRun = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isRunning())
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                }
                textComponent.text = lines[index];
            }
        }
    }

    public void disable()
    {
        containerGameObject.SetActive(false);
        canRun = false;
    }

    public void StartDialogue()
    {
        InputSystem.DisableDevice(Keyboard.current);
        containerGameObject.SetActive(canRun);
        index = 0;
        UpdateImage();
        UpdateSpeaker();
        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            UpdateImage();
            UpdateSpeaker();
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = string.Empty;
            imageComponent.texture = null;
            speakers = new string[0];
            containerGameObject.SetActive(false);
            InputSystem.EnableDevice(Keyboard.current);
        }
    }

    public void UpdateImage()
    {
        if (images.Length > index && images[index] != null)
        {
            imageComponent.texture = images[index];
            imageComponent.gameObject.SetActive(true);
        }
        else
        {
            imageComponent.gameObject.SetActive(false);
        }
        
    }

    public void UpdateSpeaker()
    {
        if (speakers.Length > index && speakers[index] != null && speakers[index] != "")
        {
            speakerComponent.text = speakers[index];
            speakerComponent.gameObject.SetActive(true);
        }
        else
        {
            speakerComponent.gameObject.SetActive(false);
        }

    }

    public void SetDialogueText(string[] t)
    {
        lines = t;
    }

    public bool isRunning()
    {
        return textComponent.text != string.Empty;
    }

    public void setDialogueImages(Texture2D[] dialogueimages)
    {
        images = dialogueimages;
    }

    public void setSpeakers(string[] s)
    {
        speakers = s;
        speakersSet = true;
    }
}
