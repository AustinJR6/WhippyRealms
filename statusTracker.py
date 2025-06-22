from copy import deepcopy


def apply_effects(combatant, log):
    for eff in deepcopy(combatant.active_effects):
        if eff.get("damage"):
            combatant.hp -= eff["damage"]
            log.append(f"{combatant.name} suffers {eff['damage']} from {eff['status']}")
        eff["remaining"] -= 1
        if eff["remaining"] <= 0:
            combatant.active_effects.remove(eff)
            log.append(f"{eff['status']} on {combatant.name} fades")


def tick_cooldowns(combatant):
    for sk in list(combatant.cooldowns.keys()):
        combatant.cooldowns[sk] -= 1
        if combatant.cooldowns[sk] <= 0:
            del combatant.cooldowns[sk]
