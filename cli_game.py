import json
import os
import random
from pathlib import Path
from combatEngine import CombatEngine

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
        self.quests_db = {q['name']: q for q in load_json('quests.json')['quests']}
        self.zones = {z['name']: z for z in load_json('zones.json')['zones']}
        self.dialogues = {d['npc']: d for d in load_json('dialogue.json')['dialogues']}
        self.state_path = DATA_DIR / 'playerState.json'
        self.state = load_json('playerState.json')
        self.stats = self.state['stats']
        self.engine = CombatEngine(self.skills)

    # ----- Save/Load -----
    def save(self):
        save_json('playerState.json', self.state)
        print('Game saved.')

    def load(self):
        self.state = load_json('playerState.json')
        self.stats = self.state['stats']
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
        active = self.state['activeQuests']
        if not active:
            print('No active quests.')
        for q in active:
            obj = q['objectives'][0]
            print(f"{q['name']}: {obj['target']} {obj.get('progress',0)}/{obj['count']}")
        board = input('Check quest board? (y/n) ').strip().lower()
        if board == 'y':
            for i, q in enumerate(self.quests_db.values(), 1):
                print(f"{i}. {q['name']}")
            choice = input('Accept quest # or press Enter: ').strip()
            if choice:
                q = list(self.quests_db.values())[int(choice)-1]
                if q not in active:
                    q = json.loads(json.dumps(q))  # deep copy
                    for o in q['objectives']:
                        o['progress'] = 0
                    active.append(q)
                    print(f"Accepted quest '{q['name']}'.")

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
            self.update_quests(enemy_name)
        for l in log:
            print(l)

    def grant_loot(self, enemy_name):
        table = self.loot.get(enemy_name)
        if table:
            drop = random.choice(table)['item']
            self.state['inventory']['items'].append(drop)
            print(f'Loot gained: {drop}')

    def update_quests(self, enemy_name):
        for quest in self.state['activeQuests']:
            for obj in quest['objectives']:
                if obj['type'] == 'kill' and obj['target'] == enemy_name:
                    obj['progress'] = obj.get('progress', 0) + 1
        completed = [q for q in self.state['activeQuests'] if all(o.get('progress',0) >= o['count'] for o in q['objectives'])]
        for q in completed:
            print(f"Quest complete: {q['name']}")
            self.state['completedQuests'].append(q)
            self.state['activeQuests'].remove(q)

    # ----- Main Loop -----
    def main(self):
        while True:
            print(f"\nLocation: {self.state['zone']}  HP:{self.stats['health']}")
            cmd = input('(travel/fight/quests/inventory/talk/save/load/quit)> ').strip().lower()
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
            elif cmd == 'quit':
                break
            else:
                print('Unknown command')


if __name__ == '__main__':
    Game().main()
