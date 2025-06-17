using UnityEngine;

/// <summary>
/// Placeholder procedural quest generator. In the future this will integrate
/// with GPTQuestEngine to produce dynamic quests.
/// </summary>
public class QuestGenerator : MonoBehaviour
{
    public Quest GenerateSimpleQuest(string questName)
    {
        Quest quest = new Quest
        {
            questName = questName,
            description = $"Complete the task: {questName}"
        };
        quest.objectives.Add(new QuestObjective { description = "Talk to the elder" });
        return quest;
    }
}
