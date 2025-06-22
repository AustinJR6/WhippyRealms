import json
import random
from copy import deepcopy

from statusTracker import apply_effects, tick_cooldowns
from boonSystem import BoonSystem
from mountManager import MountManager

# elemental modifiers simple matrix
ELEMENT_MATRIX = {
    ("Nature", "Fire"): 0.5,
    ("Fire", "Nature"): 1.5,
}

class Combatant:
    def __init__(self, data, skills_db):
        self.name = data.get("name", "Player")
        self.hp = data["stats"]["health"]
        self.attack = data["stats"].get("attack", 0)
        self.magic = data["stats"].get("magic", 0)
        self.skills = data.get("skills", [])
        self.active_effects = deepcopy(data.get("activeEffects", []))
        self.cooldowns = deepcopy(data.get("cooldowns", {}))
        self.element = data.get("element", "Physical")
        self.skills_db = skills_db

    def choose_skill(self, index=0):
        if not self.skills:
            return None
        if index >= len(self.skills):
            index = 0
        return self.skills_db[self.skills[index]]

def elemental_multiplier(attacker_el, defender_el):
    return ELEMENT_MATRIX.get((attacker_el, defender_el), 1.0)

class CombatEngine:
    def __init__(self, skills_db, boon_system=None, mount_manager=None):
        self.skills_db = skills_db
        self.boon_system = boon_system
        self.mount_manager = mount_manager

    def combat(self, player_data, enemy_data, choose_skill_fn=None):
        player = Combatant(player_data, self.skills_db)
        enemy = Combatant(enemy_data, self.skills_db)
        phases = enemy_data.get("bossPhases", [])
        phase_index = 0
        log = []
        turn = 0
        while player.hp > 0 and enemy.hp > 0:
            turn += 1
            log.append(f"-- Turn {turn} --")
            # apply ongoing effects
            apply_effects(player, log)
            apply_effects(enemy, log)
            if self.mount_manager:
                self.mount_manager.apply_combat_effect(player, log)
            if player.hp <= 0 or enemy.hp <= 0:
                if self.boon_system and player.hp <= 0:
                    self.boon_system.trigger('onDeath', player, enemy, log)
                if player.hp <= 0 or enemy.hp <= 0:
                    break
            # player turn
            if choose_skill_fn:
                chosen = choose_skill_fn(player, enemy)
                if chosen in player.skills:
                    idx = player.skills.index(chosen)
                else:
                    idx = 0
            else:
                idx = 0
            pskill_name = player.skills[idx]
            pskill = player.choose_skill(idx)
            if player.cooldowns.get(pskill_name, 0) == 0 and player.magic >= pskill.get("cost",0):
                self.use_skill(player, enemy, pskill_name, pskill, log)
            else:
                dmg = player.attack
                enemy.hp -= dmg
                log.append(f"{player.name} attacks for {dmg} dmg")
            if enemy.hp <= 0:
                break
            if phase_index < len(phases) and enemy.hp <= phases[phase_index].get("threshold", 0):
                msg = phases[phase_index].get("dialogue")
                if msg:
                    log.append(msg)
                phase_skill = phases[phase_index].get("addSkill")
                if phase_skill and phase_skill not in enemy.skills:
                    enemy.skills.append(phase_skill)
                phase_index += 1
            # enemy turn (random)
            tick_cooldowns(player)
            tick_cooldowns(enemy)
            eskill_name = random.choice(enemy.skills)
            eskill = enemy.choose_skill(enemy.skills.index(eskill_name))
            if enemy.cooldowns.get(eskill_name,0) ==0 and enemy.magic >= eskill.get("cost",0):
                self.use_skill(enemy, player, eskill_name, eskill, log)
            else:
                dmg = enemy.attack
                player.hp -= dmg
                log.append(f"{enemy.name} attacks for {dmg} dmg")
            if self.boon_system:
                self.boon_system.trigger('onHit', player, enemy, log)
            if player.hp <= 0:
                if self.boon_system:
                    self.boon_system.trigger('onDeath', player, enemy, log)
            if player.hp <= 0:
                break
        result = player.hp > 0
        return result, log, player, enemy

    def use_skill(self, user, target, skill_name, skill, log):
        user.magic -= skill.get("cost",0)
        cd = skill.get("cooldown",0)
        if cd:
            user.cooldowns[skill_name] = cd
        effect = skill.get("effect", {})
        dmg = effect.get("damage",0) + user.attack
        heal = effect.get("heal",0)
        if dmg:
            mult = elemental_multiplier(skill.get("element","Physical"), target.element)
            dmg = int(dmg * mult)
            if any(eff.get("shield") for eff in target.active_effects):
                dmg = dmg // 2
                for eff in target.active_effects:
                    if eff.get("shield"):
                        eff["remaining"] -= 1
                        if eff["remaining"] <=0:
                            target.active_effects.remove(eff)
                        break
            target.hp -= dmg
            log.append(f"{user.name} uses {skill_name} for {dmg} dmg")
        if heal:
            user.hp = min(user.hp + heal, user.hp)
            log.append(f"{user.name} heals {heal}")
        status = effect.get("status")
        if status:
            user_effect = {
                "status": status,
                "damage": effect.get("dot",0),
                "shield": 1 if status=="shield" else 0,
                "remaining": effect.get("duration",1)
            }
            if status in ["burn","poison"] and effect.get("damage"):
                user_effect["damage"] = effect["damage"]
            target.active_effects.append(user_effect)
            log.append(f"{target.name} gains status {status}")

