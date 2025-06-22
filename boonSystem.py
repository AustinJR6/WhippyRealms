import json
from pathlib import Path

DATA_DIR = Path('Assets/StreamingAssets')

class BoonSystem:
    """Manage divine boons and their effects"""

    def __init__(self, state):
        self.state = state
        self.boons = {b['id']: b for b in self._load()}

    def _load(self):
        path = DATA_DIR / 'boons.json'
        if not path.exists():
            return []
        with open(path) as f:
            return json.load(f)

    def activate(self, boon_id):
        boon = self.boons.get(boon_id)
        if not boon:
            return False
        if boon.get('type') == 'major':
            to_remove = [bid for bid in self.state.get('activeBoons', [])
                         if self.boons.get(bid, {}).get('type') == 'major']
            for bid in to_remove:
                self.state['activeBoons'].remove(bid)
        if boon_id not in self.state.get('activeBoons', []):
            self.state.setdefault('activeBoons', []).append(boon_id)
        return True

    def trigger(self, event, player, enemy=None, log=None):
        """Trigger boons based on event"""
        log = log if log is not None else []
        for bid in list(self.state.get('activeBoons', [])):
            boon = self.boons.get(bid)
            if not boon:
                continue
            if boon.get('trigger') != event:
                continue
            if bid == 'verdant_resurrection':
                uses = self.state.get('boonUses', {}).get(bid, 0)
                if uses >= 1:
                    continue
                player.hp = max(1, player.stats.get('health', player.hp) // 2)
                log.append('Verdant Resurrection revives you!')
                self.state.setdefault('boonUses', {})[bid] = uses + 1
            elif bid == 'thornsoul' and enemy is not None:
                enemy.hp -= 1
                log.append('Thornsoul reflects 1 damage!')
        return log

    def reset_daily(self):
        self.state['boonUses'] = {}
