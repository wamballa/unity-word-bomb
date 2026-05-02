# FW-001 SOLID Code Review

## Summary

The core game loop is small enough to rescue cleanly, but the current scripts blur responsibilities in ways that make level tuning and spawner behavior hard to reason about. The highest-value refactor is to move configuration and spawning rules out of scene managers before adding levels.

## Findings

1. `Assets/WordManager.cs:52` mixes input matching, active-word state, object destruction, score updates, letter bomb handling, and level-complete checks. This violates single responsibility and makes future dial/bomb rules risky because every input mechanic has to pass through one large method.

2. `Assets/LevelManager.cs:74` hard-codes level tuning in an `out`-parameter switch. This violates open/closed: every new level requires editing manager code, and tuning cannot be inspected or adjusted as Unity data.

3. `Assets/WordTimer.cs:40` combines timer scheduling, level config consumption, spawn counters, and completion signaling. It also appears to set `lastWordDropped = true` when letter drops complete at line 64, while `_letterCounter` is never incremented, so letter-drop completion can be incorrect.

4. `Assets/Word.cs:18` chooses its own random word in `Start`. That couples entity display/lifetime to word selection, making it difficult for levels, difficulty rules, or the dial to know which letters are coming before the prefab exists.

5. `Assets/LetterGenerator.cs:7` still generates special characters, including a mojibake pound symbol (`Â£`), while the current design says the dial should contain letters from dropped words. This is a likely design drift bug and an interface-segregation smell: "letter" and "bomb symbol" are currently the same concept.

6. `Assets/WordManager.cs:137` and `Assets/WordManager.cs:151` remove list items while iterating forward. Consecutive off-screen items can be skipped after `RemoveAt`, causing missed life loss or stale objects.

7. `Assets/LevelManager.cs:16` uses `DontDestroyOnLoad` without preventing duplicates. Returning to the start/main scene can create multiple managers with divergent score/lives state.

8. `Assets/WordDisplay.cs` and `Assets/Word1.cs` look like legacy alternatives to `Word.cs`. Keeping unused MonoBehaviours raises maintenance noise; verify scene/prefab references and remove or fold them into the active implementation.

## Recommended Refactor Order

1. Create a serializable `LevelConfig` model and move the existing two levels into data.
2. Change `WordSpawner` to accept the word/letter content and a spawn profile instead of letting spawned prefabs choose randomly.
3. Extract an input resolver that can be tested without Unity objects.
4. Move HUD rendering behind a small view script so `WordManager` can become an orchestration layer.
5. Rebuild bombs around the dial/letter pile design after the word-spawn pipeline exposes upcoming letters.
