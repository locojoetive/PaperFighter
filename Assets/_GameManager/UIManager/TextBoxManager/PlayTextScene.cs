using UnityEngine;

public class PlayTextScene : MonoBehaviour
{
    public string scene;

    void Start()
    {
        Conversation conversation = TextImporter.textFileToConversation(scene);
        FindObjectOfType<TextBoxManager>().EnableTextBox(conversation);
    }

    private void Update()
    {
        StageManager.hasTextSceneEnded = TextBoxManager.conversation.hasEnded();
    }
}
