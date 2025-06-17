using UnityEngine;

/// <summary>
/// AI behaviour for a companion character that follows the player and reacts to
/// the world. This script is intentionally simple and should be expanded with
/// more advanced behaviours and state machines.
/// </summary>
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class CompanionAI : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f;
    public CompanionMemory memory;

    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (memory == null)
            memory = GetComponent<CompanionMemory>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }

    public void ReactToEvent(string eventType)
    {
        memory.AddEvent(eventType);
    }

    public void CommentOnQuest(string questId)
    {
        Debug.Log($"Companion comments on quest {questId}");
    }

    public void AdjustDisposition(string trait, float amount)
    {
        memory.AdjustDisposition(trait, amount);
    }
}
