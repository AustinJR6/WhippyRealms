const fs = require('fs');

class CraftingManager {
  constructor(recipesPath, potionsPath, inventoryPath, playerStatePath) {
    this.recipesPath = recipesPath;
    this.potionsPath = potionsPath;
    this.inventoryPath = inventoryPath;
    this.playerStatePath = playerStatePath;
    this._loadData();
  }

  _loadData() {
    this.recipes = {};
    if (fs.existsSync(this.recipesPath)) {
      const data = JSON.parse(fs.readFileSync(this.recipesPath, 'utf8'));
      for (const r of data) {
        this.recipes[r.id] = r;
      }
    }
    this.potionRecipes = {};
    if (fs.existsSync(this.potionsPath)) {
      const data = JSON.parse(fs.readFileSync(this.potionsPath, 'utf8'));
      for (const p of data) {
        this.potionRecipes[p.id] = p;
      }
    }
    this.inventory = fs.existsSync(this.inventoryPath)
      ? JSON.parse(fs.readFileSync(this.inventoryPath, 'utf8'))
      : { gold: 0, materials: {}, consumables: [], craftedItems: [] };
    this.playerState = fs.existsSync(this.playerStatePath)
      ? JSON.parse(fs.readFileSync(this.playerStatePath, 'utf8'))
      : { craftingXP: 0, craftingLevel: 1 };
  }

  _saveInventory() {
    fs.writeFileSync(this.inventoryPath, JSON.stringify(this.inventory, null, 2));
  }

  _savePlayerState() {
    fs.writeFileSync(this.playerStatePath, JSON.stringify(this.playerState, null, 2));
  }

  listAvailableRecipes() {
    const list = [];
    for (const r of Object.values(this.recipes)) {
      if (this.playerState.craftingLevel >= r.requiredCraftingLevel) {
        list.push(r);
      }
    }
    return list;
  }

  _hasMaterials(req) {
    for (const [mat, count] of Object.entries(req)) {
      if ((this.inventory.materials[mat] || 0) < count) return false;
    }
    return true;
  }

  _consumeMaterials(req) {
    for (const [mat, count] of Object.entries(req)) {
      this.inventory.materials[mat] -= count;
      if (this.inventory.materials[mat] <= 0) delete this.inventory.materials[mat];
    }
  }

  craftItem(id, opts = {}) {
    const recipe = this.recipes[id];
    if (!recipe) return false;
    if (this.playerState.craftingLevel < recipe.requiredCraftingLevel) return false;
    if (recipe.requiresStation && recipe.requiresStation !== opts.station) return false;
    if (recipe.region && recipe.region !== opts.region) return false;
    if (!this._hasMaterials(recipe.requiredMaterials)) return false;
    this._consumeMaterials(recipe.requiredMaterials);
    const item = { id: recipe.id, name: recipe.name, rarity: recipe.rarity, effects: recipe.effects };
    // basic crit chance influenced by playerState.critBonus or boons
    const critChance = this.playerState.critChance || 0;
    if (Math.random() < critChance) {
      item.rarity = 'enhanced-' + item.rarity;
    }
    this.inventory.craftedItems.push(item);
    this.playerState.craftingXP = (this.playerState.craftingXP || 0) + recipe.requiredCraftingLevel * 10;
    this._saveInventory();
    this._savePlayerState();
    return item;
  }

  brewPotion(id) {
    const rec = this.potionRecipes[id];
    if (!rec) return false;
    const req = rec.ingredients.reduce((a, m) => { a[m] = (a[m] || 0) + 1; return a; }, {});
    if (!this._hasMaterials(req)) return false;
    this._consumeMaterials(req);
    this.inventory.consumables.push({ id: rec.id, name: rec.name, effect: rec.effect, duration: rec.duration });
    this.playerState.craftingXP = (this.playerState.craftingXP || 0) + 5;
    this._saveInventory();
    this._savePlayerState();
    return true;
  }
}

module.exports = CraftingManager;
