using UnityEngine;

/// <summary>
/// Example stub for integrating with a GPT-based backend to generate dynamic
/// quests. In production this would make API calls and parse responses.
/// </summary>
public class GPTQuestEngine : MonoBehaviour
{
    [Tooltip("API key for GPT service")] public string apiKey;

    public Quest GenerateQuestFromPrompt(string prompt)
    {
        Debug.Log($"Generating quest from prompt: {prompt}");
        // TODO: Call into GPT model and construct a Quest
        return new Quest { questName = "Generated Quest", description = prompt };
    }
}
