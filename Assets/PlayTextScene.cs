using UnityEngine;

public class PlayTextScene : MonoBehaviour
{
    public string scene;

    void Start()
    {
        Conversation conversation = TextImporter.textFileToConversation(scene);
        TextBoxManager.EnableTextBox(conversation);
    }

    private void Update()
    {
        StageManager.hasTextSceneEnded = TextBoxManager.conversation.hasEnded();
    }
}
