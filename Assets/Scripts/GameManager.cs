using UnityEngine;

/// <summary>
/// Top level manager that loads base game data and coordinates major systems.
/// </summary>
public class GameManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public CombatManager combatManager;
    public ZoneManager zoneManager;
    public InventoryManager inventoryManager;
    public NPCManager npcManager;
    public AICompanion aiCompanion;

    // Loaded data
    public PlayerState playerState;
    public SkillDatabase skillDb;
    public QuestDatabase questDb;
    public DialogueDatabase dialogueDb;
    public ZoneDatabase zoneDb;

    private void Awake()
    {
        // Load JSON data from Resources/Data using JsonLoader
        playerState = JsonLoader.LoadJson<PlayerState>("playerState.json");
        skillDb = JsonLoader.LoadJson<SkillDatabase>("skills.json");
        questDb = JsonLoader.LoadJson<QuestDatabase>("quests.json");
        dialogueDb = JsonLoader.LoadJson<DialogueDatabase>("dialogue.json");
        zoneDb = JsonLoader.LoadJson<ZoneDatabase>("zones.json");

        if (dialogueManager != null)
            dialogueManager.Initialize(dialogueDb);
        if (zoneManager != null)
            zoneManager.Initialize(zoneDb, playerState);
        if (npcManager != null)
        {
            npcManager.aiCompanion = aiCompanion;
            npcManager.dialogueManager = dialogueManager;
            npcManager.LoadNPCs();
        }
    }
}

/// <summary>
/// Simple player state used for the frontend demo.
/// </summary>
[System.Serializable]
public class PlayerState
{
    public int level;
    public int xp;
    public int hp;
    public string zone;
    public string[] inventory;
}
