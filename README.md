# Native Extended IV - ScriptHookDotNet for IV Library

**Author:** Lorenzo3024IT  
**License:** Custom Open Source License (see LICENSE.txt)

---

## 🔹 NativeAudio
Handles game sound-related natives and structures.
- **GameSound (structure)** → Contains a list of predefined in-game sounds (will be expanded in future versions).

---

## 🔹 NativeCamera
Provides access to camera-related native functions (position, rotation, FOV, cinematic control, etc.).

---

## 🔹 NativeControls
Handles key and button inputs from keyboard, mouse, and Xbox controllers.  
It can check the instant key down (`JustPressed = True`) or holding down (`JustPressed = False`) in a Tick inside the main script.

**Enums:**
- NativeKeys → Game-specific control keys.
- KeyValues → Standard keyboard keys.
- MouseInputs → Mouse button inputs.
- NavKeys → Navigation keys.

**Joypad (subclass)** → Reads precise trigger pressure via SlimDX.dll; if SlimDX is not used, only detects trigger pressed state using `IsButtonPressed`.

---

## 🔹 NativeDebug
Contains various native debug-related functions (some may not work in-game).

**DebugText (subclass)** → Displays useful information on-screen (player position, heading, vehicle info, etc.) and allows adding custom debug variables through `DebugText.AddCustomInfo()`.

---

## 🔹 NativeDraws
Contains functions for drawing elements on the screen (texts, coronas, lights, etc.).

**Structures:**
- TextColors → Provide color codes.
- TextBlips → Radar blips.
- TextKeys → Button icons.

**Color Models:**
- ColorRGB / ColorRGBA → Represent color models for drawing.

**Subclasses:**
- **GXT** → Draws predefined GXT strings or in-game help messages.
- **HelpMessage** → Displays help messages.
- **Movies** → Plays in-game cinematic videos (not fully tested).
- **Texture** → Loads and draws predefined game textures.

---

## 🔹 NativeEntity
A non-native class used to instantiate any in-game entity (vehicles, peds, objects) as a generic Entity (similar to GTA V).  
It's used in some internal functions (like `NativeGeneric.DeletionList`) but can also be used in custom external scripts.

---

## 🔹 NativeFire
Native functions for creating and extinguishing scripted fires.

**FireSound (subclass)** → Instantiable class to play fire sounds at specific world positions (not movable once started).

---

## 🔹 NativeGarage
Native and non-native functions to control garage doors across the map (used in missions).  
Includes a tick-based system (requires enabling `NativeGarage.Tick.Enable()` and calling `TickHelper.ProcessAll()` in the main script tick).  
Allows full control of garages, including toggleable blips.

---

## 🔹 NativeGeneric
A set of mixed native functions (pause menu, random int/float generation, etc.).

**Subclasses:**
- **Cheats** → Activates in-game cheats using `CheatIndex` enum and can reset the “TimeCheated” stat.
- **DeletionList** → Manages entity cleanup lists using both native and non-native methods. Non-native ones are recommended, as they allow parallel, independent lists.

---

## 🔹 NativeHud
Controls the visibility and flashing of HUD elements.  
Contains Flash functions and an internal tick system linked to TickHelper.

**Enums:**
- PhoneHudItems → Represents the various elements of the in-game mobile phone HUD.

---

## 🔹 NativeModels
Contains all model-related natives (`Request`, `HasModelLoaded`, `GetHashKey`, etc.).

**SpatialHelper (subclass)** → Provides native and non-native functions to get model hitboxes and dimensions in world space.

---

## 🔹 NativeObjects
Includes numerous functions related to physical objects in the game world.

---

## 🔹 NativePaynSpray
Handles functions for Pay’n’Spray garages.

---

## 🔹 NativePeds
One of the largest and most complex classes, containing an extensive collection of ped-related native functions.

**Enums:**
- Gender
- MovementType → used in some functions.
- RomanMood → can be set using `SetRomandsMood(Mood)`.

**Subclasses:**
- **Animations** → Manages ped animations and animation groups.
  - **AnimGroups (structure)** → Lists all possible animation groups in GTA IV.
  - **Gestures** → Gesture-related functions (includes 9 structures for all gesture types).
  - **Facials** → Facial expression functions (3 structures for all available facial sets).
- **Clothes** → Allows changing ped clothes and props.  
  - **Enums:** Components, Props.
- **Speeches** → Plays ambient speeches or screams.
- **Tasks** → Contains dozens of task functions for AI behavior.  
  - **Enums:** CarAction, SittingType (combines various sitting animations safely, avoiding crashes).

---

## 🔹 NativePhone
Includes some phone-related native functions.

---

## 🔹 NativePlayer
Similar to ScriptHookDotNet’s Player class, but with additional properties and methods.  
Automatically retrieves the player index.  
Provides access to `NativePlayer.Character`, compatible with original SHDN ped functions.  
Useful in shared subs where `Player.Character` is not accessible.

**Stats (subclass)** → All functions to Get, Set, Increment, Decrement, Show or Hide player stats, divided into INTs and Floats.

**Enums:** StatFloats, StatINTs.

---

## 🔹 NativePolice
Functions and properties related to the in-game police and cops.

---

## 🔹 NativePTFX
Manages particle effects (PTFX).

**ParticleFXs (structure)** → Lists all triggerable particle effects.  
Can trigger a temporary one-shot PTFX or start persistent effects (with an integer handle for later stopping).  
⚠️ *Don’t start again a PTFX in the same place without stopping the last one, or the game won’t display PTFXs after a while.*

---

## 🔹 NativeRadio
Contains all functions related to vehicle radio and mobile radio usage (usable on foot).

---

## 🔹 NativeScripts
Native functions related to in-game scripts (not fully tested).  
Note: Many of these work only within their original native context.

---

## 🔹 NativeTimer
Handles the three main in-game timers (`TimerA`, `TimerB`, `TimerC`).

- **TimerA/B** → Used by missions and ambient scripts (use with caution).  
- **TimerC** → Used mostly in multiplayer scripts, safe for custom timers.  
Can instantiate custom independent timers with millisecond precision.  
Also supports automatic Tick execution when linked with `TickHelper.ProcessAll()`.

---

## 🔹 NativeTrains
All train-related native functions (some untested).  
Can be instantiated to manage per-train properties more easily.

---

## 🔹 NativeVehicles
Large collection of vehicle-related functions, enums, and helpers.  
Includes a method to permanently disable the engine (by setting `PetrolTankHealth < 0`), preventing the player from restarting it.  
⚠️ *Should not be used on mission vehicles, as it may cause mission failure.*

**CarRecordings (subclass)** → Used to load and play car recordings (the default vehicle paths used in missions, e.g., when an enemy tries to escape the player).

---

## 🔹 NativeWorld
Functions regarding weather, ambient traffic, trains, planes, and ped density.

**TimeCycleModifiers (structure)** → Contains all modifiers usable with `SetTimeCycleModifier()` and removable via `ClearTimeCycleModifier()`.

---

## 🔹 TickHelper
A non-native helper class that manages and optimizes internal Tick updates for the library.  
`ProcessAll()` → Must be called once in the main script tick to update all registered tick-based systems (e.g., `NativeGarage`, `NativeTimer`, etc.).

---

## 🔹 Tools
Some native and non-native functions used internally, but also useful in custom scripts.

- **ClampInt / ClampFloat** → Restrict a value between a minimum and maximum value.
- **StringToInteger / IntegerToString** → Native conversion functions (generally not required, but occasionally useful).
- **WriteLog** → Writes errors or logs inside `NativeExtended-IV.log`.  
  Can also be used in custom scripts to write any content to a custom file.
