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
    private float timer;

    private void Update()
    {
        if (region == null)
            return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
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

        Vector3 pos = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f,
                                                        Random.Range(-spawnRadius, spawnRadius));
        Instantiate(prefab, pos, Quaternion.identity, transform);
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
}
