using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns creatures based on region difficulty and biome settings.
/// Common creatures spawn regularly while elite or mythic creatures have
/// conditional spawn logic using the region metadata.
/// </summary>
public class BiomeCreatureSpawner : MonoBehaviour
{
    public RegionData region;
    public List<GameObject> commonCreatures = new List<GameObject>();
    public List<GameObject> eliteCreatures = new List<GameObject>();
    public List<GameObject> mythicCreatures = new List<GameObject>();

    public float spawnRadius = 20f;
    public float spawnInterval = 10f;
    public int maxSpawned = 5;
    public float respawnDelay = 5f;
    public float leashDistance = 30f;
    public List<Transform> wonderPoints = new List<Transform>();

    private float timer;
    private readonly List<GameObject> spawned = new List<GameObject>();

    private void Update()
    {
        if (region == null)
            return;

        spawned.RemoveAll(obj => obj == null);
        timer += Time.deltaTime;
        if (timer >= spawnInterval && spawned.Count < maxSpawned)
        {
            SpawnCreature();
            timer = 0f;
        }
    }

    private void SpawnCreature()
    {
        GameObject prefab = ChooseCreature();
        if (prefab == null)
            return;

        Vector3 pos = GetSpawnPosition(prefab);
        var obj = Instantiate(prefab, pos, Quaternion.identity, transform);
        var leash = obj.AddComponent<SpawnLeash>();
        leash.center = transform;
        leash.maxDistance = leashDistance;
        spawned.Add(obj);
    }

    private GameObject ChooseCreature()
    {
        if (region.levelZone >= 25 && mythicCreatures.Count > 0 && Random.value > 0.95f)
            return mythicCreatures[Random.Range(0, mythicCreatures.Count)];

        if (region.levelZone >= 25 && eliteCreatures.Count > 0)
            return eliteCreatures[Random.Range(0, eliteCreatures.Count)];

        if (commonCreatures.Count > 0)
            return commonCreatures[Random.Range(0, commonCreatures.Count)];

        return null;
    }

    private Vector3 GetSpawnPosition(GameObject prefab)
    {
        Vector3 basePos = transform.position;
        if (prefab != null && wonderPoints.Count > 0 &&
            (eliteCreatures.Contains(prefab) || mythicCreatures.Contains(prefab)))
        {
            var anchor = wonderPoints[Random.Range(0, wonderPoints.Count)];
            basePos = anchor.position;
        }

        basePos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f,
                               Random.Range(-spawnRadius, spawnRadius));

        if (Terrain.activeTerrain != null)
        {
            Vector3 local = basePos - Terrain.activeTerrain.transform.position;
            float nx = local.x / Terrain.activeTerrain.terrainData.size.x;
            float nz = local.z / Terrain.activeTerrain.terrainData.size.z;
            float h = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(nx, nz);
            basePos.y = h + Terrain.activeTerrain.transform.position.y;
        }

        return basePos;
    }
}
