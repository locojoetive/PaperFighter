using UnityEngine;

public class PlayTextScene : MonoBehaviour
{
    private TextBoxManager textBoxManager;
    public string scene;
    [SerializeField] private Conversation conversation;

    void Start()
    {
        ConversationSnippet[] snippets = new ConversationSnippet[2];
        if (scene == "intro")
        {
            snippets[0] = new ConversationSnippet();
            snippets[0].id = 0;
            snippets[0].talkerName = "Ninja King";
            snippets[0].text = "Young Ninja!\n Kung Frog and his Minions are threatening Ninja land.";

            snippets[1] = new ConversationSnippet();
            snippets[1].id = 1;
            snippets[1].talkerName = "Ninja King";
            snippets[1].text = "You were chosen to assassinate him before he can execute his malicious plans!";

            conversation = new Conversation("intro", snippets);
        }
        else if (scene == "outro")
        {
            snippets[0] = new ConversationSnippet();
            snippets[0].id = 0;
            snippets[0].talkerName = "Ninja King";
            snippets[0].text = "Congratulations!\n You saved Ninja land and all his inhabitants!";

            snippets[1] = new ConversationSnippet();
            snippets[1].id = 1;
            snippets[1].talkerName = "Ninja King";
            snippets[1].text = "We are proud of you little Ninja.";

            conversation = new Conversation("outro", snippets);
        }


        textBoxManager = FindObjectOfType<TextBoxManager>();
        textBoxManager.EnableTextBox(conversation);
    }

    private void Update()
    {
        StageManager.hasTextSceneEnded = textBoxManager.conversation.hasEnded();
    }
}
