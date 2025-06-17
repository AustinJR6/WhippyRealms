using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Keeps track of procedural zones that have been generated. Each entry stores
/// a seed, an entrance position and whether the zone was cleared. Data is
/// persisted to JSON so the world remains consistent across play sessions.
/// </summary>
public class WorldMemory : MonoBehaviour
{
    [System.Serializable]
    public class ZoneInfo
    {
        public string zoneName;
        public int seed;
        public Vector3 entrance;
        public bool cleared;
    }

    [SerializeField]
    private List<ZoneInfo> zones = new List<ZoneInfo>();

    private string SavePath => Path.Combine(Application.persistentDataPath, "worldmemory.json");

    public ZoneInfo GetZone(string name)
    {
        return zones.Find(z => z.zoneName == name);
    }

    public void RegisterZone(string name, int seed, Vector3 entrance)
    {
        var zone = GetZone(name);
        if (zone == null)
        {
            zone = new ZoneInfo { zoneName = name, seed = seed, entrance = entrance, cleared = false };
            zones.Add(zone);
        }
        else
        {
            zone.seed = seed;
            zone.entrance = entrance;
        }
    }

    public void MarkCleared(string name)
    {
        var zone = GetZone(name);
        if (zone != null)
            zone.cleared = true;
    }

    public void Save()
    {
        var json = JsonUtility.ToJson(new ZoneCollection { zones = zones }, true);
        File.WriteAllText(SavePath, json);
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            var json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<ZoneCollection>(json);
            zones = data.zones ?? new List<ZoneInfo>();
        }
    }

    [System.Serializable]
    private class ZoneCollection
    {
        public List<ZoneInfo> zones;
    }
}
