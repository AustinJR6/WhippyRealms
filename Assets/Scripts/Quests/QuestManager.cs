using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks active quests and distributes updates to systems interested in quest
/// state changes.
/// </summary>
public class QuestManager : MonoBehaviour
{
    private readonly List<Quest> activeQuests = new List<Quest>();

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
        }
    }

    public IEnumerable<Quest> GetActiveQuests() => activeQuests;
}
