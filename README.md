# env0.adventure

**env0.adventure** is a narrative-driven game about interacting with a machine you do not fully understand.

The player begins in a physical space, engaging with a screen, controls, and their surrounding environment. Progress is driven through observation, choice, and interpretation rather than mechanical skill. Over time, the game shifts perspective — from physical interaction, to digital exploration, and eventually to direct engagement with an intentionally limited, unreliable artificial intelligence.

The experience is text-forward, slow-paced, and deliberate. Pauses, emphasis, and restraint are used as narrative tools. The goal is not realism or simulation accuracy, but tension, uncertainty, and meaning derived from partial information.

While the project is built with a strict separation between game logic and presentation, this is a **means**, not the point. The separation exists to support a cohesive experience across multiple narrative chapters that share a single front end, not as a technical exercise in abstraction.

This repository currently focuses on **Chapter 1: Physical Space**.

Terminal-based and AI-driven chapters are planned for later versions and are explicitly out of scope for the current milestones.

---

## Design Principles

- **Narrative first**
  - Systems exist to support pacing, tension, and consequence.
- **Semantic communication**
  - The core expresses meaning (emphasis, rhythm, intent), not UI behavior.
- **Strict separation of concerns**
  - Game logic is engine-agnostic.
  - Godot is responsible only for presentation and input delivery.
- **Playable at every version**
  - Every v0.x milestone produces a runnable, playable build.
- **No premature future features**
  - Terminal and AI systems are deferred until much later versions.

---

## Architecture Overview

### Core Logic
- Drives narrative flow, state, and consequence.
- Emits semantic output (text, emphasis, pacing).
- Requests player input abstractly (choices, free text, continue).
- Has no knowledge of Godot, UI layout, animation, audio, or millisecond timing.

### Front End (Godot)
- Renders output based on semantic metadata.
- Applies pacing, emphasis, and styling rules.
- Presents input exactly as requested by the core.
- Remains reusable across all chapters.

---

## Unified Front-End Contract (v0.x)

All chapters communicate with the front end using the same contract. This contract is intentionally small, semantic, and stable.

### Output

```csharp
enum LineType { Standard, System, Error, Emphasis }
enum Beat { None, Short, Medium, Long }

record OutputLine(
    string Text,
    LineType Type = LineType.Standard,
    Beat Beat = Beat.None
);
```

- **LineType** defines baseline presentation semantics.
- **Beat** indicates a narrative pause before the next line.
- Beats are declarative; the front end maps them to actual timing.

---

### Input Requests

```csharp
enum InputKind { Choice, FreeText, Continue }

record InputRequest(
    InputKind Kind,
    string Prompt,
    IReadOnlyList<ChoiceOption>? Choices
);
```

- The engine requests input.
- The front end never infers interaction mode.

---

### Game Mode

```csharp
enum GameMode { Physical, Terminal, AAI }
```

- Only `Physical` is active in v0.x.
- Included early to stabilise the contract for later chapters.

---

## Version Roadmap

### v0.1 — Core Logic Baseline (Complete)
**Status:** ✅ Complete

**What it is**
- Engine-agnostic narrative core exists.
- State, flow, and interaction logic are functional.
- Content authored externally.
- A playable **.NET console build** exists with text-only interaction.

**What it is not**
- No graphical UI.
- No pacing or presentation rules.
- No UX concerns beyond correctness.

---

### v0.2 — First Playable Loop
**Status:** ⏳ In progress

**What it looks like**
- Godot renders narrative output.
- Emphasis and pacing are respected.
- Player makes choices and advances the story.
- Single gameplay mode: Physical.

**Done when**
- A short physical scene is playable end-to-end without explanation.

---

### v0.3 — Contract Hardening & Authoring Flow

**What it looks like**
- Multi-room / multi-context physical gameplay.
- Stable rendering under real content volume.
- Minimal validation and developer shortcuts.

**Done when**
- Authoring content is faster than breaking the front end.

---

### v0.4 — Feedback & Presentation Rules

**What it looks like**
- Consistent visual response to emphasis and pacing.
- Skip / fast-forward behavior.
- Player-adjustable text speed.
- Optional UI reactions to state changes.

**Done when**
- Pacing and emphasis work without manual tuning.

---

### v0.5 — Chapter 1 Vertical Slice

**What it looks like**
- Complete physical chapter segment (10–20 minutes).
- Stable save/load.
- Front end treated as reusable and stable.

**Done when**
- It can be shared for feedback about the experience, not the tech.

---

## Out of Scope for v0.x

- Terminal gameplay
- Artificial Artificial Intelligence systems
- Engine-specific logic in the core
- Millisecond-based narrative timing
- Feature parity with later chapters

---

## Long-Term Vision (v1.0)

The complete game consists of three chapters:

1. **Physical** — interaction with the machine in real space.
2. **Terminal** — exploration of digital systems.
3. **AAI** — engagement with a deliberately limited, unreliable artificial intelligence.

All chapters share a single front end and communication contract, differing only in their core logic implementation.
