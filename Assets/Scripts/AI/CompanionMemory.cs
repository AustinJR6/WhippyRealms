using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tracks events shared with the player and emotional state for a companion.
/// </summary>
public class CompanionMemory : AIMemory
{
    [System.Serializable]
    public class EventEntry
    {
        public string description;
        public float timestamp;
    }

    public List<EventEntry> sharedEvents = new List<EventEntry>();
    public int bondLevel = 50; // 0-100
    public Dictionary<string, float> disposition = new Dictionary<string, float>
    {
        {"loyal", 0f},
        {"cautious", 0f},
        {"rebellious", 0f},
        {"curious", 0f}
    };

    public void AddEvent(string desc)
    {
        sharedEvents.Add(new EventEntry { description = desc, timestamp = Time.time });
        Remember(desc);
    }

    public void AdjustBond(int amount)
    {
        bondLevel = Mathf.Clamp(bondLevel + amount, 0, 100);
    }

    public void AdjustDisposition(string trait, float amount)
    {
        if (!disposition.ContainsKey(trait))
            disposition[trait] = 0f;
        disposition[trait] += amount;
    }
}
