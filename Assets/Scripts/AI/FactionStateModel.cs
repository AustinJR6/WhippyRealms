using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Keeps track of faction relationships and reputations. Factions respond to
/// player actions and world events.
/// </summary>
public class FactionStateModel : MonoBehaviour
{
    private readonly Dictionary<string, int> factionReputation = new Dictionary<string, int>();

    public int GetReputation(string faction)
    {
        factionReputation.TryGetValue(faction, out int rep);
        return rep;
    }

    public void ModifyReputation(string faction, int delta)
    {
        if (!factionReputation.ContainsKey(faction))
            factionReputation[faction] = 0;
        factionReputation[faction] += delta;
    }
}
