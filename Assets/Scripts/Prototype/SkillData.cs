using System.Collections.Generic;

[System.Serializable]
public class SkillEntry
{
    public string name;
    public int damage;
    public int heal;
    public string type;
    public string effect;
}

[System.Serializable]
public class SkillDatabase
{
    public List<SkillEntry> skills;
}
