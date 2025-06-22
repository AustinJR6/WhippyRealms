using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Loads NPC definitions and manages interactions with NPCs. AI-enabled NPCs
/// can route dialogue through the AICompanion component.
/// </summary>
public class NPCManager : MonoBehaviour
{
    public string npcFile = "npcs.json";
    public AICompanion aiCompanion;
    public DialogueManager dialogueManager;

    private readonly Dictionary<string, NPCEntry> lookup = new Dictionary<string, NPCEntry>();

    private void Awake()
    {
        LoadNPCs();
    }

    public void LoadNPCs()
    {
        lookup.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, npcFile);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"NPC file not found at {path}");
            return;
        }

        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<NPCCollection>(json);
        if (wrapper == null || wrapper.npcs == null)
            return;

        foreach (var npc in wrapper.npcs)
            lookup[npc.id] = npc;
    }

    public bool IsAI(string id)
    {
        return lookup.TryGetValue(id, out var npc) && npc.isAI;
    }

    public string GetSystemPrompt(string id)
    {
        if (lookup.TryGetValue(id, out var npc))
        {
            return !string.IsNullOrEmpty(npc.personality) ? npc.personality : npc.systemPrompt;
        }
        return string.Empty;
    }

    public string GetName(string id)
    {
        return lookup.TryGetValue(id, out var npc) ? npc.name : id;
    }

    public void TalkTo(string id, string playerInput, string location, string quest)
    {
        if (!lookup.TryGetValue(id, out var npc))
        {
            dialogueManager?.DisplayLine("System", $"No NPC named {id}");
            return;
        }

        if (npc.isAI && aiCompanion != null)
        {
            aiCompanion.systemPrompt = !string.IsNullOrEmpty(npc.personality) ? npc.personality : npc.systemPrompt;
            aiCompanion.npcId = npc.name;
            aiCompanion.dialogueManager = dialogueManager;
            aiCompanion.HandlePlayerInput(playerInput, location, quest);
        }
        else
        {
            dialogueManager?.StartDialogue(npc.dialogueRoot);
        }
    }

    [System.Serializable]
    public class NPCEntry
    {
        public string id;
        public string name;
        public string location;
        public string dialogueRoot;
        public bool isAI;
        [TextArea]
        public string systemPrompt;
        [TextArea]
        public string personality;
    }

    [System.Serializable]
    private class NPCCollection
    {
        public List<NPCEntry> npcs;
    }
}
