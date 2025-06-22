using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles communication with the OpenAI API for AI-driven NPC dialogue.
/// </summary>
public class AICompanion : MonoBehaviour
{
    public string npcId = "NPC";
    [TextArea]
    public string systemPrompt = "You are a helpful NPC.";
    public DialogueManager dialogueManager;

    private const int MaxHistory = 10;
    private readonly List<string> history = new List<string>();

    /// <summary>
    /// Send player input and context to the OpenAI API.
    /// </summary>
    public void HandlePlayerInput(string input, string location, string quest)
    {
        StartCoroutine(QueryOpenAI(input, location, quest));
    }

    private IEnumerator QueryOpenAI(string input, string location, string quest)
    {
        string apiKey = System.Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("OPENAI_API_KEY not set");
            yield break;
        }

        var messages = new List<Message>
        {
            new Message { role = "system", content = systemPrompt }
        };

        if (!string.IsNullOrEmpty(location) || !string.IsNullOrEmpty(quest))
        {
            var ctx = $"Location: {location}\nQuest: {quest}".Trim();
            messages.Add(new Message { role = "system", content = ctx });
        }

        foreach (var entry in history)
        {
            int idx = entry.IndexOf(':');
            if (idx <= 0) continue;
            string role = entry.StartsWith("Player:") ? "user" : "assistant";
            string content = entry.Substring(idx + 1).Trim();
            messages.Add(new Message { role = role, content = content });
        }

        messages.Add(new Message { role = "user", content = input });
        history.Add("Player:" + input);
        TrimHistory();

        var reqData = new ChatRequest { model = "gpt-3.5-turbo", messages = messages };
        string json = JsonUtility.ToJson(reqData);
        using (var req = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            byte[] body = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("OpenAI request failed: " + req.error);
                yield break;
            }

            var response = JsonUtility.FromJson<ChatResponse>(req.downloadHandler.text);
            if (response != null && response.choices != null && response.choices.Length > 0)
            {
                string content = response.choices[0].message.content.Trim();
                history.Add("NPC:" + content);
                TrimHistory();
                dialogueManager?.DisplayLine(npcId, content);
            }
        }
    }

    private void TrimHistory()
    {
        while (history.Count > MaxHistory)
            history.RemoveAt(0);
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class ChatRequest
    {
        public string model;
        public List<Message> messages;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class ChatResponse
    {
        public Choice[] choices;
    }
}
