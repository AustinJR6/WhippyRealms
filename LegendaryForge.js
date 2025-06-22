const fs = require('fs');

class LegendaryForge {
  constructor(recipesPath, playerStatePath, inventoryPath) {
    this.recipesPath = recipesPath;
    this.playerStatePath = playerStatePath;
    this.inventoryPath = inventoryPath;
    this._load();
  }

  _load() {
    this.recipes = {};
    if (fs.existsSync(this.recipesPath)) {
      const data = JSON.parse(fs.readFileSync(this.recipesPath, 'utf8'));
      for (const r of data) {
        if (r.rarity === 'legendary') {
          this.recipes[r.id] = r;
        }
      }
    }
    this.player = fs.existsSync(this.playerStatePath)
      ? JSON.parse(fs.readFileSync(this.playerStatePath, 'utf8'))
      : { craftingXP: 0, craftingLevel: 1 };
    this.inventory = fs.existsSync(this.inventoryPath)
      ? JSON.parse(fs.readFileSync(this.inventoryPath, 'utf8'))
      : { materials: {}, craftedItems: [] };
  }

  _save() {
    fs.writeFileSync(this.inventoryPath, JSON.stringify(this.inventory, null, 2));
    fs.writeFileSync(this.playerStatePath, JSON.stringify(this.player, null, 2));
  }

  _hasMaterials(req) {
    for (const [m, c] of Object.entries(req)) {
      if ((this.inventory.materials[m] || 0) < c) return false;
    }
    return true;
  }

  _consume(req) {
    for (const [m, c] of Object.entries(req)) {
      this.inventory.materials[m] -= c;
      if (this.inventory.materials[m] <= 0) delete this.inventory.materials[m];
    }
  }

  forge(id, options = {}) {
    const rec = this.recipes[id];
    if (!rec) return false;
    if (!this._hasMaterials(rec.requiredMaterials)) return false;
    if (options.relic !== rec.requiredRelic) return false;
    if ((this.player.reputation?.[rec.faction] || 0) < (rec.repRequired || 0)) return false;
    this._consume(rec.requiredMaterials);
    const item = {
      id: id,
      name: options.name || rec.name,
      rarity: 'legendary',
      effects: rec.effects
    };
    this.inventory.craftedItems.push(item);
    this.player.craftingXP = (this.player.craftingXP || 0) + 50;
    this._save();
    return item;
  }
}

module.exports = LegendaryForge;
