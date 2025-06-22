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
    public LogManager logManager;
    public UIManager uiManager;

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
        {
            dialogueManager.logManager = logManager;
            dialogueManager.Initialize(dialogueDb);
        }
        if (zoneManager != null)
        {
            zoneManager.logManager = logManager;
            zoneManager.Initialize(zoneDb, playerState);
        }
        if (combatManager != null)
        {
            combatManager.logManager = logManager;
            combatManager.Initialize(playerState);
        }
        if (inventoryManager != null)
        {
            inventoryManager.logManager = logManager;
            inventoryManager.Initialize(playerState);
        }
        if (npcManager != null)
        {
            npcManager.aiCompanion = aiCompanion;
            npcManager.dialogueManager = dialogueManager;
            npcManager.LoadNPCs();
        }

        zoneManager?.TravelTo(playerState.zone);
        logManager?.Log($"Welcome to {playerState.zone}.");
        uiManager?.UpdateHUD(playerState);
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
