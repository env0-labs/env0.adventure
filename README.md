# env0.adventure

A deliberately minimal, old-school **Choose Your Own Adventure engine** written in C#.

This project is built in small, frozen versions.  
Each version defines a clear endpoint, is completed, and then left alone.

If it feels boring, it’s working.

---

## v0 — Frozen

### What v0 is

v0 is a proof of viability.

It demonstrates that:
- a minimal CYOA loop can exist,
- the engine can be reasoned about end-to-end,
- state, choices, and transitions are explicit,
- and the game terminates cleanly.

v0 is complete and **must not be extended**.

---

### v0 Success Criteria (met)

- One navigable area
- One simple activity
- Explicit start and explicit end
- Numbered choices only
- Disabled choices shown with reasons
- State consists only of:
    - `currentSceneId`
    - boolean flags

Canonical v0 example:
- A hallway
- A locked door
- A kitchen
- The game ends

---

### v0 Constraints (non-negotiable)

- No AI
- No parser / free-text input
- No inventory
- No counters or timers
- No conditional scene text
- No save/load
- No UI
- No engine awareness of success or failure

v0 exists to prove the **shape**, not the scale.

---

## v0.1 — Defined

v0.1 extends v0 **horizontally**, not vertically.

It adds enough interactivity to make JSON authoring worthwhile, without introducing new systems or abstractions.

---

### What v0.1 adds

- Story content authored in **JSON**
- All scenes, choices, and effects loaded at startup
- A small navigable space of **4–5 rooms**
- Basic navigation between rooms
- One simple gated interaction using flags
- One explicit end scene (`IsEnd = true`)

Example scope:
- Hallway
- Kitchen
- Living room
- Bedroom
- Cupboard or utility space

Navigation should feel like moving through a mundane physical space.

---

### What v0.1 keeps the same

- Numbered choices only
- Choices always visible
- Disabled choices shown with a reason
- State remains:
    - `currentSceneId`
    - boolean flags only
- Effects remain limited to:
    - `SetFlag`
    - `ClearFlag`
    - `GotoScene`
- Exactly one scene transition per choice
- Fail-fast on invalid data

The engine remains intentionally dumb.

---

### v0.1 Success Criteria

v0.1 is complete when:

- The player can navigate between **at least 4 rooms**
- At least one route or room is gated by a flag
- The objective can be completed, reaching an end scene
- All story content is loaded from JSON
- Invalid or malformed JSON crashes loudly with a clear error
- Behaviour is predictable and traceable

If adding JSON changes engine behaviour, v0.1 has failed.

---

### What v0.1 explicitly does NOT add

- No parser or verb/noun input
- No inventory system
- No conditional scene text
- No counters, timers, or progression systems
- No save/load
- No UI or rendering work
- No Godot integration
- No attempt at extensibility or tooling

v0.1 exists to prove **authorability**, not flexibility.

---

## Status

- **v0**: complete and frozen on `main`
- **v0.1**: active development branch

The kitchen still exists.  
Now there are a few more rooms.  
Nothing else gets invented.
