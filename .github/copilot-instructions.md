# .github/copilot-instructions.md

## Mod Overview and Purpose
**Mod Name:** I Have Materials (Continued)

The "I Have Materials (Continued)" mod is an update of Ben Lubars' original mod for RimWorld. This mod enhances the gameplay experience by allowing players to place construction blueprints without having the required materials available on hand. By default, it forces common resources like wood, steel, cloth, and naturally occurring stones on the map to be counted as available, providing greater flexibility and ease of construction.

## Key Features and Systems
- **Always Available Blueprints:** Blueprints can be placed regardless of actual material stock.
- **Configurable Materials:** Default materials include wood, steel, cloth, and natural stones, but these can be customized through settings.
- **Seamless Integration:** The mod does not alter save files, allowing safe addition or removal at any point in a game.
- **Compatibility Precautions:** Conflicts may arise with any mods that replace the `Designator_Build.ProcessInput` function.

## Coding Patterns and Conventions
- **Class and Method Definitions:**
  - The mod includes a few C# files such as `IHaveMaterialsMod.cs`, which defines the main mod class and settings with methods to initialize and manage settings.
  - The `Designator_Build_ProcessInput` file involves handling the input process for build designators, which is central to the functionality of the mod.
- **Class Naming Convention:** Classes use PascalCase, such as `IHaveMaterialsMod`.
- **Method Naming Convention:** Methods also follow PascalCase, for example, `DefsLoaded` and `RebuildStuffIfNeeded`.

## XML Integration
- XML integration is not highlighted within this mod but can be used for defining default values or settings if required. The mod primarily relies on C# for processing logic and interaction with RimWorld systems.

## Harmony Patching
- **Purpose:** Harmony patches are utilized to modify, override, or extend the base game functionalities without directly altering the game's code.
- **Patch Target:** The primary function targeted is `Designator_Build.ProcessInput`.
- **Implementation:** Custom static methods are used to apply patches, ensuring the mod interacts correctly with RimWorld's systems.

## Suggestions for Copilot
- **Generate Harmony Patches:** Have Copilot suggest Harmony patches when the base method of a RimWorld class needs modification. Ensure that these patches are done non-destructively.
- **Assist with Configuration UI:** Utilize Copilot to help create user interfaces for configuring mod settings, especially in the `DoWindowContents` method.
- **Debugging Helpers:** Incorporate logging suggestions to assist in debugging, useful in the `RebuildStuffIfNeeded` method for tracing material configurations.
- **Code Consistency:** Use Copilot to ensure coding style consistency across different parts of the mod, maintaining clean and understandable code.

### Additional Tips
- Always ensure that your modifications can be gracefully integrated with other mods by maintaining minimum dependencies and side effects.
- Consider using inline comments to explain complex logic, enhancing code readability for future contributors.
- When debugging, utilize RimWorld's existing tools and log features to provide comprehensive error reports.

This guide is intended to assist future development and maintenance of the mod using GitHub Copilot, ensuring consistency and quality in code practices.
