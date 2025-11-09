# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 2D turn-based RPG game called "salnomsal" built with Universal Render Pipeline (URP). The game features:
- Turn-based combat with mini-games for actions (dodge, parry, match patterns)
- Map-based progression system with node-based navigation
- Equipment and inventory system with stat modifiers
- Party management with up to 3 playable characters
- Character customization with portraits and equipment slots

## Key Dependencies

- **Unity Version**: Unity 6 (URP 17.0.4)
- **Scripting Defines**: `DOTWEEN`, `UNITY_POST_PROCESSING_STACK_V2`, `USING_URP` (Standalone), `MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED`
- **Core Packages**:
  - DOTween (animation tweening)
  - Unity Input System (1.14.2)
  - Cinemachine (3.1.3)
  - Post Processing (3.5.0)
  - More Mountains Feedbacks (Feel system)
  - 2D Animation (10.2.1)
  - Addressables (asset management and dynamic loading)

## Architecture

### Manager Pattern (Singleton-based)
The game uses singleton managers that persist via `DontDestroyOnLoad`:

- **GameManager**: Central state holder for team composition, character data, and expedition status
  - Stores `TeamSlots[3]`, `Characters[3]`, `PortraitItems[3]`, `CharacterStats[3]`
  - Tracks alive status per character index
  - Location: `Assets/02.Scripts/Manager/GameManager.cs`

- **CombatManager**: Handles turn-based combat flow
  - Manages `TurnOrder` (priority queue based on speed stats)
  - Coordinates player actions, target selection, and skill execution
  - Controls combat UI and enemy spawning
  - Location: `Assets/02.Scripts/Combat/Manager/CombatManager.cs`

- **MapManager**: Controls map-based progression
  - Manages current node, map generation, and node traversal
  - Handles background and BGM randomization per node
  - Location: `Assets/02.Scripts/Map/Manager/MapManager.cs`

- **EquipmentManager**: Handles equipment instances and persistence
  - Loads/saves equipment data via `InventoryRepository`
  - Uses event system (`OnDataChanged`) for UI updates
  - Location: `Assets/02.Scripts/Equipments/3.Manager/EquipmentManager.cs`

### Character System
**Inheritance Hierarchy**: `Character` (abstract) → `PlayableCharacter` / `EnemyCharacter`

**Key Interfaces**:
- `ITurnActor`: Turn-based units (`StartTurn()`, `EndTurn()`, speed properties)
- `ITargetable`: Entities that can be targeted (health, buffs, damage handling)

**Character Features**:
- Stats: MaxHealth, MaxCost (mana), AttackPower, Speed, Resistance
- Action delegates: `OnTurnStart`, `OnTurnEnd` (for buffs/debuffs)
- Equipment integration: Characters apply `StatModifier` from weapons/armor
- Animation-driven actions: Coroutines wait for animator state completion

**PlayableCharacter** (`Assets/02.Scripts/PlayableCharacters/PlayableCharacter.cs`):
- 3 skill slots (DefaultAttack, Skill1, Skill2)
- Camera movement and DOTween sequences for attacks
- Projectile support with muzzle positions
- Equipment stats applied via `ApplyItems()` at Start

**EnemyCharacter** (`Assets/02.Scripts/Monster/EnemyCharacter.cs`):
- Abstract methods: `Attack()`, `Skill1()`, `Skill2()`, `Skill3()`, `Skill4()`
- Concrete implementations define enemy behavior patterns

### Asset Management & Addressables

**Problem**: Initially, the character system used ScriptableObjects (SO) to store character data including portrait images. However, this approach caused reference issues when trying to load portrait images at runtime, particularly when:
- Images needed to be accessed across different scenes
- ScriptableObject references broke during scene transitions
- Direct SO references created tight coupling between data and assets

**Solution**: Migrated to Unity Addressables system for dynamic asset loading

**Implementation**:
- Portrait images registered as Addressable assets with unique keys
- Runtime loading via `Addressables.LoadAssetAsync<Sprite>(key)`
- Decoupled asset references from ScriptableObject data
- Improved memory management with async loading/unloading

**Benefits**:
- **Flexible Asset Management**: Assets loaded on-demand rather than bundled with scenes
- **Reduced Memory Footprint**: Load portraits only when needed, release when done
- **Build Optimization**: Addressables can be built into separate asset bundles
- **Scalability**: Easy to add new character portraits without rebuilding ScriptableObjects
- **Cross-Scene Reliability**: Asset references maintained across scene transitions

