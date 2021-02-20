using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    private static GameObject textBox;
    private static GameObject textBoxBackground;
    private static GameObject textBoxAuthor;
    private static GameObject textBoxContent;
    private static AudioSource audioSource;
    private static bool active = false;

    public static Conversation conversation;


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

    public void EnableTextBox(Conversation conversation)
    {
        active = true;
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        textBox = GameObject.FindGameObjectWithTag("TextBox");
        if (textBox != null)
        {

            Transform[] children = textBox.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                switch (child.gameObject.tag)
                {
                    case "TextBoxBackground":
                        textBoxBackground = child.gameObject;
                        break;
                    case "TextBoxAuthor":
                        textBoxAuthor = child.gameObject;
                        break;
                    case "TextBoxContent":
                        textBoxContent = child.gameObject;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Configure a Text Box!");
        }
        textBoxBackground.SetActive(true);
        textBoxContent.SetActive(true);
        textBoxAuthor.SetActive(true);
        TextBoxManager.conversation = conversation;
        DisplayConversation();
    }

    private void DisableTextBox()
    {
        textBox.GetComponent<Image>().CrossFadeAlpha(0F, 0.1f, false);
        active = false;
        textBoxBackground.SetActive(false);
        textBoxContent.SetActive(false);
        textBoxAuthor.SetActive(false);
    }
}
