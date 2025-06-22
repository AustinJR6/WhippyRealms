using System.Linq;
using UnityEngine;

public class PrototypeGame : MonoBehaviour
{
    public Player player = new Player();

    private SpeciesDatabase speciesDb;
    private ClassDatabase classDb;
    private SkillDatabase skillDb;
    private CreatureDatabase creatureDb;
    private LootDatabase lootDb;
    private ZoneDatabase zoneDb;

    private void Start()
    {
        speciesDb = DataLoader.LoadJson<SpeciesDatabase>("species.json");
        classDb = DataLoader.LoadJson<ClassDatabase>("classes.json");
        skillDb = DataLoader.LoadJson<SkillDatabase>("skills.json");
        creatureDb = DataLoader.LoadJson<CreatureDatabase>("creatures.json");
        lootDb = DataLoader.LoadJson<LootDatabase>("loot.json");
        zoneDb = DataLoader.LoadJson<ZoneDatabase>("zones.json");

        InitializePlayer("Human", "Bloomward Druid");
        EnterZone("Thornroot Paths");
    }

    private void InitializePlayer(string species, string clazz)
    {
        player.species = species;
        player.playerClass = clazz;

        var sp = speciesDb.species.First(s => s.name == species);
        player.stats = sp.baseStats.Clone();

        var cl = classDb.classes.First(c => c.name == clazz);
        player.stats.Add(cl.stats);
        player.skills.AddRange(cl.skills);

        Debug.Log($"Created {species} {clazz} with {player.stats.health} HP.");
    }

    private void EnterZone(string zoneName)
    {
        player.zone = zoneName;
        var zone = zoneDb.zones.First(z => z.name == zoneName);
        Debug.Log($"Entered {zoneName}");
        SpawnEnemy(zone.enemies[0]);
    }

    private void SpawnEnemy(string enemyName)
    {
        var enemy = creatureDb.creatures.First(c => c.name == enemyName);
        CombatSystem.EngageCombat(player, enemy, skillDb, lootDb);
    }
}
