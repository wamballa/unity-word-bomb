# WB-001 SOLID Code Review

## Summary
The core loop is playable in shape, but the code is hard to tune because level data, spawn cadence, radial letter sets, heap danger, score, scene flow, and word creation are still coupled through MonoBehaviours. The first refactor should make spawning and level tuning data-driven before adding levels.

## Findings

### Single Responsibility
- `Assets/Scripts/Core/GameManager.cs` owns score, high score, pause/mute settings, difficulty progression, radial letter set selection, heap danger checks, scene restart, app rating, and radial menu setup. This makes level tuning risky because changing spawn/difficulty settings can accidentally affect UI, persistence, or scene flow.
- `Assets/Scripts/Managers/ObjectSpawner.cs` owns spawn timing, spawn position calculation, prefab creation, object registration, and word/number type decisions. Extracting schedule and bounds logic would make spawner tuning much easier.
- `Assets/Scripts/Word/FallingWord.cs` owns movement, crash detection, word selection, radial word lookup, visual updates, audio, and typed completion. The word should receive a chosen word and fall config rather than find global services during `Start`.

### Open/Closed
- `GameManager.GetFallSpeed(string type)` and `GetFallDelayTime(string type)` use string selectors, so adding a new falling object type or special spawn rule requires editing central logic and risks typo-driven defaults. Prefer an enum or data asset keyed by spawn category.
- Radial letter sets are hard-coded in `GameManager`. New levels should be added through `ScriptableObject` level definitions or serialized assets, not source edits.

### Liskov Substitution
- Word and number falling objects share concepts but not a shared contract: spawn, fall, crash, become removable. A small interface such as `IFallingEntity` or separate lifecycle events would allow managers to track objects without assuming concrete components everywhere.

### Interface Segregation
- `IGameplayInputReceiver` is a good start because dial, keyboard, and future input can route to the same gameplay surface.
- `WordGameplayManager` still depends on concrete `GameObject`, `FallingWord`, `NumberController`, and `FeedbackManager.Instance`. Splitting word targeting, number matching, and feedback reactions would keep input handling narrow.

### Dependency Inversion
- Many scripts find dependencies at runtime with `FindFirstObjectByType`, `GameObject.Find`, or static globals (`InputRouter.Receiver`, `GameEvents`). This makes tests and prefab reuse harder. Prefer serialized references for scene services and constructor/setter injection for pure classes.
- `RadialWordLoader` is directly loaded by `FallingWord`, so word generation depends on scene lookup rather than an abstraction like `IWordProvider`.

## Refactor Order
1. Create level/spawn config assets for radial letters, word lengths, fall speeds, spawn delays, number/bomb frequency, and difficulty progression.
2. Refactor `ObjectSpawner` to consume config and delegate spawn positions/schedules to small helper classes.
3. Move word picking out of `FallingWord`; spawn should assign the selected word before the object starts falling.
4. Split `GameManager` into small services for score/settings, danger tracking, difficulty progression, and scene lifecycle.
5. Reintroduce bombs as an input/spawn feature after the dial vocabulary and level data are stable.
