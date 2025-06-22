const fs = require('fs');

class EncounterManager {
  constructor(jsonPath, state) {
    this.state = state;
    this.encounters = {};
    if (fs.existsSync(jsonPath)) {
      const data = JSON.parse(fs.readFileSync(jsonPath, 'utf8'));
      for (const e of data) {
        this.encounters[e.id] = e;
      }
    }
  }

  trigger(eventKey) {
    const matches = Object.values(this.encounters).filter(e => e.trigger === eventKey);
    const triggered = [];
    for (const enc of matches) {
      this.state.encounterHistory = this.state.encounterHistory || [];
      this.state.encounterHistory.push({ id: enc.id, time: Date.now() });
      triggered.push(enc);
    }
    return triggered;
  }
}

module.exports = EncounterManager;
