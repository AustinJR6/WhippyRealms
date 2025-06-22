import json
import os
import random
from pathlib import Path
from combatEngine import CombatEngine
from questSystem import QuestManager
from boonSystem import BoonSystem
from mountManager import MountManager
from reputationManager import ReputationManager

DATA_DIR = Path('Assets/StreamingAssets')


def load_json(filename):
    path = DATA_DIR / filename
    with open(path) as f:
        return json.load(f)


def save_json(filename, data):
    path = DATA_DIR / filename
    with open(path, 'w') as f:
        json.dump(data, f, indent=2)


class Game:
    def __init__(self):
        self.skills = {s['name']: s for s in load_json('skills.json')['skills']}
        self.creatures = {c['name']: c for c in load_json('creatures.json')['creatures']}
        self.loot = load_json('loot.json')['lootTables']
        self.zones = {z['name']: z for z in load_json('zones.json')['zones']}
        self.dialogues = {d['npc']: d for d in load_json('dialogue.json')['dialogues']}
        self.state_path = DATA_DIR / 'playerState.json'
        self.state = load_json('playerState.json')
        self.stats = self.state['stats']
        self.boons = BoonSystem(self.state)
        self.mounts = MountManager(self.state)
        self.reputation = ReputationManager(self.state)
        self.engine = CombatEngine(self.skills, self.boons, self.mounts)
        self.quest_mgr = QuestManager(self.state, self.reputation, self.mounts, self.boons)

    # ----- Save/Load -----
    def save(self):
        save_json('playerState.json', self.state)
        self.reputation._save()
        print('Game saved.')

    def load(self):
        self.state = load_json('playerState.json')
        self.stats = self.state['stats']
        self.boons = BoonSystem(self.state)
        self.mounts = MountManager(self.state)
        self.reputation = ReputationManager(self.state)
        self.engine = CombatEngine(self.skills, self.boons, self.mounts)
        self.quest_mgr = QuestManager(self.state, self.reputation, self.mounts, self.boons)
        print('Game loaded.')

    # ----- Inventory -----
    def show_inventory(self):
        inv = self.state['inventory']
        print(f"Gold: {inv['gold']}")
        for i, item in enumerate(inv['items'], 1):
            print(f"{i}. {item}")
        cmd = input('(equip/use/back)> ').strip().lower()
        if cmd == 'equip' and inv['items']:
            idx = int(input('Item # to equip: ')) - 1
            if 0 <= idx < len(inv['items']):
                self.state['equipped']['weapon'] = inv['items'][idx]
                print(f"Equipped {inv['items'][idx]}")
        elif cmd == 'use' and inv['items']:
            idx = int(input('Item # to use: ')) - 1
            if 0 <= idx < len(inv['items']):
                item = inv['items'].pop(idx)
                if item == 'Healing Herb':
                    self.stats['health'] += 5
                    print('You feel rejuvenated.')
                else:
                    print(f'Used {item}')

    # ----- Quests -----
    def show_quests(self):
        self.quest_mgr.quest_log()
        board = input('Check quest board? (y/n) ').strip().lower()
        if board == 'y':
            all_quests = list(self.quest_mgr.quests_db.values())
            for i, q in enumerate(all_quests, 1):
                print(f"{i}. {q['name']}")
            choice = input('Accept quest # or press Enter: ').strip()
            if choice:
                idx = int(choice) - 1
                if 0 <= idx < len(all_quests):
                    self.quest_mgr.accept_quest(all_quests[idx]['id'])

    # ----- Dialogue -----
    def talk(self):
        npc = 'Old Hermit'
        d = self.dialogues.get(npc)
        if not d:
            return
        node = 'start'
        while True:
            nd = d['nodes'][node]
            print(nd['text'])
            if 'event' in nd:
                self.handle_event(nd['event'])
            if not nd['options']:
                break
            opts = list(nd['options'].items())
            for i, (text, _) in enumerate(opts, 1):
                print(f"{i}. {text}")
            choice = int(input('> ')) - 1
            node = opts[choice][1]

    def handle_event(self, event):
        if event.startswith('gain_item:'):
            item = event.split(':',1)[1]
            self.state['inventory']['items'].append(item)
            print(f'Gained item: {item}')

    # ----- Travel -----
    def travel(self):
        current = self.zones[self.state['zone']]
        targets = current.get('connected_zones', [])
        for i, name in enumerate(targets, 1):
            print(f"{i}. {name}")
        if not targets:
            print('No connected zones.')
            return
        choice = int(input('Travel to #: ')) - 1
        if 0 <= choice < len(targets):
            dest = self.zones[targets[choice]]
            min_lvl, max_lvl = dest['levelRange']
            if self.state['level'] < min_lvl:
                print('You are not high enough level.')
                return
            self.state['zone'] = dest['name']
            print(f"Traveled to {dest['name']}")

    # ----- Combat -----
    def fight(self):
        zone = self.zones[self.state['zone']]
        if not zone['enemies']:
            print('No enemies here.')
            return
        enemy_name = random.choice(zone['enemies'])
        enemy = json.loads(json.dumps(self.creatures[enemy_name]))
        print(f'Encountered {enemy_name}!')
        def choose_skill(player, foe):
            print(f"You: {player.hp} HP  {foe.name}: {foe.hp} HP")
            for i, s in enumerate(player.skills, 1):
                print(f"{i}. {s}")
            idx = int(input('Choose skill: ')) - 1
            if 0 <= idx < len(player.skills):
                return player.skills[idx]
            return player.skills[0]

        result, log, player_obj, enemy_obj = self.engine.combat(self.state, enemy, choose_skill)
        self.stats['health'] = player_obj.hp
        self.state['activeEffects'] = player_obj.active_effects
        self.state['cooldowns'] = player_obj.cooldowns
        if not result:
            print('You were defeated...')
            self.stats['health'] = max(1, self.stats['health'] // 2)
        else:
            print(f'You defeated {enemy_name}!')
            self.grant_loot(enemy_name)
            self.quest_mgr.update_kill_count(enemy_name)
        for l in log:
            print(l)

    def grant_loot(self, enemy_name):
        table = self.loot.get(enemy_name)
        if table:
            drop = random.choice(table)['item']
            self.state['inventory']['items'].append(drop)
            print(f'Loot gained: {drop}')


    # ----- Main Loop -----
    def main(self):
        while True:
            print(f"\nLocation: {self.state['zone']}  HP:{self.stats['health']}")
            cmd = input('(travel/fight/quests/inventory/talk/save/load/questlog/accept/complete/reset/quit)> ').strip().lower()
            if cmd == 'travel':
                self.travel()
            elif cmd == 'fight':
                self.fight()
            elif cmd == 'quests':
                self.show_quests()
            elif cmd == 'inventory':
                self.show_inventory()
            elif cmd == 'talk':
                self.talk()
            elif cmd == 'save':
                self.save()
            elif cmd == 'load':
                self.load()
            elif cmd == 'questlog':
                self.quest_mgr.quest_log()
            elif cmd.startswith('accept '):
                self.quest_mgr.accept_quest(cmd.split(' ',1)[1])
            elif cmd.startswith('complete '):
                self.quest_mgr.complete_quest(cmd.split(' ',1)[1])
            elif cmd == 'reset':
                self.quest_mgr.reset_quests()
            elif cmd == 'quit':
                break
            else:
                print('Unknown command')


if __name__ == '__main__':
    Game().main()
