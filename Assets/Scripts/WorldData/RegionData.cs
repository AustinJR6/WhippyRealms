using UnityEngine;

/// <summary>
/// Describes a world region and its basic properties.
/// </summary>
[System.Serializable]
public class RegionData
{
    public string name;
    [TextArea]
    public string description;
    public string deity;
    public int minLevel;
    public int maxLevel;
    public string elevationType;
    public string biomeType;
    public string vegetationType;
    public string climateType;
    public string cityType;
    public string shrineType;
    public string wonderType;
    public string[] terrainFeatures;
    public string tag;
    public int levelZone;
}
