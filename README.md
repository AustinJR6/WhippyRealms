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
