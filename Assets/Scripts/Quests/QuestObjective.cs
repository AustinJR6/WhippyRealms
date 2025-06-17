using UnityEngine;

/// <summary>
/// Single objective within a quest. Could be kill, collect, explore, etc.
/// </summary>
[System.Serializable]
public class QuestObjective
{
    public string description;
    public bool isComplete;
}
