using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Loads region data from a JSON file and provides access to regions by name.
/// The JSON file should be placed under StreamingAssets/regions.json.
/// </summary>
public class RegionManager : MonoBehaviour
{
    public string regionsFile = "regions.json";
    public List<RegionData> regions = new List<RegionData>();

    private void Awake()
    {
        LoadRegions();
    }

    /// <summary>
    /// Loads region definitions from StreamingAssets.
    /// </summary>
    public void LoadRegions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, regionsFile);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Region file not found at {path}");
            return;
        }

        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<RegionCollection>(json);
        if (wrapper != null && wrapper.regions != null)
            regions = wrapper.regions;
    }

    /// <summary>
    /// Get region data by name.
    /// </summary>
    public RegionData GetRegion(string name)
    {
        return regions.Find(r => r.name == name);
    }

    [System.Serializable]
    private class RegionCollection
    {
        public List<RegionData> regions;
    }
}
