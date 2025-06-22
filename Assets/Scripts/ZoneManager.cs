using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Controls zone transitions and displays flavor text.
/// </summary>
public class ZoneManager : MonoBehaviour
{
    public Text logText;
    public LogManager logManager;
    private PlayerState player;
    private Dictionary<string, ZoneEntry> zones = new Dictionary<string, ZoneEntry>();

    public void Initialize(ZoneDatabase db, PlayerState state)
    {
        player = state;
        zones.Clear();
        foreach (var z in db.zones)
            zones[z.name] = z;
    }

    public void TravelTo(string zoneName)
    {
        if (zones.ContainsKey(zoneName))
        {
            player.zone = zoneName;
            Append($"You travel to {zoneName}.");
        }
        else
        {
            Append($"Unknown zone: {zoneName}");
        }
    }

    public void ShowStats()
    {
        Append($"Level {player.level} HP {player.hp} XP {player.xp}");
    }

    private void Append(string msg)
    {
        if (logManager != null)
        {
            logManager.Log(msg);
            return;
        }

        if (logText != null)
            logText.text += msg + "\n";
        else
            Debug.Log(msg);
    }
}

[System.Serializable]
public class ZoneEntry
{
    public string name;
    public int level;
    public List<string> connectsTo;
}

[System.Serializable]
public class ZoneDatabase
{
    public List<ZoneEntry> zones;
}
