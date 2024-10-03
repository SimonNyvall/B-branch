# Project Architecture

## Overview

This document provides an overview of the project's architecture, including the key components, their interactions, and the overall design philosophy.
The project is divided into three main components, [CLI](./CLI.md), [GitService](./GitService.md) and [Shared](./Shared.md).

## Project Structure
```plaintext
├── src/
│   ├── CLI/
│   ├── GitService/
│   └── Shared/
└── tests/
    ├── UnitTests/
    └── IntegrationTests/
```

## [CLI](./CLI.md)
* Purpose: Handles the command line interface, user input and branch assembly. Also the main entry point for the application.
* Dependencies: [GitService](./GitService.md), [Shared](./Shared.md)
* Technologies: `.NET Console Application`

## [GitService](./GitService.md)
* Purpose: Fetches and parses git information into the Shared component.
* Dependencies: [Shared](./Shared.md)
* Technologies: `.NET Class Library`

## [Shared](./Shared.md)
* Purpose: Contains shared classes used by both the CLI and GitService.
* Dependencies: None
* Technologies: `.NET Class Library`

## [Tests](./Tests.md)
* Purpose: Contains unit and integration tests for the CLI and GitService components.
* Dependencies: [CLI](./CLI.md), [GitService](./GitService.md), [Shared](./Shared.md)
* Technologies: `.NET Core xUnit`, `NSubstitute`

## Interactions Between Components
- The CLI invokes commands that are processed by the GitService.
- The GitService fetches Git data and uses utilities from the Shared module to format or store the results.
- Both CLI and GitService rely on the Shared project for reusable components like logging, configuration, and error handling.

```plaintext
+--------+
| Shared | <----------------+
+--------+                  |
    A                       |
    |                       |
    |                       |
+--------+           +------------+
|  CLI   |  -------> | GitService |
+--------+           +------------+
```