# env0.adventure — v0

## What this is

A **minimal, old-school Choose Your Own Adventure engine** written in C#.

This is a deliberately constrained experiment to prove that:
- a tiny, boring core can exist,
- it can be reasoned about easily,
- and it can later be rendered by something like Godot without rewriting the logic.

This is **not** a game yet.  
This is **not** a framework.  
This is a working foundation that can be thrown away without regret.

---

## v0 Success Criteria

The experiment is considered **successful** if all of the following are true:

### Backend
- One navigable area consisting of multiple connected locations
- One simple activity that can be completed
- Pure C# logic with no UI concerns

### Frontend (for later)
- Single static screen
- Simple CRT-style visual treatment
- No audio
- No reactive or dynamic FX

If these conditions are met, v0 is complete.  
Anything beyond this is explicitly out of scope.

---

## Core Design Principles

### 1. Extreme Simplicity
- No AI
- No parser
- No free text input
- No dynamic narrative systems
- No clever abstractions

Choices are **numbered**.  
Players select options like a traditional CYOA book.

---

### 2. Hard Separation of Concerns

- **Engine**: game logic and state transitions
- **Model**: pure data definitions
- **Runtime**: orchestration / harness code

There is **no UI code** in this project.  
Godot (or any other renderer) will be a *dumb* consumer of engine output later.

---

### 3. Explicit State Only

Game state consists of:
- `currentSceneId`
- a dictionary of boolean flags

There are:
- no counters
- no timers
- no implicit history
- no notion of time

If something matters, it is made explicit.

---

### 4. Static Scenes, Dynamic Choices

- Scene text is static and never changes
- All variability lives in **choices**
- Choices are always visible
- Choices may be disabled, with a reason shown

No hidden options.  
No guessing what the engine is doing.

---

### 5. Ordered Effects, No Logic Inside Data

Choices execute an **ordered list of effects**, such as:
- set a flag
- clear a flag
- move to another scene

Rules:
- exactly one scene transition per choice
- scene transition must be last
- no conditionals inside effects
- no loops
- no reuse

The engine is dumb by design.

---

### 6. Explicit Start and End

- The game starts at an explicitly defined scene
- The game ends when an `IsEnd = true` scene is reached
- The engine has no concept of success or failure

Meaning belongs to the story, not the engine.

---

### 7. Fail Fast, Always

Invalid data causes the program to crash loudly:
- missing scenes
- invalid transitions
- malformed effects

This is intentional.  
This is a developer tool, not a consumer product.

---

## Current State (v0)

At the v0 endpoint, the project will:

- Run as a console application
- Present a scene description
- Show numbered choices
- Allow the player to select a choice
- Execute effects
- Transition between scenes
- Reach an end scene and terminate

The canonical v0 example is:
- A hallway
- A kitchen
- A locked door
- A single successful interaction
- The game ends

Nothing more.

---

## What This Is Not (Yet)

Explicitly out of scope for v0:

- Free-text input
- Verb/noun parsing
- Inventory systems
- AI or procedural narrative
- Save/load
- Time-based mechanics
- env0.terminal integration
- Reuse systems or templates

These may be explored later, but **not as extensions of v0**.

---

## Why This Exists

This project exists to:
- regain momentum after over-engineering
- prove a minimal core can work
- provide a stable base for later experimentation
- avoid premature abstraction

If v0 feels boring, it’s working.

---

## Next Steps (after v0)

Possible future directions (not commitments):
- JSON-authored story graphs
- Godot-based renderer
- Alternative input modes
- Separate game states (e.g. terminal mode)
- More complex interactions layered *on top*, not baked in

None of these affect the definition of v0.

---

## Status

**v0 is considered complete when the kitchen is reachable and the game ends.**

No more.  
No less.
