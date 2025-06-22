using System.Collections.Generic;

[System.Serializable]
public class CreatureEntry
{
    public string name;
    public int level;
    public StatBlock stats;
    public List<string> skills;
}

[System.Serializable]
public class CreatureDatabase
{
    public List<CreatureEntry> creatures;
}
