# 🧟 Unity State Machine Game – Zombie Combat Adventure
# Game Download Link :- https://magicemperor30.itch.io/zombie-killer
## 🎮 Overview

A Unity-based side-scroller that demonstrates a **modular state machine system** supporting multiple gameplay modes: on-foot navigation, combat, and vehicle driving. The game is built with scalability and mobile performance in mind and includes interactive AI zombies, dynamic obstacles, and a complete gameplay loop.
The Game Here has a different Character cause the its size was greater than 100mbs which was causing problem for uploading so i have uploaded a different character which works too any humanoid character is fine.
## 🚀 Features

### ✅ Modular State Machine System

* **On-Foot States**:

  * `Idle`, `Walk`, `Run`
* **Combat States**:

  * `CombatMovement` (handles shooting & movement)
  * `Defend`
* **Driving States**:

  * `Driving` (handles Acceleration and Braking)
* **Zombie AI States**:

  * `Idle`, `Walk`, `Run`, `Attack`

### ✅ Gameplay Elements

* One action-packed level with:

  * Zombies as enemies
  * Explosive barrels
  * Flamethrowers that damage the player
* Player and zombie both have separate health systems

### ✅ Game Loop

* **Start Menu**: Start and Quit options
* **Cutscene**: Displays game objective
* **In-Game Pause Menu**: Pause, resume, or quit during gameplay
* **Finish**: Reach the car to complete the level

### ✅ Mobile Optimization

* Optimized rendering using `ScalableBufferManager` for dynamic resolution scaling (default: 0.8x)
* Target frame rate set to 30 FPS for battery efficiency
* VSync configurable (disabled by default for mobile performance)
* Auto-detects low-end devices using RAM and VRAM thresholds
* Dynamically applies quality settings:
  * Low Quality for RAM ≤ 1500MB or VRAM ≤ 512MB
  * High Quality otherwise
* Lightweight, breakpoint-safe code optimized for Android & iOS builds
* Ready for Android deployment and tested for smooth mobile gameplay

## 🧠 Architecture

### 🔁 State Machine Design

* Uses **State**, **Strategy**, and **Factory** design patterns
* Each major state encapsulates its own sub-states and transitions
* Fully modular and extendable

### 🛠 Technologies Used

* Unity Engine (Version 6)
* C# (OOP, design patterns)
* Unity UI TMP
* Git for version control

## 🧪 Debug & Dev Tools

* Conditional loggings

## 📦 Project Setup

### 1. Clone the Repository

```bash
git clone https://github.com/MagicEmperor30/ZombieKiller_3dSideScroller.git
```

### 2. Open in Unity

* Open the project with Unity Hub
* Set build target to Android based on your needs

### 3. Play

* Press `Play` in the Unity Editor or build to your device

## 🎯 Controls

| Action        | Input (Mobile)          |
| ------------- | ----------------------- |
| Move          | On-screen arrows        |
| Shoot         | Combat Mode only button |
| Defend        | Shield button           |
| Drive         | Left/Right + Brake      |
| Skip Cutscene | Appears after 10 sec    |

### ⚠ Known Issues & Improvements

* 🐞 **Bullet Firing**: Single bullet does not always spawn on first tap; requires multiple taps (needs fix)
* 🧠 **Enemy AI**: Advanced zombie AI with patrol, chase, and attack behavior
* 🐞 **Combat Mode Bug**: Player may occasionally get stuck in combat mode; switching back to non-combat temporarily resolves it
* 🛡️ **Defend Logic**: Defend behavior implemented with future improvements planned



## 📎 Submission Checklist

* ✅ GitHub repo link with full Unity project
* ✅ README with explanation of system and features
* ✅ Buildable for Android
* ✅ Well-documented and modular C# code


