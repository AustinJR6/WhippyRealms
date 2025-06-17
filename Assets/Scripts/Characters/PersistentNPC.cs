using UnityEngine;

/// <summary>
/// Provides a unique identifier for NPCs and automatically loads/saves their
/// memory to disk.
/// </summary>
[RequireComponent(typeof(NPCMemory))]
public class PersistentNPC : MonoBehaviour
{
    public string npcId = System.Guid.NewGuid().ToString();
    private NPCMemory memory;

    private string SavePath => System.IO.Path.Combine(Application.persistentDataPath, $"npc_{npcId}.json");

    private void Awake()
    {
        memory = GetComponent<NPCMemory>();
        LoadMemory();
    }

    public void SaveMemory()
    {
        var json = JsonUtility.ToJson(memory, true);
        System.IO.File.WriteAllText(SavePath, json);
    }

    private void LoadMemory()
    {
        if (System.IO.File.Exists(SavePath))
        {
            var json = System.IO.File.ReadAllText(SavePath);
            JsonUtility.FromJsonOverwrite(json, memory);
        }
    }
}
