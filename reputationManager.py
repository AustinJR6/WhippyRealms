import json
from pathlib import Path

DATA_DIR = Path('Assets/StreamingAssets')

PERK_THRESHOLDS = {
    "Verdancia": {
        "Nature Bond": 20,
        "Root Cloak": 35
    }
}

class ReputationManager:
    """Track faction reputation and perk unlocks"""

    def __init__(self, state):
        self.state = state
        self.factions = self._load()

    def _load(self):
        path = DATA_DIR / 'factions.json'
        if not path.exists():
            return {}
        with open(path) as f:
            return json.load(f)

    def _save(self):
        path = DATA_DIR / 'factions.json'
        with open(path, 'w') as f:
            json.dump(self.factions, f, indent=2)

    def add_reputation(self, faction, amount):
        fac = self.factions.setdefault(faction, {"reputation": 0, "perksUnlocked": []})
        fac["reputation"] += amount
        unlocked = []
        for perk, req in PERK_THRESHOLDS.get(faction, {}).items():
            if fac["reputation"] >= req and perk not in fac["perksUnlocked"]:
                fac["perksUnlocked"].append(perk)
                unlocked.append(perk)
        self._save()
        if unlocked:
            ps = self.state.setdefault('unlockedPerks', [])
            for p in unlocked:
                if p not in ps:
                    ps.append(p)
        return unlocked
