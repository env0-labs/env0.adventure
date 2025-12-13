# env0.adventure

A narrative-driven game built around a strict separation between **game logic** and **front-end presentation**, designed to support multiple gameplay “chapters” using a shared UI and interaction contract.

This repository currently focuses on **Chapter 1: Physical Space**, where the player interacts with a machine while moving through a physical environment.

Terminal-based and AI-driven chapters are planned for later versions and are **explicitly out of scope** for the current milestones.

---

## Design Principles

- **Strict separation of concerns**
  - Game logic is engine-agnostic.
  - Godot is responsible only for presentation and input delivery.
- **Semantic, not procedural, hooks**
  - The core expresses meaning (emphasis, rhythm, intent).
  - The front end decides how that meaning is rendered.
- **Playable at every version**
  - Every v0.x milestone produces a runnable, playable build.
- **No premature future features**
  - Terminal and AI systems are deferred until much later versions.

---

## Architecture Overview

### Core Logic
- Emits semantic output events (text, emphasis, pacing).
- Requests player input via abstract input requests.
- Maintains game state and narrative flow.
- Has no knowledge of Godot, UI layout, animation, audio, or timing in milliseconds.

### Front End (Godot)
- Renders output lines based on semantic metadata.
- Applies pacing, emphasis, and styling rules.
- Presents input (choices, free text) as requested by the core.
- Can swap core implementations without changing UI code.

---

## Unified Front-End Contract (v0.x)

All chapters communicate with the front end using the same contract.

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

- **LineType** controls baseline presentation.
- **Beat** indicates a narrative pause before the next line.
- Beats are semantic; the front end maps them to actual delays.

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
- The front end never guesses interaction mode.

---

### Game Mode

```csharp
enum GameMode { Physical, Terminal, AAI }
```

- Currently only `Physical` is used.
- Included early to stabilise the contract for future chapters.

---

## Version Roadmap

### v0.1 — Core Logic Baseline (Complete)
**Status:** ✅ Complete

**What it is**
- Engine-agnostic core logic exists.
- Narrative flow, state handling, and interaction model are functional.
- Content can be authored externally.
- No dependency on Godot or any front-end.
- Output and input are conceptual, not yet rendered.

**What it is not**
- No playable Godot build.
- No pacing, emphasis, or presentation rules.
- No concern for UX beyond correctness.

**Completion meaning**
- The narrative engine exists as a coherent, testable system.
- The project moves from “engine experiment” to “game integration.”

---

### v0.2 — First Playable Loop
**Status:** ⏳ In progress

**What it looks like**
- Godot renders output lines from the core.
- Semantic output supported:
  - `LineType`: `Standard | System | Error | Emphasis`
  - `Beat`: `None | Short | Medium | Long`
- Player can make choices and advance the narrative.
- Single gameplay mode: `Physical`.

**Done when**
- A short physical scene is playable end-to-end without explanation.

---

### v0.3 — Contract Hardening & Authoring Flow

**What it looks like**
- Multi-room / multi-context physical gameplay.
- Godot reliably renders larger authored content sets.
- Minimal content validation (fail fast, fail loud).
- Basic dev shortcuts (restart, jump to node, dump state).

**Done when**
- Content authoring speed exceeds front-end breakage.

---

### v0.4 — Feedback & Presentation Rules

**What it looks like**
- Consistent visual and pacing reactions to `LineType` and `Beat`.
- Skip / fast-forward behaviour.
- Text speed scaling.
- Optional UI responses to state changes.

**Done when**
- Pacing and emphasis land without manual babysitting.

---

### v0.5 — Chapter 1 Vertical Slice

**What it looks like**
- Complete physical chapter segment (10–20 minutes).
- Stable save/load.
- Front end treated as reusable and stable.
- Contract changes become deliberate and rare.

**Done when**
- It can be shared for real feedback about the game, not the tech.

---

## Out of Scope for v0.x

- Terminal gameplay (`env0.terminal`)
- Artificial Artificial Intelligence systems
- Engine-specific logic in the core
- Millisecond-based timing in narrative
- Feature parity with planned v1.0 chapters

---

## Long-Term Vision (v1.0)

The full game will consist of three chapters:

1. **Physical** — interaction with the machine in real space.
2. **Terminal** — exploration of digital systems.
3. **AAI** — interaction with a deliberately limited, janky artificial intelligence.

All chapters will share the same front end and communication contract, differing only in the core logic implementation.
