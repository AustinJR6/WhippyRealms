const fs = require('fs');

class TrialManager {
  constructor(jsonPath, state) {
    this.state = state;
    this.trials = {};
    if (fs.existsSync(jsonPath)) {
      const data = JSON.parse(fs.readFileSync(jsonPath, 'utf8'));
      for (const t of data) {
        this.trials[t.id] = t;
      }
    }
  }

  _meetsRequirements(trial) {
    const req = trial.requirements || {};
    if (req.reputation) {
      for (const [fac, val] of Object.entries(req.reputation)) {
        if ((this.state.reputation?.[fac] || 0) < val) return false;
      }
    }
    if (req.quest && !this.state.completedQuests?.includes(req.quest)) return false;
    return true;
  }

  startTrial(id) {
    const t = this.trials[id];
    if (!t) return false;
    if (!this._meetsRequirements(t)) return false;
    this.state.activeTrial = id;
    return true;
  }

  completeTrial(id) {
    const t = this.trials[id];
    if (!t || this.state.activeTrial !== id) return false;
    this.state.activeTrial = null;
    this.state.completedTrials = this.state.completedTrials || [];
    if (!this.state.completedTrials.includes(id)) {
      this.state.completedTrials.push(id);
    }
    const reward = t.reward || {};
    if (reward.legendaryItem) {
      this.state.inventory.items.push(reward.legendaryItem);
    }
    if (reward.craftingXP) {
      this.state.craftingXP = (this.state.craftingXP || 0) + reward.craftingXP;
    }
    if (reward.boon) {
      this.state.activeBoons = this.state.activeBoons || [];
      if (!this.state.activeBoons.includes(reward.boon)) {
        this.state.activeBoons.push(reward.boon);
      }
    }
    return true;
  }
}

module.exports = TrialManager;
