# Combat and Weapon System

This document separates the shared, editor-authored definitions from the mutable objects created for each player at runtime.

## System Map

```mermaid
classDiagram
    direction LR

    class CombatActionDefinition {
        <<ScriptableObject>>
        +string ActionId
        +CombatActionType ActionType
        +float StartupDuration
        +float ActiveDuration
        +float RecoveryDuration
        +float BaseDamage
        +string HitboxId
        +float CooldownSeconds
        +int ChargeCost
        +int AmmoCost
        +bool AllowMultipleHits
    }

    class MainActionSet {
        <<ScriptableObject>>
        +CombatActionDefinition[] ComboActions
        +float ComboResetTime
        +GetAction(comboIndex) CombatActionDefinition
    }

    class WeaponDefinition {
        <<ScriptableObject>>
        +string WeaponId
        +MainActionSet MainActionSet
        +CombatActionDefinition OffHandAction
        +float BaseDamageMultiplier
        +float AttackSpeedMultiplier
        +GameObject EquippedVisualPrefab
        +RuntimeAnimatorController AnimatorOverrideController
    }

    class WeaponInstance {
        <<runtime>>
        +WeaponDefinition Definition
        +WeaponRuntimeState RuntimeState
        +GetCurrentMainAction(time) CombatActionDefinition
        +CanUseMainAction(action) bool
        +CanUseOffHandAction(action) bool
        +MarkMainActionUsed(action, time)
        +MarkOffHandActionUsed(action, time)
    }

    class WeaponRuntimeState {
        +float MainActionCooldownRemaining
        +float OffHandCooldownRemaining
        +int CurrentCharges
        +int CurrentAmmo
        +int CurrentComboIndex
        +float LastMainActionTime
        +Tick(deltaTime)
        +SpendResources(chargeCost, ammoCost)
    }

    class PlayerWeaponLoadout {
        <<MonoBehaviour>>
        +WeaponInstance SlotA
        +WeaponInstance SlotB
        +WeaponSlot EquippedSlot
        +SwapWeapons() bool
        +EquippedWeaponChanged event
    }

    class PlayerCombatController {
        <<MonoBehaviour>>
        +CombatState CurrentState
        +bool IsActionLocked
        +TryUseMainAction() bool
        +TryUseOffHandAction() bool
        +TrySwapWeapons() bool
    }

    class CombatActionExecutor {
        <<MonoBehaviour>>
        +bool IsExecuting
        +ExecuteAction(attacker, weapon, action) bool
        -ExecuteRoutine()
        -ProcessActiveHits()
        -BuildHitData() HitData
    }

    class HitboxAuthoring {
        <<MonoBehaviour>>
        +string HitboxId
        +Collider HitCollider
    }

    class HitboxUtility {
        <<static>>
        +TryBuildQuerySpec() bool
    }

    class HitDetectionUtility {
        <<static>>
        +QueryNonAlloc(spec, results) int
    }

    class HitData {
        <<struct>>
        +GameObject Attacker
        +GameObject Target
        +float Damage
        +Vector3 HitPoint
        +CombatActionDefinition SourceAction
        +WeaponInstance SourceWeaponInstance
    }

    class IDamageable {
        <<interface>>
        +TakeHit(hit)
    }

    class HealthComponent {
        <<MonoBehaviour>>
        +TakeHit(hit)
    }

    class CombatAnimatorBridge {
        <<MonoBehaviour>>
        +PlayAction(action)
        +ApplyWeaponAnimator(weapon)
        +PlayWeaponSwap()
    }

    class WeaponVisualController {
        <<MonoBehaviour>>
        +RefreshEquippedVisual(weapon)
    }

    MainActionSet "1" o-- "0..*" CombatActionDefinition : combo actions
    WeaponDefinition "1" --> "0..1" MainActionSet : main actions
    WeaponDefinition "1" --> "0..1" CombatActionDefinition : off-hand action
    WeaponInstance "*" --> "1" WeaponDefinition : shared definition
    WeaponInstance "1" *-- "1" WeaponRuntimeState : owns
    PlayerWeaponLoadout "1" *-- "2" WeaponInstance : slots
    PlayerCombatController --> PlayerWeaponLoadout
    PlayerCombatController --> CombatActionExecutor
    PlayerCombatController --> CombatAnimatorBridge
    CombatActionExecutor --> HitboxUtility
    HitboxUtility --> HitboxAuthoring : resolves by ID
    CombatActionExecutor --> HitDetectionUtility
    CombatActionExecutor ..> HitData : creates
    IDamageable <|.. HealthComponent
    CombatActionExecutor --> IDamageable : TakeHit
    PlayerWeaponLoadout --> WeaponVisualController : change event
    PlayerWeaponLoadout --> CombatAnimatorBridge : change event
```

