const fs = require('fs');

class ZoneManager {
  constructor(jsonPath, state) {
    const raw = JSON.parse(fs.readFileSync(jsonPath, 'utf8'));
    this.zones = {};
    for (const z of raw.zones) {
      this.zones[z.name] = {
        trials: [],
        dungeons: [],
        encounters: [],
        ...z
      };
    }
    this.state = state;
    if (!this.state.unlockedZones) {
      this.state.unlockedZones = [state.location || 'Thornroot Paths'];
    }
  }

  _checkReputation(req = {}) {
    if (!req) return true;
    for (const [fac, val] of Object.entries(req)) {
      if ((this.state.reputation[fac] || 0) < val) return false;
    }
    return true;
  }

  canEnter(name) {
    const z = this.zones[name];
    if (!z) return false;
    if (z.levelRange && this.state.level < z.levelRange[0]) return false;
    if (z.unlockQuest && !this.state.completedQuests.includes(z.unlockQuest)) return false;
    if (!this._checkReputation(z.unlockReputation)) return false;
    return true;
  }

  travelTo(name) {
    if (!this.canEnter(name)) return false;
    this.state.location = name;
    if (!this.state.unlockedZones.includes(name)) this.state.unlockedZones.push(name);
    const z = this.zones[name];
    if (z.flavor) console.log(z.flavor);
    return true;
  }
}

module.exports = ZoneManager;
