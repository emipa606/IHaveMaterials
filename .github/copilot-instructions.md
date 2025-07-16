# GitHub Copilot Instruction File for RimWorld Modding Project

## Mod Overview and Purpose
This RimWorld mod, "IHaveMaterials", enhances the building experience by providing better management of available construction materials. It aims to improve the quality of life for players by making construction activities more intuitive and efficient.

## Key Features and Systems
- **Enhanced Material Management:** The mod introduces a system for tracking and utilizing building materials more effectively.
- **Improved Build Process:** Modifications to the construction designation improve user interactions and reduce resource wastage.
  
## Coding Patterns and Conventions
- **Coding Style:** The project follows C# naming conventions with PascalCase for classes and methods. Comments and documentation within the code are minimal, focusing instead on self-descriptive code.
- **Class Structure:** Classes such as `IHaveMaterials` extend from `ModBase`, encapsulating mod-specific logic.
- **Internal Static Classes:** Example includes `Designator_Build_ProcessInput_Patch`, indicating it might be used for harmony patching without object instantiation.

## XML Integration
- **XML Defs:** While not detailed in the summary, typical XML integration would involve custom `<Defs>` for linking new mod features with the game's existing systems. Ensure XML tags are properly closed and formatted according to RimWorld standards.
- **XML Strings:** If applicable, provide localized strings in XML to support multiple languages. Use standard tags like `<li>` for item lists.

## Harmony Patching
- **Static Harmony Patches:** Use static classes such as `Designator_Build_ProcessInput_Patch` to apply Harmony patches, modifying base game methods. This involves using Harmony annotations like `[HarmonyPatch(typeof(TargetClass), "TargetMethod")]`.
- **Method Patching:** Carefully choose between prefix/postfix/transpiler patches based on the required alteration to the game's methods.

## Suggestions for Copilot
- **Refactoring Help:** Use Copilot to suggest method extractions for large methods or repetitive code patterns to improve readability and maintainability.
- **Autocompletion:** Leverage Copilot for autocompleting standard patterns in Harmony patching and Def creation.
- **XML Suggestions:** Get help in completing XML tags and ensuring proper format, especially for mods with extensive XML configurations.
- **Debugging Assistance:** Copilot can be utilized for generating debug statements to trace mod behavior during runtime.
- **Localization Support:** Copilot can assist in generating string keys and entries for enabling localization of mod features.

This file should guide developers using GitHub Copilot to efficiently extend and maintain the "IHaveMaterials" mod for RimWorld, ensuring consistency and quality in their code contributions.
