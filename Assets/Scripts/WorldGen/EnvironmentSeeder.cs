using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Places environment prefabs such as cities, shrines and ruins based on region
/// data. Placement can use predefined anchor points or simple Perlin noise
/// distribution. Objects are parented under this component for easy cleanup when
/// the region unloads.
/// </summary>
public class EnvironmentSeeder : MonoBehaviour
{
    public RegionManager regionManager;
    public string regionName;

    [Header("Prefabs")]
    public List<GameObject> cityPrefabs = new List<GameObject>();
    public List<GameObject> shrinePrefabs = new List<GameObject>();
    public List<GameObject> wonderPrefabs = new List<GameObject>();
    public List<GameObject> ruinPrefabs = new List<GameObject>();

    [Tooltip("Optional anchor points inside the terrain where prefabs may spawn.")]
    public List<Transform> anchors = new List<Transform>();

    public float perlinScale = 10f;

    private void Start()
    {
        if (regionManager == null)
            regionManager = FindObjectOfType<RegionManager>();
        SeedRegion(regionName);
    }

    /// <summary>
    /// Spawns prefabs appropriate for the region at anchor points or
    /// procedurally generated positions.
    /// </summary>
    public void SeedRegion(string name)
    {
        if (regionManager == null || string.IsNullOrEmpty(name))
            return;

        var region = regionManager.GetRegion(name);
        if (region == null)
            return;

        SpawnSet(cityPrefabs, 1);
        SpawnSet(shrinePrefabs, 2);
        SpawnSet(wonderPrefabs, 3);
        SpawnSet(ruinPrefabs, 4);
    }

    private void SpawnSet(List<GameObject> prefabs, int seedOffset)
    {
        if (prefabs == null || prefabs.Count == 0)
            return;

        int count = anchors.Count > 0 ? anchors.Count : 3;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = anchors.Count > i ? anchors[i].position : GetProceduralPosition(i + seedOffset);
            var prefab = prefabs[Random.Range(0, prefabs.Count)];
            Instantiate(prefab, pos, Quaternion.identity, transform);
        }
    }

    private Vector3 GetProceduralPosition(int seed)
    {
        float x = Mathf.PerlinNoise(seed, 0f) * 50f - 25f;
        float z = Mathf.PerlinNoise(0f, seed) * 50f - 25f;
        return transform.position + new Vector3(x, 0f, z);
    }
}
