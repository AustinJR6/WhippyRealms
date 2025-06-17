using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a quest with objectives and rewards. This is intentionally simple
/// and will be expanded with faction impact, branching paths, etc.
/// </summary>
[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public List<QuestObjective> objectives = new List<QuestObjective>();

    public bool IsComplete => objectives.TrueForAll(o => o.isComplete);
}
