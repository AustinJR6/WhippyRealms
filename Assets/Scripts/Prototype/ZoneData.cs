using System.Collections.Generic;

[System.Serializable]
public class ZoneEntry
{
    public string name;
    public int[] levelRange;
    public List<string> enemies;
    public List<string> quests;
}

[System.Serializable]
public class ZoneDatabase
{
    public List<ZoneEntry> zones;
}
