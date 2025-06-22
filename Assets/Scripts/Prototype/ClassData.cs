using System.Collections.Generic;

[System.Serializable]
public class ClassEntry
{
    public string name;
    public StatBlock stats;
    public List<string> skills;
}

[System.Serializable]
public class ClassDatabase
{
    public List<ClassEntry> classes;
}
