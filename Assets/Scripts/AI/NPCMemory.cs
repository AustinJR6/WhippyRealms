using UnityEngine;
using System;

/// <summary>
/// Memory data for NPCs persisting simple interaction state.
/// </summary>
public class NPCMemory : AIMemory
{
    public DateTime lastInteraction;
    public int reputation;
    public string lastDialogueOption;
    public string previousDialogueOption;
    public readonly System.Collections.Generic.HashSet<string> flags = new System.Collections.Generic.HashSet<string>();

    public void RecordInteraction(string dialogueOption, int reputationDelta)
    {
        previousDialogueOption = lastDialogueOption;
        lastDialogueOption = dialogueOption;
        reputation += reputationDelta;
        lastInteraction = DateTime.UtcNow;
        Remember($"Spoke: {dialogueOption}");
    }

    public void SetFlag(string flag)
    {
        flags.Add(flag);
    }
}