## Development-Time Authoring

Definitions are project assets and should remain immutable during play. Runtime counters belong to `WeaponRuntimeState`, not the assets.

```mermaid
flowchart LR
    A[Create CombatActionDefinition assets] --> B[Set timing, damage, costs, animation, and hitbox ID]
    B --> C[Create MainActionSet asset]
    C --> D[Order actions as a combo]
    D --> E[Create WeaponDefinition asset]
    E --> F[Assign main set, off-hand action, multipliers, visual, and animator]
    F --> G[Assign definitions to PlayerWeaponLoadout slots]
    G --> H[Add matching HitboxAuthoring components to attacker hierarchy]
    H --> I[Wire PlayerInput, executor, animator bridge, visual socket, and target layers]
    I --> J[Enter Play Mode]

    subgraph Example[Current Unarmed assets]
        P1[Punch_1] --> US[Unarmed_Main]
        P2[Punch_2] --> US
        US --> UW[Unarmed]
        P1 -->|off-hand action| UW
    end
```

## Runtime Object Creation and Updates

```mermaid
flowchart TD
    A[Player prefab awakens] --> B[Loadout ensures state for Slot A and Slot B]
    B --> C[Controller finds loadout, executor, animator bridge, and PlayerInput]
    C --> D[Controller subscribes to input and weapon-change events]
    D --> E[Visual controller spawns equipped weapon visual]
    E --> F[Every Update]
    F --> G[Tick both WeaponInstances]
    G --> H[Reduce main and off-hand cooldowns toward zero]
    H --> F
```

## Main Attack Execution

```mermaid
sequenceDiagram
    actor Player
    participant Input as PlayerInput
    participant Controller as PlayerCombatController
    participant Loadout as PlayerWeaponLoadout
    participant Weapon as Equipped WeaponInstance
    participant Executor as CombatActionExecutor
    participant Hitbox as HitboxUtility
    participant Physics as HitDetectionUtility / Physics
    participant Target as IDamageable

    Player->>Input: MainAttack performed
    Input->>Controller: TryUseMainAction()
    Controller->>Loadout: EquippedWeapon
    Loadout-->>Controller: WeaponInstance
    Controller->>Weapon: GetCurrentMainAction(Time.time)
    Weapon-->>Controller: action at CurrentComboIndex
    Controller->>Weapon: CanUseMainAction(action)

    alt blocked, busy, cooling down, or lacks resources
        Weapon-->>Controller: false
        Controller-->>Input: action rejected
    else action can start
        Controller->>Executor: ExecuteAction(player, weapon, action)
        Executor-->>Controller: true
        Controller->>Controller: set state to Attacking and apply action lock
        Controller->>Weapon: MarkMainActionUsed(action, Time.time)
        Weapon->>Weapon: start cooldown, spend resources, advance combo

        Executor->>Executor: play trigger and SFX, then wait StartupDuration
        loop each frame of ActiveDuration (at least once)
            Executor->>Hitbox: TryBuildQuerySpec()
            alt matching HitboxAuthoring exists
                Hitbox-->>Executor: collider-derived query
            else no matching hitbox
                Hitbox-->>Executor: action fallback query
            end
            Executor->>Physics: QueryNonAlloc(query, buffer)
            Physics-->>Executor: overlapping colliders
            loop each valid, non-self target
                Executor->>Executor: build HitData and apply weapon damage multiplier
                Executor->>Target: TakeHit(hitData)
            end
        end
        Executor->>Executor: wait RecoveryDuration
        Executor->>Controller: OnActionFinished()
        Controller->>Controller: set state to Idle and clear action lock
    end
```

