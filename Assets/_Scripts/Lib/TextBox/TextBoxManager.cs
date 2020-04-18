using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    private static GameObject textBox;
    private static GameObject textBoxBackground;
    private static GameObject textBoxAuthor;
    private static GameObject textBoxContent;
    private static AudioSource audioSource;
    public static bool active = false;
    public static bool paused = false;

    public static Conversation conversation;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        textBox = GameObject.FindGameObjectWithTag("TextBox");
        if (textBox != null)
        {

            Transform[] children = textBox.GetComponentsInChildren<Transform>(true);

            foreach (Transform tr in children) 
            {
                switch (tr.gameObject.tag)
                {
                    case "TextBoxBackground":
                        textBoxBackground = tr.gameObject;
                        break;
                    case "TextBoxAuthor":
                        textBoxAuthor = tr.gameObject;
                        break;
                    case "TextBoxContent":
                        textBoxContent = tr.gameObject;
                        break;
                    default:
                        break;
                }
            }
        } else
        {
            Debug.LogWarning("Configure a Text Box!");
        }
    }

    private void Update()
    {
        if (active && conversation != null && InputManager.confirm)
        {
            conversation.currentSnippet++;
            if (conversation.hasEnded())
            {
                DisableTextBox();
                Time.timeScale = 1F;
            }
            else
            {
                DisplayConversation();
            }
        }
    }

    private static void DisplayConversation()
    {
        if (audioSource) audioSource.Stop();

        textBoxContent.GetComponent<Text>().text = conversation.getCurrentSnippet().text;
        Debug.Log(textBoxContent.GetComponent<Text>().text);

        if (conversation.getAuthor().Length == 0)
        {
            textBoxAuthor.gameObject.SetActive(false);
        }
        else
        {
            textBoxAuthor.GetComponent<Text>().text = conversation.getAuthor();
        }

        if (conversation.getCurrentSnippet() != null)
        {
            AudioClip vocals = conversation.getCurrentSnippet().vocals;
            if (audioSource && vocals != null)
            {
                audioSource.PlayOneShot(vocals);
            }
        }

    }

    public static void EnableTextBox(Conversation conversation)
    {
        active = true;
        // textBox.GetComponent<Image>().CrossFadeAlpha(100F, 0.1f, false);
        foreach (Transform go in textBox.GetComponentsInChildren<Transform>(true))
        {
            go.gameObject.SetActive(true);
        }
        TextBoxManager.conversation = conversation;
        DisplayConversation();
        textBoxBackground.SetActive(true);
        textBoxContent.SetActive(true);
        textBoxAuthor.SetActive(true);
    }

    private void DisableTextBox()
    {
        textBox.GetComponent<Image>().CrossFadeAlpha(0F, 0.1f, false);
        active = false;
        textBoxBackground.SetActive(false);
        textBoxContent.SetActive(false);
        textBoxAuthor.SetActive(false);
    }

    public static bool isPlayingSound()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}
