using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;
    public GameObject textBoxBackground;
    public GameObject textBoxAuthor;
    public GameObject textBoxContent;
    public bool active = false;

    public Conversation conversation;


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

    private void DisplayConversation()
    {
        textBoxContent.GetComponent <TextMeshProUGUI>().SetText(conversation.getCurrentSnippet().text);

        if (conversation.getAuthor().Length == 0)
        {
            textBoxAuthor.gameObject.SetActive(false);
        }
        else
        {
            textBoxAuthor.GetComponent<TextMeshProUGUI>().SetText(conversation.getAuthor() + ":");
        }
    }

    public void EnableTextBox(Conversation conversation)
    {
        active = true;
        textBoxBackground.SetActive(true);
        textBoxContent.SetActive(true);
        textBoxAuthor.SetActive(true);
        this.conversation = conversation;
        DisplayConversation();
    }

    private void DisableTextBox()
    {
        textBoxBackground.GetComponent<Image>().CrossFadeAlpha(0F, 0.1f, false);
        active = false;
    }
}
