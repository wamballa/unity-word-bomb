# AGENTS.md

This is a small Unity falling-word typing game. Keep agent work focused on gameplay scripts and light project docs.

## Scope

- Work primarily in `Assets/*.cs`.
- Do not edit `.meta` files unless the user explicitly asks for Unity asset moves/renames that require them.
- Ignore generated folders: `Library/`, `Temp/`, `Obj/`, `Logs/`, `UserSettings/`.
- Treat `Assets/MobileDependencyResolver/` and `Assets/TextMesh Pro/` as third-party/plugin content.

## Current Script Map

- `Word.cs`: falling word state, typed-letter progress, and TextMeshPro display mutation.
- `Letter.cs`: falling single-symbol bomb/letter state.
- `WordManager.cs`: runtime coordinator for active words, letters, scoring, lives, HUD text, and completion checks.
- `WordSpawner.cs`: prefab spawning and spawn positions.
- `WordTimer.cs`: level spawn timing and counters.
- `LevelManager.cs`: score/lives persistence, scene progression, and hard-coded level config.
- `WordGenerator.cs`: static random word list.
- `LetterGenerator.cs`: static random special-character list.
- `WordInput.cs`: Unity keyboard input bridge into `WordManager`.
- `WordDisplay.cs` and `Word1.cs`: likely legacy/experimental scripts; verify scene/prefab references before deleting.

## Maintenance Rules

- Prefer small, serializable Unity data objects for level tuning rather than hard-coded switch blocks.
- Keep spawn timing, spawn placement, word choice, input handling, HUD updates, and score/lives in separate responsibilities.
- Preserve scene/prefab references when renaming public fields or MonoBehaviour classes.
- Add tests for pure C# rules as they are extracted from MonoBehaviours.
- Commit each completed backlog task with a meaningful message.

## Verification

- Before code commits, run the Unity editor tests if available.
- If Unity CLI is unavailable, at least compile/open the project in Unity and report that limitation.
