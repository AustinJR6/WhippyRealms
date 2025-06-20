using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates and configures a Unity Terrain using data loaded from RegionManager.
/// Terrain elevation, textures and environment effects are chosen based on the
/// region JSON metadata such as elevationType, vegetationType and climateType.
/// </summary>
[RequireComponent(typeof(Terrain))]
public class RegionTerrainManager : MonoBehaviour
{
    public RegionManager regionManager;
    public string regionName;
    public ParticleSystem fogParticles;
    public ParticleSystem rainParticles;
    public ParticleSystem ashParticles;

    private Terrain terrain;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        if (regionManager == null)
            regionManager = FindObjectOfType<RegionManager>();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(regionName))
            ApplyRegion(regionName);
    }

    /// <summary>
    /// Apply terrain generation settings for the given region.
    /// </summary>
    public void ApplyRegion(string name)
    {
        if (terrain == null || regionManager == null)
            return;

        var region = regionManager.GetRegion(name);
        if (region == null)
        {
            Debug.LogWarning($"Region {name} not found");
            return;
        }

        ClearRegion();
        GenerateHeights(region);
        PaintTerrain(region);
        PopulateVegetation(region);
        SpawnWeather(region);
    }

    private void GenerateHeights(RegionData region)
    {
        var data = terrain.terrainData;
        int width = data.heightmapResolution;
        int height = data.heightmapResolution;
        float[,] heights = new float[width, height];

        float scale = 5f;
        float amp = 0.1f;
        if (region.elevationType == "mountains") amp = 0.3f;
        else if (region.elevationType == "plains") amp = 0.05f;
        else if (region.elevationType == "desert") amp = 0.08f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float nx = (float)x / width * scale;
                float ny = (float)y / height * scale;
                heights[y, x] = Mathf.PerlinNoise(nx, ny) * amp;
            }
        }
        data.SetHeights(0, 0, heights);
    }

    private void PaintTerrain(RegionData region)
    {
        // Example texture painting using the first alphamap layer indices.
        // A real implementation would map textures to indices dynamically.
        var data = terrain.terrainData;
        int w = data.alphamapWidth;
        int h = data.alphamapHeight;
        float[,,] maps = new float[w, h, data.alphamapLayers];

        int textureIndex = 0; // default grass
        if (region.climateType == "arid")
            textureIndex = 1; // sand
        else if (region.climateType == "cold")
            textureIndex = 2; // snow
        else if (region.climateType == "lava")
            textureIndex = 3; // lava rock

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                for (int i = 0; i < data.alphamapLayers; i++)
                    maps[x, y, i] = (i == textureIndex) ? 1f : 0f;
            }
        }
        data.SetAlphamaps(0, 0, maps);
    }

    private void SpawnWeather(RegionData region)
    {
        if (fogParticles != null)
            fogParticles.gameObject.SetActive(false);
        if (rainParticles != null)
            rainParticles.gameObject.SetActive(false);
        if (ashParticles != null)
            ashParticles.gameObject.SetActive(false);

        if (region.climateType == "humid" && rainParticles != null)
            rainParticles.gameObject.SetActive(true);
        else if (region.climateType == "arid" && fogParticles != null)
            fogParticles.gameObject.SetActive(true);
        else if (region.climateType == "lava" && ashParticles != null)
            ashParticles.gameObject.SetActive(true);
    }

    /// <summary>
    /// Remove vegetation and reset heights so terrain can be regenerated.
    /// </summary>
    public void ClearRegion()
    {
        var data = terrain.terrainData;
        data.SetHeights(0, 0, new float[data.heightmapResolution, data.heightmapResolution]);
        data.treeInstances = new TreeInstance[0];
        data.SetDetailLayer(0, 0, 0, new int[data.detailWidth, data.detailHeight]);

        foreach (var obj in spawnedObjects)
            if (obj != null)
                Destroy(obj);
        spawnedObjects.Clear();
    }

    /// <summary>
    /// Example vegetation spawning based on biome type. This can be extended
    /// with rules for trees, grass and effects.
    /// </summary>
    private void PopulateVegetation(RegionData region)
    {
        // Placeholder: instantiate prefabs or place trees based on biomeType.
        // For demo purposes we just spawn a simple primitive as an example.
        if (region.biomeType == "forest")
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = terrain.transform.position + Vector3.up;
            obj.transform.SetParent(transform);
            spawnedObjects.Add(obj);
        }
    }
}