**Portfolio Note**: This demonstrates understanding of Unity's asset management systems and problem-solving approach when ScriptableObject limitations were encountered. The migration to Addressables showcases knowledge of:
- Async/await patterns for asset loading
- Memory optimization techniques
- Unity's modern asset pipeline
- Architectural decision-making based on technical constraints

### Equipment & Inventory System
Uses a **Domain-Driven Design** approach with layered structure:

**Layers**:
1. **Domain**: Core models (`InventoryDDD`, `SlotDDD`, `CharacterStat`)
2. **Repository**: Data persistence (`InventoryRepository`)
3. **Manager**: Business logic (`EquipmentManager`, `InventoryManager`)
4. **UI**: Presentation layer (`UI_Inventory`, `UI_InventorySlot`, `UI_ChestInventory`)

**Equipment Flow**:
- `EquipmentSO` (ScriptableObject) defines base equipment templates
- `EquipmentInstance` represents runtime equipment with randomized stats
- `EquipmentGenerator` creates instances with randomized `StatModifier` ranges
- `StatModifier` contains `StatType` (Attack, MaxHealth, MaxMana, Speed) and `Value`
- Equipment saved as `EquipmentSaveData` via `SerializableEquipmentData`

**Equipment Directory**: `Assets/02.Scripts/Equipments/`
- Note: `1.Domain.meta` was deleted (git status shows `D Assets/02.Scripts/Equipments/1.Domain.meta`)

### Combat Flow

**Turn Order System**:
1. Initialize combat: `InitializeCombat()` populates `TurnOrder` with all actors
2. Sort by `CurrentSpeed` (descending) using `SetOrder()`
3. Execute turn: `StartTurn()` → player/AI action → `EndTurn()`
4. Speed increment: All waiting actors gain `SpeedIncrementPerTurn` to speed
5. Re-add completed actor to `TurnOrder` and re-sort

**Player Turn**:
1. Select skill slot (`SetSelectedSkill`)
2. Outline valid targets based on `TargetType` (Enemy/Ally) and `SkillRange` (Single/Global)
3. Click target (`SetTarget`) → validate → execute `DoAction()`
4. Play animation sequence with camera movement
5. Apply skill effects in `WaitAnimationEnd` coroutine
6. Return to position and `EndTurn()`

**Input Blocking**: `_isInputBlocked` flag prevents actions during animations

### Mini-Game System
Mini-games provide interactive skill execution:

**Game Types**:
- **Avoid Game**: Dodge incoming projectiles with shield/movement (`Assets/02.Scripts/MiniGame/AvoidGame/`)
- **Match Game**: Pattern matching sequences (`Assets/02.Scripts/MiniGame/MatchGame/`)
- **Parry Game**: Timing-based slash deflection (`Assets/02.Scripts/MiniGame/ParryingGame/`)

**Manager**: `MiniGameScenesManager` handles scene transitions between combat and mini-games

### Map System
**Node-Based Progression** (`Assets/02.Scripts/Map/`):

- `MapGenerator`: Procedural map generation with seed-based randomization
- `MapNode`: Graph structure with `Parents` and `Children` connections
- `NodeType`: Different encounter types (Combat, Boss, Rest, Shop, etc.)
- Navigation: `SetCurrentNode()` activates only valid child nodes

**UI Components**:
- `UI_MapNode`: Visual representation of map nodes
- `UI_LineDrawer`: Connects nodes visually
- `UI_MapGenerator`: Controls map display and generation parameters

### Skills System
**Skill Types**: Attack, Heal, Buff, Stun
**Skill Range**: Single, Global
**Target Types**: Enemy, Ally

**Skill Components** (`Assets/02.Scripts/Skills/`):
- `SkillDataSO` / `PlayableSkillSO`: ScriptableObject skill definitions
- `Skill`: Runtime skill wrapper with cost and cooldown
- Concrete implementations: `Attack`, `Heal`, `Buff`, `Stun`

**Skill Execution**:
- Skills use `UseSkill(caster, target)` pattern
- Damage calculation includes critical chance/damage
- Buffs register callbacks to `OnTurnStart`/`OnTurnEnd`

## Scene Structure

