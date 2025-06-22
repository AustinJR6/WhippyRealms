import json
from pathlib import Path

DATA_DIR = Path('Assets/StreamingAssets')


def _load_json(filename, default=None):
    path = DATA_DIR / filename
    if not path.exists():
        return default if default is not None else {}
    with open(path) as f:
        return json.load(f)


def _save_json(filename, data):
    path = DATA_DIR / filename
    with open(path, 'w') as f:
        json.dump(data, f, indent=2)


class QuestManager:
    """Handles loading, tracking and completing quests."""

    def __init__(self, player_state, rep_mgr, mount_mgr, boon_mgr):
        self.state = player_state
        self.rep_mgr = rep_mgr
        self.mount_mgr = mount_mgr
        self.boon_mgr = boon_mgr
        self.quests_db = {q['id']: q for q in _load_json('quests.json', [])}

    # ---- Quest Acquisition ----
    def accept_quest(self, quest_id):
        quest = self.quests_db.get(quest_id)
        if not quest:
            print('Quest not found.')
            return
        if any(q['id'] == quest_id for q in self.state['activeQuests']):
            print('Quest already active.')
            return
        instance = json.loads(json.dumps(quest))
        instance['progress'] = {k: 0 for k in instance['objectives']}
        self.state['activeQuests'].append(instance)
        print(f"Accepted quest '{instance['name']}'.")

    # ---- Progress Updates ----
    def update_kill_count(self, enemy_name):
        updated = False
        for quest in list(self.state['activeQuests']):
            obj = quest['objectives'].get('kill')
            if obj and obj['enemy'] == enemy_name:
                quest['progress']['kill'] += 1
                updated = True
                cur = quest['progress']['kill']
                total = obj['count']
                print(f"Quest Updated: {cur}/{total} {enemy_name}s defeated")
                if cur >= total:
                    self._complete_quest(quest)
        return updated

    # ---- Completion ----
    def _complete_quest(self, quest):
        print(f"Quest complete: {quest['name']}")
        rewards = quest.get('rewards', {})
        self.state['xp'] = self.state.get('xp', 0) + rewards.get('xp', 0)
        self.state['inventory']['items'].extend(rewards.get('items', []))
        for faction, val in rewards.get('reputation', {}).items():
            self.rep_mgr.add_reputation(faction, val)
        self.mount_mgr.unlock_from_quest(quest.get('id'), self.rep_mgr)
        for boon in rewards.get('boons', []):
            self.boon_mgr.activate(boon)
        self.state['completedQuests'].append(quest)
        self.state['activeQuests'].remove(quest)

    # ---- Dev Helpers ----
    def quest_log(self):
        if not self.state['activeQuests']:
            print('No active quests.')
        for quest in self.state['activeQuests']:
            obj = quest['objectives'].get('kill')
            if obj:
                progress = quest['progress'].get('kill', 0)
                print(f"{quest['name']}: {progress}/{obj['count']} {obj['enemy']}")
        if self.state['completedQuests']:
            print('Completed: ' + ', '.join(q['name'] for q in self.state['completedQuests']))

    def complete_quest(self, quest_id):
        for quest in list(self.state['activeQuests']):
            if quest['id'] == quest_id:
                self._complete_quest(quest)
                break

    def reset_quests(self):
        self.state['activeQuests'].clear()
        self.state['completedQuests'].clear()
        print('Quests reset.')
