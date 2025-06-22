const fs = require('fs');

class DungeonManager {
  constructor(jsonPath, state) {
    this.state = state;
    this.dungeons = {};
    if (fs.existsSync(jsonPath)) {
      const data = JSON.parse(fs.readFileSync(jsonPath, 'utf8'));
      for (const d of data) {
        this.dungeons[d.id] = d;
      }
    }
  }

  enterDungeon(id) {
    const d = this.dungeons[id];
    if (!d) return false;
    this.state.activeDungeon = id;
    this.state.dungeonHistory = this.state.dungeonHistory || [];
    this.state.dungeonHistory.push({ id, timestamp: Date.now() });
    return true;
  }

  completeDungeon(id) {
    if (this.state.activeDungeon !== id) return false;
    this.state.activeDungeon = null;
    return true;
  }
}

module.exports = DungeonManager;
