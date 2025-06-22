using System.Collections.Generic;

[System.Serializable]
public class QuestObjectiveData
{
    public string type;
    public string target;
    public int count;
}

[System.Serializable]
public class QuestEntryData
{
    public string name;
    public List<QuestObjectiveData> objectives;
    public List<string> rewards;
}

[System.Serializable]
public class QuestDatabase
{
    public List<QuestEntryData> quests;
}