Off-hand execution follows the same sequence, except it selects the **inactive** weapon's `OffHandAction` and uses that weapon's off-hand cooldown.

## Action Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Idle
    Idle --> Startup: accepted input
    Startup --> Active: StartupDuration elapsed
    Active --> Active: query hits each frame
    Active --> Recovery: ActiveDuration elapsed
    Recovery --> Idle: RecoveryDuration elapsed / callback

    note right of Active
        One HashSet is kept for the whole action.
        Unless AllowMultipleHits is true,
        each IDamageable is hit once.
    end note
```

The controller currently reports `CombatState.Attacking` for startup, active, and recovery. The lifecycle above describes the executor coroutine rather than distinct controller states.

## Weapon Swap

```mermaid
sequenceDiagram
    actor Player
    participant Controller as PlayerCombatController
    participant Loadout as PlayerWeaponLoadout
    participant Visuals as WeaponVisualController
    participant Animator as CombatAnimatorBridge

    Player->>Controller: SwapWeapons input
    Controller->>Controller: reject if locked, attacking, or recovering
    Controller->>Loadout: SwapWeapons()
    Loadout->>Loadout: require definitions in both slots
    Loadout->>Loadout: toggle EquippedSlot
    Loadout-->>Visuals: EquippedWeaponChanged(weapon)
    Visuals->>Visuals: destroy old visual, then instantiate equipped prefab
    Loadout-->>Controller: EquippedWeaponChanged(weapon)
    Controller->>Animator: ApplyWeaponAnimator(weapon)
    Controller->>Animator: PlayWeaponSwap()
```

## Hit Detection Decision

```mermaid
flowchart TD
    A[Active action frame] --> B{Find HitboxAuthoring with action.HitboxId?}
    B -->|yes| C{Collider type}
    C -->|BoxCollider| D[Build box overlap query]
    C -->|SphereCollider| E[Build sphere overlap query]
    C -->|CapsuleCollider| F[Build capsule overlap query]
    C -->|unsupported| X[Skip this frame]
    B -->|no| G[Build fallback shape from action and origin]
    D --> H[Physics overlap, non-allocating]
    E --> H
    F --> H
    G --> H
    H --> I[Ignore attacker hierarchy]
    I --> J[Find IDamageable in collider parent]
    J --> K{Already hit and multiple hits disabled?}
    K -->|yes| L[Skip target]
    K -->|no| M[Build HitData, call TakeHit, play hit effects]
```

## Current Implementation Notes

- `WeaponDefinition.AttackSpeedMultiplier` is authored but is not applied to startup, active, or recovery timing.
- `LockMovement`, `CanRotateDuringAction`, and `CanStartInAir` are authored but are not consumed by the shown runtime classes.
- `CombatState.Recovering` is checked but never assigned; the controller remains `Attacking` until the executor completes recovery.
- `CanBeCancelled` and `CanInterruptRecovery` cannot currently start a replacement action because `CombatActionExecutor.IsExecuting` rejects it first.
- An action trigger can be sent by both `CombatAnimatorBridge` and `CombatActionExecutor` when both reference an animator.
- `AllowMultipleHits` bypasses the per-action de-duplication set, so a target may receive a hit on every active frame.
- `Hitbox` is a legacy trigger-driven path; new actions use `CombatActionExecutor` and physics overlap queries.
