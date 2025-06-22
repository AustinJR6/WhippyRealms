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
    public List<PrefabEntry> prefabLibrary = new List<PrefabEntry>();

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
        if (region.prefabs != null)
        {
            for (int i = 0; i < region.prefabs.Length; i++)
            {
                SpawnByName(region.prefabs[i], i);
            }
        }
    }

    private void SpawnByName(string prefabName, int seed)
    {
        var entry = prefabLibrary.Find(p => p.key == prefabName);
        GameObject prefab = null;
        float scale = 1f;
        if (entry != null)
        {
            prefab = entry.prefab;
            scale = entry.scale > 0 ? entry.scale : 1f;
        }
        if (prefab == null)
            prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning($"Prefab {prefabName} not found");
            return;
        }

        Vector3 pos = anchors.Count > seed ? anchors[seed].position : SamplePosition(seed);
        var obj = Instantiate(prefab, pos, Quaternion.identity, transform);
        obj.transform.localScale *= scale;
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
