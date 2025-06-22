using System.Linq;
using UnityEngine;

public static class CombatSystem
{
    public static void EngageCombat(Player player, CreatureEntry enemy, SkillDatabase skills, LootDatabase loot)
    {
        Debug.Log($"Encounter! {enemy.name} appears.");
        int playerHP = player.stats.health;
        int enemyHP = enemy.stats.health;
        while (playerHP > 0 && enemyHP > 0)
        {
            // Player turn using first skill
            SkillEntry playerSkill = skills.skills.First(s => s.name == player.skills[0]);
            enemyHP -= playerSkill.damage + player.stats.attack;
            Debug.Log($"Player uses {playerSkill.name} for {playerSkill.damage + player.stats.attack} damage. Enemy HP: {enemyHP}");
            if (enemyHP <= 0) break;

            // Enemy turn using first skill
            SkillEntry enemySkill = skills.skills.First(s => s.name == enemy.skills[0]);
            int damage = enemySkill.damage + enemy.stats.attack;
            if (!player.godMode)
                playerHP -= damage;
            Debug.Log($"{enemy.name} hits for {damage}. Player HP: {playerHP}");
        }

        if (playerHP <= 0)
        {
            Debug.Log("Player was defeated.");
        }
        else
        {
            Debug.Log($"Defeated {enemy.name}!");
            GrantLoot(player, enemy.name, loot);
        }
    }

    private static void GrantLoot(Player player, string enemyName, LootDatabase loot)
    {
        if (loot.lootTables != null && loot.lootTables.TryGetValue(enemyName, out var table) && table.Count > 0)
        {
            var drop = table[Random.Range(0, table.Count)].item;
            player.inventory.items.Add(drop);
            Debug.Log($"Loot obtained: {drop}");
        }
    }
}
