using UnityEngine;

/// <summary>
/// Detects player interaction with an AI-enabled NPC and forwards the
/// conversation to the NPCManager.
/// </summary>
public class AITrigger : MonoBehaviour
{
    public string npcId;
    public NPCManager npcManager;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            npcManager?.TalkTo(npcId, "Hello", string.Empty, string.Empty);
        }
    }
}
