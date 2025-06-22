const fs = require('fs');

class DialogueManager {
  constructor(jsonPath, state) {
    const raw = JSON.parse(fs.readFileSync(jsonPath, 'utf8'));
    const trees = raw.dialogueTrees || raw;
    this.trees = {};
    for (const t of trees) {
      this.trees[t.id] = t;
    }
    this.state = state;
  }

  _checkRequirements(req = {}) {
    if (req.reputation) {
      for (const [faction, val] of Object.entries(req.reputation)) {
        if ((this.state.reputation[faction] || 0) < val) {
          return false;
        }
      }
    }
    if (req.completedQuest) {
      if (!this.state.completedQuests.includes(req.completedQuest)) return false;
    }
    return true;
  }

  getLine(id) {
    const tree = this.trees[id];
    if (!tree) return null;
    for (const line of tree.lines) {
      if (this._checkRequirements(line.requirements)) {
        return { ...line, speaker: tree.speaker, quest: tree.quest };
      }
    }
    return null;
  }
}

module.exports = DialogueManager;
