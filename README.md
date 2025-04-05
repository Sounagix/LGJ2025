# 🎮 Game Design Document – *Heart of Cards*

**Game Jam:** LAGS Game Jam 2025  
**Platform:** WebGL (Unity)  
**Genre:** Card Game / Strategy / Exploration  
**Average playtime:** 5–10 minutes  
**Game mode:** Single player  
**Controls:** Mouse  

---

## 🧩 General Concept

**Heart of Cards** is a strategy game where the player explores a map made entirely of cards.  
Each move is a tactical decision between empty paths, combat, or rewards.

The goal is to reach the final boss card and defeat it, using a deck built from previously collected cards.  
Once a card is used in combat, it is removed from the deck for the rest of the run, forcing the player to manage their resources strategically.

---

## 🗺️ Core Mechanics

### 🔹 Exploration
- The map is generated from cards that act as nodes.
- The player clicks to move between adjacent cards.
- Node types:
  - 🟦 Empty: nothing happens.
  - 🟨 Reward: grants a new card to the deck.
  - 🔺 Combat: triggers a battle.

### 🔹 Combat System
- The player uses the available deck upon entering combat.
- Cards are removed from the deck **until the end of the run** once played.
- Cards can deal damage, block, heal, provide attack buffs, or defense buffs.

### 🔹 Victory Condition
- Reach the final boss card.
- Defeat it to complete the run.

---

## 📋 Content and Design

### 🃏 Cards
- Obtained by stepping on reward cards, which offer three options — the player must choose one.
- Cards can perform actions such as:
  - Basic attack
  - Defense
  - Healing
  - Buffs

### 💀 Enemies
- Enemies feature simple or unique attack patterns.
- The final boss has greater difficulty.

---

## 🎨 Visual Style
- 2D pixel art graphics.
- Clean and minimalist aesthetic.
- Stylized cards with a clear, readable UI.

---

## 🔊 Sound and Music

*(To be defined)*

---

## 🖱️ Controls

- **Mouse only**
  - Click to move across the card map.
  - Drag & drop or click to play cards during combat.

---

## ⚙️ Technology

- **Engine:** Unity
- **Version:** 2022 or later
- **Target platform:** WebGL
- **Distribution:** Itch.io

---

## 🙌 Credits

| Name       | Role                            |
|------------|---------------------------------|
| sounagix   | Game Design, Programming        |
| Reckoner   | Art, Game Design, Management    |

---

## 📌 Additional Notes

- Fast-paced gameplay: 5–10 minutes per run.
- Easy to learn, hard to master.
- Inspired by games like *Slay the Spire*, *Dicey Dungeons*, and roguelike systems.
- Features an original mechanic: exploring a map made entirely of cards.

---
## 🌐 Links

- **Itch.io page:** [https://randomplayerstudios.itch.io/heartofcards](https://randomplayerstudios.itch.io/heartofcards)

- **Play the game:** [https://sounagix.github.io/HeartOfCards/](https://sounagix.github.io/HeartOfCards/)
- **Source code (GitHub):** [https://github.com/Sounagix/LGJ2025](https://github.com/Sounagix/LGJ2025)

---