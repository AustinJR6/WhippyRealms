import json
from pathlib import Path

DATA_DIR = Path('Assets/StreamingAssets')

class MountManager:
    """Handle mount ownership and combat effects"""

    def __init__(self, state):
        self.state = state
        self.mounts = {m['id']: m for m in self._load()}

    def _load(self):
        path = DATA_DIR / 'mounts.json'
        if not path.exists():
            return []
        with open(path) as f:
            return json.load(f)

    def unlock_from_quest(self, quest_id, reputation_mgr=None):
        for m in self.mounts.values():
            req = m.get('requirements', {})
            if req.get('quest') != quest_id:
                continue
            if reputation_mgr:
                fac = req.get('region')
                needed = req.get('reputation', 0)
                current = reputation_mgr.factions.get(fac, {}).get('reputation', 0)
                if current < needed:
                    continue
            if m['id'] not in self.state.get('ownedMounts', []):
                self.state.setdefault('ownedMounts', []).append(m['id'])

    def set_active(self, mount_id):
        if mount_id in self.state.get('ownedMounts', []):
            self.state['activeMount'] = mount_id

    def apply_combat_effect(self, player, log):
        m_id = self.state.get('activeMount')
        if not m_id:
            return
        mount = self.mounts.get(m_id)
        if not mount:
            return
        effect = mount.get('combatEffect', '')
        if 'Heals' in effect:
            heal = 5
            player.hp += heal
            log.append(f"{mount['name']} heals you for {heal} HP")
