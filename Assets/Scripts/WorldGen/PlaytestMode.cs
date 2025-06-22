using UnityEngine;

/// <summary>
/// Simple scene controller for playtesting regions. Allows switching
/// between regions with hotkeys and teleports the player to the start
/// position when a region is loaded.
/// </summary>
public class PlaytestMode : MonoBehaviour
{
    public RegionManager regionManager;
    public RegionTerrainManager terrainManager;
    public RegionPrefabPlacer prefabPlacer;
    public BiomeCreatureSpawner creatureSpawner;
    public PlayerController player;

    private void Start()
    {
        if (regionManager == null) regionManager = FindObjectOfType<RegionManager>();
        if (terrainManager == null) terrainManager = FindObjectOfType<RegionTerrainManager>();
        if (prefabPlacer == null) prefabPlacer = FindObjectOfType<RegionPrefabPlacer>();
        if (creatureSpawner == null) creatureSpawner = FindObjectOfType<BiomeCreatureSpawner>();
        if (player == null) player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadRegion("Aurelia");
        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadRegion("Verdancia");
        if (Input.GetKeyDown(KeyCode.Alpha3)) LoadRegion("Durndara");
        if (Input.GetKeyDown(KeyCode.Alpha4)) LoadRegion("Selenora");
        if (Input.GetKeyDown(KeyCode.Alpha5)) LoadRegion("Tharnor");
        if (Input.GetKeyDown(KeyCode.Alpha6)) LoadRegion("Vokaria");

        if (Input.GetKeyDown(KeyCode.R)) ReloadCurrent();
        if (Input.GetKeyDown(KeyCode.T)) TeleportTo(Vector3.zero);
    }

    private string current;

    private void LoadRegion(string name)
    {
        current = name;
        if (terrainManager != null) terrainManager.ApplyRegion(name);
        if (prefabPlacer != null) prefabPlacer.PlacePrefabs(name);
        var reg = regionManager.GetRegion(name);
        if (creatureSpawner != null) creatureSpawner.region = reg;
        if (player != null) player.transform.position = Vector3.up * 2f;
    }

    private void ReloadCurrent()
    {
        if (!string.IsNullOrEmpty(current)) LoadRegion(current);
    }

    private void TeleportTo(Vector3 pos)
    {
        if (player != null) player.transform.position = pos + Vector3.up * 2f;
    }
}
