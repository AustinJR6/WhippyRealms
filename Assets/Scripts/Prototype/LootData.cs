using System.Collections.Generic;

[System.Serializable]
public class LootItem
{
    public string item;
    public string rarity;
}

[System.Serializable]
public class LootTable
{
    public List<LootItem> items;
}

[System.Serializable]
public class LootDatabase
{
    public Dictionary<string, List<LootItem>> lootTables;
}
