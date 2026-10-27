# Native Extended IV

**Native Extended IV** is a library for ScriptHookDotNet (SHDN) v1.7.1.8 that provides a wide range of native and non-native functions. Some functions are combined into higher-level helpers designed to make them easier and safer to use. The library also includes non-native classes and methods for advanced functionality, along with `TickHelper`, which processes internal and external tick-based systems created with `NativeTimer` instances.

**Current version:** 1.3.0

**Source code, releases, and license:** [GitHub repository](https://github.com/Lorenzo3024it/GTA-IV-Native-Extended)

This document describes the main classes included in the library, along with their nested classes, structs, and enums.

## NativeAudio

Handles game-audio-related native functions and structs, including mission-completion sounds. It can also trigger a one-shot game sound without creating a new `GameSound` instance.

- **`GameSound`** (nested class) — An instantiable class for playing game sounds from a position, an entity, or the frontend. It also supports non-native looping functionality.
- **`GameSounds`** (struct) — Contains a list of predefined in-game sounds. The list may be expanded in future versions.
- **`FireSound`** (nested class) — An instantiable class for playing fire sounds at specific world positions. The sounds cannot be moved after playback starts.

## NativeCamera

Provides camera-related native functions for position, rotation, field of view (FOV), cinematic control, and more.

Create a `NativeCamera` instance to use this functionality. It is separate from ScriptHookDotNet's `GTA.Camera` class.

**Enums**
- `ShakeBehaviour` and `ShakeType` — Used by `ShakeAdvanced`.
- `CameraState` — Non-native camera state.
- `CameraType` — List of known camera types used in SCO scripts.
- `VehicleCameras` — Vehicle camera modes used by `SetVehicleCamMode`.

## NativeCheckpoint (new)

An instantiable class for creating checkpoints with an optional arrow and blip. It can also trigger an action automatically when the player enters the checkpoint.

- `CheckpointType` — Determines whether to display the checkpoint, the arrow, or both.

## NativeControls

Handles keyboard, mouse, and Xbox controller input.

Input-checking functions must be called from the main script's `Tick` event. They can detect either a newly pressed key (`JustPressed = True`) or a key being held (`JustPressed = False`). `IsKeyPressed` can optionally check secondary and tertiary keys.

**Enums**
- `NativeKeys` — Game-specific control keys.
- `MouseInputs` — Mouse button inputs.
- `NavKeys` — Navigation keys.

- **`Joypad`** (nested class) — Provides native functions related to the joypad. It reads precise trigger positions through **SlimDX**; without SlimDX, it can detect only whether a trigger is pressed.
  - `Inputs` — Joypad buttons.

The SlimDX library is included in the archive, subject to its license. You do not need to add it as a project reference, but its DLL must be copied to GTA IV's main folder if you want to use the functionality that depends on it.

## NativeDebug

Contains various native debugging functions. Some may not work in-game.

- **`DebugText`** (nested class) — Displays useful on-screen information, such as player position, heading, and vehicle information. It also allows custom debug variables to be added through `DebugText.AddCustomInfo`.

## NativeDraws

Provides functions for drawing 2D elements on-screen, including text, rectangles, and game textures. It also supports custom help messages with custom colors and backgrounds.

Set `DrawDebugGrid = True` to display a 2D grid, with the center coordinates shown at the bottom of the screen.

`Print` is a useful method for coding and debugging. It displays up to three custom lines at the bottom of the screen without overriding other displays. It uses `TickHelper` to keep the text visible for five seconds (a fixed duration), and it can also be called from within a tick.

The native implementations of `DrawText`, `PrintBig`, `PrintHelp`, and `Print` can display approximately 80 characters per line. To work around this limitation, use the `TextColors.FX_LineBreak` tag to split text manually. The tag is handled internally to draw the text line by line, taking text wrapping and the line-height multiplier into account. This limitation also affects `NativeMenu` descriptions.

- **`ColorRGB` / `ColorRGBA`** (structs) — Represent color models used for drawing.
- **`CustomFade`** (nested class) — A non-native class for triggering screen fades with a custom color, alpha, duration, and other options.
- **`GameTexture`** (nested class) — An instantiable class for drawing game textures.
- **`GXT`** (nested class) — Draws predefined GXT strings and help messages.
- **`Movie`** (nested class) — Plays in-game cinematic videos. This feature has not been fully tested.
- **`TextColors`, `TextBlips`, `TextKeys`** (structs) — Provide color codes, radar blips, and button icons for use with `DisplayText` and `DrawText`. Thanks to [Inhibitor](https://www.se7ensins.com/forums/threads/grand-theft-auto-iv-text-colours-blips-icons.1880672/).
- **`TextStyle`** (nested class) — Provides native options for customizing text displayed through `DrawText`.

**Enums**
- `BigPrintType` — Used by `PrintBig` to match native big-print channels.
- `Justification` — Used by `TextStyle`.
- `QueueMode` — Used by `PrintBig` and `PrintHelp` (custom functionality).

- **`DrawManager`** (internal nested class) — Controls the display duration of many elements. Call `TickHelper.ProcessAll()` from the main script's `Tick` event.

## NativeExplosion

Provides three explosion-related functions and the `ExplosionType` enum.

## NativeFire

Provides native functions for creating and extinguishing scripted fires. Instantiate the class to manage fires more safely.

## NativeGarage

Provides native and non-native functions for controlling garage doors across the map, as used in missions. It includes a tick-based system linked to `TickHelper` and supports garage blips that can be toggled.

- **`PaynSpray`** (nested class) — Handles functions for Pay'n'Spray garages.

## NativeGeneric

A collection of native functions for features such as the pause menu, random integer/float/Boolean generation, and the in-game brief. It also provides functions for creating race checkpoints (separate from `NativeCheckpoint`) and simple 3D-world elements such as checkpoints, arrows, lights, and coronas.

- **`Bullet`** (nested class) — Contains native functions for firing bullets and finding them in the 3D world.
- **`CarRecording`** (nested class) — Loads and plays car recordings, such as default vehicle paths used in some missions when an enemy tries to escape. **Untested.**
- **`Cheats`** (nested class) — Provides functions for activating in-game cheats through the `CheatIndex` enum and resetting the `TimeCheated` stat.
- **`Replay`** (nested class) — Contains native functions related to the in-game replay system.
- `RaceCheckpointType` (enum) — Used by `CreateRaceCheckpoint`.

## NativeHelicopter (new)

Inherits from `NativeVehicle` and provides native functions specific to helicopters.

- `HeliParts` (enum) — Helicopter components.

## NativeHud

Controls the visibility and flashing of HUD elements. It includes flash functions and an internal tick system linked to `TickHelper`.

## NativeMenu

One of the library's most complex classes. It provides in-game menus and submenus with different item and value types. The menu's appearance can be customized in the main script, and each item can have a custom selected color and a description box.

The class detects value changes, saves values to the script's settings file, and invokes an action in the main script to update values. The setting's name matches the name of the associated menu item.

Call `BindValueChanged` once for the main menu; submenus are linked automatically. Pass the action delegate when calling it from the constructor (for example, `AddressOf Test_ValueChanged`). The callback receives a `NativeMenu` instance, the item name (`String`), and the value (`Object`). In this callback, reload the settings file and any variables linked to it. Variables that are not saved in the settings file can also be updated in the same method by converting the value to the appropriate type.

The menu also provides an in-game key-rebinding function. Main methods include:

- `AddItem` — Adds a section heading (a non-selectable item) or a button linked to a submenu or action.
- `AddItems_FromFileSection` — Adds items with values from a specified settings-file section.
- `AddItem_WithTextList` — Adds an item containing a list of strings.
- `AddItem_InGame` — Adds an in-game button that lets users add new items to the menu. When selected, the user can enter the new item's name; its value type (for example, `Boolean` or `Single`) and default value are defined by the script. The new item is added to the menu, and the button becomes available again so more items can be added. Items and their values can also be added automatically to the settings file.
- `AddItem_WithValue` — Adds an item with a value of a specified type (`Integer`, `Single`, `Boolean`, or key).
- `AddSeparator` — Adds a non-selectable separator line.
- `SubMenu` — Retrieves a submenu by name, creating it automatically if necessary.

- **`MenuAppearance`** (nested class) — Contains the visual parameters exposed by the `Appearance` property and methods for customizing individual elements.
- **`MenuKeys`** (struct) — Contains the keys used by the menu.
- `ValueType` (enum) — Lists the supported value types.

The description text, drawn using `DISPLAY_TEXT_WITH_LITERAL_STRING`, is limited to approximately 80 characters per line by the native implementation. Use `TextColors.FX_LineBreak` to split lines manually and work around this limitation.

## NativeModel

Contains model-related native functions, such as `Request`, `HasModelLoaded`, and `GetHashKey`.

## NativeObject

Contains numerous functions related to physical objects in the game world.

## NativePed

One of the largest and most complex classes, containing an extensive collection of ped-related native functions.

**Enums**
- `Gender`, `MovementType` — Used by various functions.
- `RomansMood` — Can be set through `SetRomansMood(Mood)`.
- `Components`, `Props` — Used by clothing functions.

- **`Animation`** (nested class) — Manages ped animations and animation groups. It can be instantiated or its static methods can be used.
  - **`AnimGroups`** (struct) — Lists the animation groups available in GTA IV.
  - **`Gesture`** (nested class) — Provides gesture-related functions, including nine structs covering different gesture types.
  - **`Facial`** (nested class) — Provides facial-expression functions, including three structs covering available facial sets.
- **`Clothes`** (nested class) — Allows changing a ped's clothes and props.
- **`Speech`** (nested class) — Plays ambient speech and screams.
  - `AudioEvents` — Used by `SayAudioEvent`.
  - `Screams` — Used by `SayScream`.
- **`Route`** (nested class) — Lets you define a route using multiple coordinates and assign a ped to drive or follow it on foot.
- **`Task`** (nested class) — Contains most of the task functions for AI behavior.
  - **Enums:** `DrivingAction`, `DrivingBehaviour`, `DrivingFlags1`, `DrivingFlags2`, `SittingType`. `SittingType` combines sitting animations in a way intended to avoid crashes.
  - **`SittingIdleAnims`** (struct) — Contains sitting idle animations used in missions.

## NativePlayer

Similar to ScriptHookDotNet's `Player` class, with additional properties and methods. It provides access to `NativePlayer.Character`, which is compatible with the original SHDN ped functions. This is useful in static methods where `Player.Character` is not accessible.

- `MovementType` (enum) — Used by `GetMovementType`.
- **`NativePhone`** (nested class) — Provides native phone-related functions, including the `Enabled` and `IsOnScreen` properties and the `ForceClose` method, which interacts with native SCO phone scripts.
  - **`Camera`** (nested class) — Provides phone-camera functions. **Not fully tested.**
  - **Enums:** `PhoneHudItem`, `PhoneState`, `Ringtones`.
- `PlayerMood` (enum) — Used by `SetMood` to select facial expressions. Many expressions trigger a looping facial animation; the enum also includes normal and angry/pissed-off moods.
- **`Stats`** (nested class) — Provides functions to get, set, increment, and decrement player stats, and to show or hide them. Stats are divided into integer and floating-point values.
  - **Enums:** `StatFloats`, `StatINTs`.

## NativePolice

Provides functions and properties related to the in-game police system. It also uses `TickHelper` to process `ControlWantedLevel`. It supports animated wanted-level changes and clearing, a fake wanted level, disabling police dispatch, and the `NeverWanted` property.

It also provides `TriggerCrimeReport`, which plays a police-dispatch sound at a specified position for a given crime type, and `TriggerScriptedReport`, which plays a scripted dispatch sound used in missions.

- `Crime` (enum) — Used by `TriggerCrimeReport`.
- `ScriptedReport` (enum) — Contains scripted crime audio events for `TriggerScriptedReport`.
- `Wanted` (enum) — Used by the `WantedState` property to retrieve the wanted state based on the native wanted-star level, and by `TickHelper` methods.

## NativePTFX

Manages particle effects (PTFX). Static methods can trigger a temporary one-shot PTFX.

The class can also be instantiated to manage native PTFX handles and help prevent game issues.

- **`ParticleFXs`** (struct) — Lists triggerable particle effects in GTA IV. Some EFLC effects may be missing.
- **`Effects`** (struct) — Lists effects. GTA IV SCO scripts use the `Fade` effect; the other listed effects are from TBoGT.

## NativeRadio

Contains functions related to vehicle radio and mobile radio, which can be used on foot or inside a vehicle with the engine turned off.

- `RadioStations` (enum) — Lists radio stations available in GTA IV. EFLC stations may be added in a future version.

## NativeScript

Provides native functions for SCO scripts. **Use with caution:** starting or stopping some scripts may cause issues or crash the game.

- **`IVScripts`** (struct) — Lists native scripts for GTA IV. EFLC scripts may be added in a future version.

## NativeTimer

Handles the three main in-game timers (`TimerA`, `TimerB`, and `TimerC`) and the game timer (`TotalGameTime`). It can also create independent custom timers with millisecond precision, based on game time and the time scale, using the `GET_GAME_TIMER` native. Each timer can trigger multiple actions and can optionally have a maximum tick limit.

`TickHelper` must be processed from the main script's `Tick` event for timer updates to work. A 1 ms interval is suggested by the library documentation.

- `TimerA` / `TimerB` — Used by missions and ambient scripts. Use with caution.
- `TimerC` — Used mostly in multiplayer scripts. It should be safer to use, but creating a `NativeTimer` instance is recommended.
- `TotalGameTime` (read-only) — Exposes the game timer. Confirm its exact timing semantics against the implementation before describing it as total playtime for the current save.

## NativeTrain

Contains train-related native functions, some of which have not been tested. It can be instantiated to manage per-train properties more easily.

## NativeVehicle

A large collection of vehicle-related functions, enums, and helpers.

It includes a method that permanently disables the engine by setting `PetrolTankHealth < 0`, preventing the player from restarting it.

**Warning:** Do not use this method on mission vehicles, as it may cause mission failures.

- **`Door`** (nested class) — Controls door states and uses `TickHelper` to avoid the hood or trunk closing instantly.
- **`IDE`** (nested class) — A non-native class that automatically retrieves vehicle parameters from `vehicles.ide` and other vehicle-related files.
- **`StuckCheck`** (nested class) — Provides vanilla stuck checks for AI peds' vehicles. **Untested.**
- **`Window`** (nested class) — Provides window-related functionality.
- **Enums:** `EngineState`, `LightsState`, `HazardLightsState`, `GpsVoice`.

## NativeViewport (new)

Provides functions for managing native viewports. **Use with caution.**

## NativeWorld

A large class covering various aspects of the game world, including stunt jumps, world audio, and distances.

- **`Area`** (nested class) — Provides functions for clearing areas and adding or removing spawn blocks, cover blocks, and more.
  - **`CoverPoint`** (nested class) — An instantiable class for managing cover points for peds. **Not fully tested.**
- **`Calendar`** (nested class) — Provides functions related to in-game time, day, and month. It uses `TickHelper` for `AdvanceTime` (animated) and `SetMinuteDuration`.
  - **Enums:** `DaysOfWeek`, `Month`.
- **`Interior`** (nested class) — Retrieves rooms from entities and gets or sets door states around the map. It also provides `TerminateDoorLockScripts` to prevent automatic door-state resets.
  - **`Doors`** (struct) — Lists various door hashes.
- **`Navigation`** (nested class) — Provides native functions for GPS and waypoints. `GetTravelDistance` is native but may not always be accurate.
- **`Population`** (nested class) — Provides properties and methods, combining native functions to control ambient vehicles, ped and traffic density, and emergency vehicles.
- **`Respawn`** (nested class) — Combines native functions with `TickHelper` to add hospitals, police stations, and custom respawn points in single-player.
- **`Road`** (nested class) — Provides methods for retrieving street and cross-street names, path nodes, and pavements.
- **`Zone`** (nested class) — Provides functions for retrieving zone names, setting population types, and more.
  - **`ZoneHash`** (struct) — Lists zone hashes.
  - `DangerLevel` (enum) — Used by `SetDangerLevel` with native values.
- **`Weather`** (nested class) — Provides native functions related to weather and timecycle modifiers.
  - **`TimeCycleModifiers`** (struct) — Lists timecycle modifiers for GTA IV. Some EFLC modifiers may be missing.
- `Islands` (enum) — Native island IDs (map areas).
- `WeatherType` (enum) — Lists weather types.

## TickHelper

A non-native helper class that manages and coordinates the library's internal tick-based updates.

- `ProcessAll` — Call this once from the main script's `Tick` event to update all registered tick-based systems, such as `NativeGarage` and `NativeTimer`. A 1 ms interval is suggested by the library documentation.

## Tools

Contains native and non-native utility functions used internally, some of which may also be useful in scripts.

- `ClampInteger` / `ClampFloat` — Restrict a value to a minimum and maximum.
- `GetCoefFps` — Calculates the FPS coefficient based on a default FPS value.
- `Lerp` / `LerpAngle` / `ToRadians` / `ToDegrees` — Utility math functions.
- `StringToInteger` / `IntegerToString` — Native conversion functions that are rarely needed but may occasionally be useful.
- `FPS` / `FrameTime` / `FrameCount` / `GamePath` / `ScreenAspectRatio` / `ScreenResolution` — Useful properties.
- `ShowFps` (property) — Displays the FPS counter in the top-left corner.
- **`LogFile`** (nested class) — Creates custom log files and provides `WriteLog` and `SetHeaderMessage`.
- **`SettingsFile`** (nested class) — Makes it easier to create custom settings files that can be linked to `NativeMenu`.

## Installation notes

Add `NativeExtended-IV.dll` to your project's references, then place the DLL in GTA IV's main folder. Copy **SlimDX** to the same folder only if you need the functionality that depends on it, and make sure to comply with its license.

## Future plans and contributions

Future versions may include testing of currently untested functions and additional features, such as cutscenes, task sequences, and more.

Contributions are welcome and appreciated.

