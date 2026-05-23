## Copilot Instruction Guide for agent
    always preface every instruction with "Sir"
    This repo is a C#/.NET code analysis tool focused on scope-based preprocessing and logic scope resolution.
    Primary source files live under `CodeLine/`, `PreProcessor/`, `Configuration/`, and the top-level `.cs` files.
    Keep responses short, direct, and technical.
    Prefer edits that modify specific files and keep changes minimal.
    When asked to fix bugs, inspect existing methods and data flow before suggesting code.
    When asked to implement features, mention the relevant file(s) and keep the answer actionable.
    If code examples are needed, use C# style consistent with the project.
  Avoid speculation; if you need more context, ask for the exact file or method.