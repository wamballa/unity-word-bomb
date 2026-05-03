# AGENTS.md

## Scope
- Work on game scripts in `Assets/Scripts/**`.
- Ignore Unity-generated/cache folders: `Library/`, `Temp/`, `obj/`, `Logs/`, `Build/`, `Build_BurstDebugInformation_DoNotShip/`.
- Do not edit `.meta` files unless a task explicitly requires Unity asset creation, move, or deletion.
- Treat package/vendor/sample code as out of scope unless a script in `Assets/Scripts/**` directly depends on it.

## Current Game Shape
- Core state and tuning: `Assets/Scripts/Core/GameManager.cs`.
- Word spawning: `Assets/Scripts/Managers/ObjectSpawner.cs`.
- Typed word flow: `Assets/Scripts/GameLogic/WordGameplayManager.cs`.
- Falling word behaviour: `Assets/Scripts/Word/FallingWord.cs`, `WordTyping.cs`, `WordVisual.cs`, `RadialWordLoader.cs`.
- Dial input: `Assets/Scripts/Input/InputRouter.cs`, `RadialSwipeDrawer.cs`, plus radial menu scripts.
- Number/bomb flow: `Assets/Scripts/Number/**`.

## Working Rules
- Keep changes small and task-scoped.
- Backlog task IDs live in `Assets/BACKLOG.md`; create one meaningful commit for each completed task.
- Prefer data-driven tuning over hard-coded spawn/difficulty values.
- Preserve user scene/prefab changes unless the task explicitly asks to change them.
- Before refactoring, check serialized public fields and scene references to avoid breaking Unity inspector wiring.

## Review Bias
- Use SOLID as the maintenance lens.
- Separate data/config from runtime orchestration.
- Keep MonoBehaviours thin where practical; move pure selection/tuning logic into testable classes or ScriptableObjects.
- Avoid new global/static coupling unless it replaces worse coupling and has a clear owner.
