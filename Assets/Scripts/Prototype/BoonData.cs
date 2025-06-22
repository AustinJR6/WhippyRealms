using System.Collections.Generic;

[System.Serializable]
public class BoonEntry
{
    public string name;
    public string effect;
}

[System.Serializable]
public class BoonDatabase
{
    public List<BoonEntry> boons;
}
