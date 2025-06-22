using System.Collections.Generic;

[System.Serializable]
public class RecipeEntry
{
    public string name;
    public Dictionary<string, int> materials;
    public string resultItem;
    public int resultQuantity;
}

[System.Serializable]
public class RecipeDatabase
{
    public List<RecipeEntry> recipes;
}
