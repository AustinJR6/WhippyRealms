using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for AI memory. Stores key events and decisions so AI characters
/// can remember the player's actions over time.
/// </summary>
public class AIMemory : MonoBehaviour
{
    protected readonly List<string> memoryLog = new List<string>();

    protected void Remember(string entry)
    {
        memoryLog.Add(entry);
        if (memoryLog.Count > 100)
            memoryLog.RemoveAt(0);
    }

    public IEnumerable<string> Recall() => memoryLog;
}