**Main Scenes** (`Assets/01.Scenes/`):
- `SalnomSalTitleMenu.unity`: Main menu entry point
- `Village.unity`: Character management and equipment hub
- `StartMapScene.unity`: Map navigation and node selection
- `BattleScene.unity`: Turn-based combat
- `AvoidScene.unity`, `ParryingScene.unity`, `MagicScene.unity`: Mini-games
- `Boss.unity`: Boss encounter scene

**Scene Flow**:
Title → Village (party setup) → Map (node selection) → Battle/MiniGame → Map (repeat) → Boss

## Data Persistence

**Save System**:
- Uses Unity `PlayerPrefs` via JSON serialization
- `JsonHelper` utility for array serialization
- Equipment saved via `InventoryRepository.Load()` / `Save()`
- Character data stored in `GameManager` (persists between scenes)

**Serializable Data Structures**:
- `EquipmentSaveData`: Equipment stats and modifiers
- `SerializableEquipmentData`: Wrapper for `EquipmentInstance[]`
- `PortraitItemData`: Character portrait and save data

## UI Architecture

**UI Namespaces**: Most UI scripts prefixed with `UI_`

**Key UI Systems**:
- `UI_Battle`: Combat UI coordinator (health bars, skill buttons)
- `UI_Inventory`: Equipment inventory management
- `UI_ChestInventory`: In-combat equipment view
- `FloatingTextDisplay`: Damage/healing floating text (pooling pattern)
- `UI_TouchBounce`, `UI_Show`, `UI_Selector`: Reusable UI animation utilities

**UI Update Pattern**:
- Managers trigger events (`OnDataChanged`, `OnMapNodeChanged`)
- UI subscribes and refreshes (`Initialize()`, `Refresh()`)

## Code Conventions

**Naming**:
- Private fields: `_camelCase` with underscore prefix
- Public properties: `PascalCase`
- Serialized fields: `[SerializeField] private` preferred over public
- Manager instances: `Instance` property for singleton access

**Access Patterns**:
- Managers accessed via `ManagerName.Instance`
- Character index tracked via `Index` property (0-2)
- Heavy use of DOTween sequences for animations

## Common Development Patterns

**Adding New Playable Character**:
1. Create prefab in `Assets/03.Prefabs/Characters/`
2. Assign to `GameManager.Characters[index]` in Village scene
3. Configure `PlayableCharacter` component with skills, animations
4. Create portrait sprite and mark as Addressable asset with unique key
5. Reference portrait via Addressable key (not direct ScriptableObject reference)
6. Create `PortraitItem` for character selection UI with async portrait loading

**Adding New Equipment**:
1. Create `EquipmentSO` in `Assets/04.ScriptableObjects/Equipment/`
2. Define `StatRange` for each stat modifier
3. Add to `EquipmentGenerator.equipmentSOs` array
4. Equipment automatically generates with randomized stats

**Adding New Enemy**:
1. Create class inheriting `EnemyCharacter`
2. Implement abstract methods (`Attack`, `Skill1-4`, `Death`)
3. Spawn via `MonsterSpawner` or direct instantiation
4. Register with `CombatManager.SpawnEnemy()`

**Adding New Skill**:
1. Create `PlayableSkillSO` in ScriptableObjects
2. Set `SkillType`, `SkillTarget`, `SkillRange`
3. Assign to character's `Skills` list
4. Create corresponding animator triggers (Attack, Skill1, Skill2)

## Important Notes

- **Character Index Consistency**: Character index (0-2) must match across `GameManager.Characters`, `TeamSlots`, `CharacterStats`, `IsAlive` arrays
- **Turn Order Integrity**: Always use `SetOrder()` after modifying `TurnOrder` list
- **Equipment Loading**: Equipment IDs hardcoded in `CombatManager.SpawnPlayer()` (line 88-89) - should be data-driven
- **Animation Coupling**: Skill execution tightly coupled to animation state machine completion
- **Singleton Lifecycle**: All managers use `DontDestroyOnLoad` - be careful with scene reloads
- **Addressables Asset Loading**: Portrait images and other character assets use Addressables - always use async loading patterns and handle load failures gracefully
- **Asset Reference Migration**: Legacy ScriptableObject direct references were replaced with Addressable keys - avoid mixing both approaches

## Third-Party Assets

The project includes several third-party asset packages:
- **Feel (More Mountains)**: Feedback system for game feel
- **Hovl Studio**: Particle effects
- **DOTween**: Animation tweening (via scripting defines)
- **Easy Performant Outline**: Outline effects for target selection

When modifying these directories, preserve existing material and prefab configurations.
