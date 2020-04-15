using System;

public class Conversation
{
    public string id;
    public int currentSnippet;
    public ConversationSnippet[] snippets;

    public Conversation(string id, ConversationSnippet[] snippets)
    {
        this.id = id;
        this.snippets = snippets;
        currentSnippet = 0;
    }

    public bool hasEnded()
    {
        return currentSnippet > snippets.Length - 1;
    }

    public ConversationSnippet getCurrentSnippet()
    {
        return snippets[currentSnippet];
    }

    public string getAuthor()
    {
        return snippets[currentSnippet].talkerName;
    }
}