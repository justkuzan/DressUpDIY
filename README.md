# 💄 Dress Up DIY — Makeup Mechanic

> Test assignment completed (Unity Developer position, March 2026).
> Fully implemented makeup interaction system for a mobile dress-up game — cream, eyeshadow, lipstick, blush, and a reset sponge.

![Gameplay demo](demo.gif)

---

## Task Requirements

The assignment was to implement a makeup application mechanic from scratch using provided assets, matching a reference game's interaction design.

Required mechanics:
- **Cream** — drag hand to face zone, remove acne (sprite swap)
- **Eyeshadow** — pick color from palette, brush tip changes color, apply to character
- **Lipstick** — pick up, drag to face, change lip color
- **Blush** — same drag-and-apply flow
- **Sponge** — one-tap full makeup reset, no animation

Interaction rules for all items:
- Hand movement controlled via `onDrag` to the face zone
- Drop outside face zone → nothing happens
- Drop inside face zone → trigger apply animation, then auto-return hand to default position
- Smooth transitions between all states — no snapping or jitter

---

## Demo

> No live build available. See the GIF above for a full walkthrough of all mechanics.

---

## What I Built Beyond the Requirements

- **Full responsive layout** — adapted for all screen sizes including tablets, using UGUI anchors and a `SafeArea` handler for notched devices
- **ScriptableObject-based item config** (`MakeupItemSO`) — each cosmetic item's type, color, sprites, and animation data is defined as a SO, making the system data-driven and easy to extend without touching code
- **Book tabs UI** (`BookTabsController`) — tabbed navigation between makeup categories with smooth switching
- **ShaderLab / HLSL** — custom shader for brush tip color tinting, avoiding per-color texture swaps

---

## Tech Stack

| | |
|---|---|
| **Engine** | Unity (C#) |
| **Platform** | Android (APK) |
| **UI System** | UGUI with anchors-based responsive layout |
| **Data** | ScriptableObjects (`MakeupItemSO`) |
| **Rendering** | ShaderLab, HLSL |
| **Interaction** | EventSystem, `IDragHandler`, `IPointerUpHandler` |

---

## Project Structure

```
DressUpDIY/
└── Assets/
    └── _Project/
        ├── Scripts/
        │   ├── Core/
        │   │   ├── CharacterFace.cs      # Face zone detection, sprite swapping
        │   │   ├── HandController.cs     # Drag logic, state transitions, animations
        │   │   └── MakeupItemSO.cs       # ScriptableObject — item config
        │   ├── MakeupData/
        │   │   ├── Blush/
        │   │   ├── Cream/
        │   │   ├── Eyeshadow/
        │   │   └── Lipstick/             # Per-mechanic logic scripts
        │   └── UI/
        │       ├── BookTabsController.cs # Tab navigation between makeup categories
        │       └── MakeupButton.cs       # Item button — triggers pickup flow
        ├── Prefabs/
        ├── Scenes/
        └── Sprites/
```

---

## State Flow

```
Idle
 └─► [Player taps item] → MakeupButton triggers HandController
       └─► PickUp animation plays
             └─► Hand fixed at chest level
                   └─► [Player drags via IDragHandler]
                         ├─► Release outside CharacterFace zone → return to Idle
                         └─► Release inside CharacterFace zone
                               └─► Apply animation → sprite/color swap → return → Idle
```

---

## About

Built by **Anton Kuzan** — Unity Developer with a background in UI/UX design.

- [LinkedIn](https://www.linkedin.com/in/antonkuzan)
- [GitHub](https://github.com/justkuzan)
- [Behance](https://www.behance.net/akuzan)
