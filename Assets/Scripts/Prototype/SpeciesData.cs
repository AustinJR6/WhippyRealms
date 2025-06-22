using System.Collections.Generic;

[System.Serializable]
public class SpeciesEntry
{
    public string name;
    public StatBlock baseStats;
}

[System.Serializable]
public class SpeciesDatabase
{
    public List<SpeciesEntry> species;
}
