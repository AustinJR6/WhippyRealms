using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Instantiates region prefabs such as cities, shrines and wonders using
/// region metadata. Prefabs can be placed at anchor points or sampled using
/// Perlin noise for a procedural feel.
/// </summary>
public class RegionPrefabPlacer : MonoBehaviour
{
    public RegionManager regionManager;
    public string regionName;

    [Header("Anchors")] public List<Transform> anchors = new List<Transform>();

    [Header("Prefab Libraries")]
    public List<PrefabEntry> cityPrefabs = new List<PrefabEntry>();
    public List<PrefabEntry> shrinePrefabs = new List<PrefabEntry>();
    public List<PrefabEntry> wonderPrefabs = new List<PrefabEntry>();
    public List<PrefabEntry> ruinPrefabs = new List<PrefabEntry>();

    private readonly List<GameObject> spawned = new List<GameObject>();

    private void Start()
    {
        if (regionManager == null) regionManager = FindObjectOfType<RegionManager>();
        if (!string.IsNullOrEmpty(regionName)) PlacePrefabs(regionName);
    }

    /// <summary>
    /// Destroy all spawned prefabs so the region can be unloaded cleanly.
    /// </summary>
    public void ClearPrefabs()
    {
        foreach (var obj in spawned)
            if (obj != null)
                Destroy(obj);
        spawned.Clear();
    }

    /// <summary>
    /// Create prefabs for the specified region.
    /// </summary>
    public void PlacePrefabs(string name)
    {
        if (regionManager == null) return;
        var region = regionManager.GetRegion(name);
        if (region == null) return;

        ClearPrefabs();
        SpawnFromTable(cityPrefabs, region.cityType, 0);
        SpawnFromTable(shrinePrefabs, region.shrineType, 1, true);
        SpawnFromTable(wonderPrefabs, region.wonderType, 2);
        SpawnFromTable(ruinPrefabs, "default", 3);
    }

    private void SpawnFromTable(List<PrefabEntry> table, string key, int seed, bool alignToTerrain = false)
    {
        if (table == null || table.Count == 0) return;
        var entry = table.Find(p => p.key == key);
        if (entry == null) entry = table[0];
        var pos = anchors.Count > seed ? anchors[seed].position : SamplePosition(seed);
        var obj = Instantiate(entry.prefab, pos, Quaternion.identity, transform);
        if (alignToTerrain && Terrain.activeTerrain != null)
        {
            var normal = Terrain.activeTerrain.terrainData.GetInterpolatedNormal(pos.x, pos.z);
            obj.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(obj.transform.forward, normal), normal);
        }
        if (entry.scale > 0f) obj.transform.localScale *= entry.scale;
        spawned.Add(obj);
    }

    private Vector3 SamplePosition(int seed)
    {
        float x = Mathf.PerlinNoise(seed, 0f) * 50f - 25f;
        float z = Mathf.PerlinNoise(0f, seed) * 50f - 25f;
        return transform.position + new Vector3(x, 0f, z);
    }

    [System.Serializable]
    public class PrefabEntry
    {
        public string key;
        public GameObject prefab;
        public float scale = 1f;
    }
}
