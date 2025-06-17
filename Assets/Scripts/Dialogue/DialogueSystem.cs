using UnityEngine;

/// <summary>
/// Simple dialogue system that can display lines of dialogue and track basic
/// conversational state. This is where AI-generated dialogue can be plugged in.
/// </summary>
public class DialogueSystem : MonoBehaviour
{ 
    public void Say(string speaker, string line)
    {
        Debug.Log($"{speaker}: {line}");
    }

    /// <summary>
    /// Say a line influenced by memory state of the speaker.
    /// </summary>
    public void Say(string speaker, string line, AIMemory memory)
    {
        string prefix = string.Empty;

        if (memory is CompanionMemory cm)
        {
            if (cm.disposition.TryGetValue("rebellious", out float reb) && reb > 5f)
                prefix = "[Rebellious] ";
            else if (cm.disposition.TryGetValue("loyal", out float loy) && loy > 5f)
                prefix = "[Loyal] ";
        }
        else if (memory is NPCMemory npc)
        {
            if (npc.reputation < -10)
                prefix = "[Hostile] ";
            else if (npc.reputation > 10)
                prefix = "[Friendly] ";
        }

        Debug.Log($"{speaker}: {prefix}{line}");
    }
}
