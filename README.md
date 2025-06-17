# WhippyRealms

WhippyRealms is a fantasy RPG project built in Unity. It aims to create a dynamic, living world powered by AI-driven quests, evolving factions and responsive companions. The project is inspired by titles such as **Skyrim**, **Elden Ring** and old-school **Runescape**.

## Core Features

- **Open World Zones** – Modular scenes connected by portals and gates. Player progress persists across zones.
- **AI-Generated Quests** – Quests are generated on demand via GPT-style logic and tracked by the quest system.
- **Living Companions & Factions** – Companions react to player choices and factions track reputation dynamically.
- **Dialogue & Emotion** – NPC dialogue can be generated with emotional tone and optional voice playback.
- **Procedural Generation** – Dungeons, loot and enemies are generated to keep the world fresh.

## Getting Started

1. Install **Unity 2022.3 LTS** or newer.
2. Clone this repository and open the project in Unity.
3. The main scripts live in `Assets/Scripts`. Attach them to GameObjects as needed.
4. To run the sample scene, create a new scene and add a `GameManager` with `SceneLoader` and `SaveSystem` components.

## Python Tools

Some AI integrations can be prototyped with Python. Install dependencies using:

```bash
pip install -r requirements.txt
```

## Vision for Future Development

This repository serves as the foundation for a much larger world. Future updates will include:

- Deeper quest logic driven by GPTQuestEngine
- Richer faction reputation systems
- Procedurally generated wilderness and dungeons
- Voice acting using TTS engines
- Expanded lore, magic systems and world-building

Contributions and ideas are welcome as WhippyRealms grows into a fully realized RPG experience.

## The World of WhippyRealms

### Backstory: The Shattering of the Concord

Long ago, five great kingdoms forged a **Concord of Light** — a pact to keep the balance between mortal realms and the Veil, a mysterious layer of unseen power that separated the living world from the unknowable. For centuries, peace flourished under the wisdom of the Concord Council — until one kingdom broke the pact, hungry to control the Veil itself.

The Sundering War that followed shattered the Veil. Magic spilled like blood across the land. Kingdoms fell, forests warped and seas split. Now, centuries later, remnants of the old world linger… and new powers rise in their shadows.

### World Map

#### Regions and Biomes

| Region Name | Biome | Notes |
|-------------|------------------------------|------------------------------------------|
| **Ishalyn Wastes** | Arid canyons, sandstone cities | Ancient ruins, scattered tribes |
| **Fynwyll Forests** | Deep enchanted woods | Home of druids, beasts, and old gods |
| **Velmara Peaks** | Snowy mountains, glacial ridges | Sky monasteries, hidden tombs |
| **Solmere Bay** | Temperate coastlands, islands | Seafaring culture, sunken ruins |
| **The Withering Expanse** | Corrupted wasteland | Scar of the Veil, cursed creatures |
| **Elarion Heartlands** | Rolling plains, rivers, oak groves | Rebuilt cities, center of power and trade |

#### Cities & Villages (Early Game Zones)

| Name | Region | Type | Description |
|------|---------|-------|-------------|
| **Myrrhfen** | Elarion | Village | Quiet village near the edge of Fynwyll — known for strange dreams |
| **Caelthorn** | Velmara | City | Fortress carved into a cliff, scholars of lost magic |
| **Duskwatch** | Solmere | Port city | Black markets, ships to the isles |
| **Rivenhold** | Ishalyn | City | Built into stone mesas, ruled by warrior clans |
| **Thornsway** | Fynwyll | Druid enclave | Hidden deep in misty woods, nature-bound faith |
| **Ashmere** | Withering | Abandoned city | Once the capital of the Concord — now cursed and haunted |

#### Forests

* **The Fynwyll** — Living forest that changes path layouts, home to beasts and spirits.
* **Duskleaf Vale** — Fog-choked glade with poisonous flora.
* **The Boughmaze** — A forest that loops in on itself; only those with a soulstone can navigate it.

#### Mountains and Ranges

* **Velmara Spires** — Towering, snow-covered peaks full of sky shrines.
* **The Thunderspine** — Mountain range struck by eternal storms; home to ancient dragon bones.
* **Cavemarch Ridge** — Network of tunnels, ore veins and goblin warrens.

#### Lakes, Seas and Rivers

* **Lake Elari** — Heart-shaped lake rumored to hold a sunken temple.
* **Solmere Sea** — Divided by shipwrecks and legends of a sea leviathan.
* **River Selyn** — Flows from the Velmara into the Heartlands bringing trade and trouble.

#### Climate Zones

| Region | Climate |
|--------|------------------------------|
| **Ishalyn** | Dry, hot, sandstorms |
| **Fynwyll** | Cool, wet, foggy, mystical |
| **Velmara** | Cold, snowy, high winds |
| **Solmere** | Mild, oceanic, variable |
| **Withering** | Chaotic, corrupted, toxic weather |
| **Elarion** | Temperate, stable, lush |

### Next We Could Build

* Factions (old Concord remnants, new mage guilds, beast tribes)
* Pantheon / Religion (Veilwalkers, forgotten gods, spirit cults)
* Magic System (Veil magic vs rune magic? Classless or archetypes?)
