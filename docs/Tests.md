# Tests Documentation

## Overview
The `tests` directory contains two main testing projects: `Unit Tests` and `Integration Tests`. These tests are designed to ensure the functionality and reliability of the components within the application, including the CLI, GitService, and Shared modules.

## Project Structure
```plaintext
tests/
├── UnitTests/
│   ├── CLI/
│   ├── GitService/
│   └── Shared/
└── IntegrationTests/
    └── IntegrationTests/
```

### Unit Tests
- Purpose: Unit tests validate the smallest testable parts of the application, such as individual classes and methods. They ensure that each component behaves as expected in isolation.
- Framework: The unit tests are built using [xUnit](https://xunit.net/) and utilize [NSubstitute](https://nsubstitute.github.io/) for mocking dependencies.

**Example Unit Test Structure**
```csharp
[Fact]
public void ParseArguments_ShouldReturnContainsFlag_WithContainsArgument()
{
    string[] args = ["--contains", "test"];

    FlagCollection options = [];

    bool isSuccessful = Parse.TryParseOptions(args, out options);

    Assert.True(options.Contains<ContainsFlag>());
    Assert.True(isSuccessful);
}
```

### Integration Tests
- Purpose: Integration tests validate the interactions between different components of the application. They check how modules work together and ensure that data flows correctly across the system.
- Framework: Like the unit tests, integration tests are also built using xUnit.

## How to Run Tests
To run the tests, either run `dotnet test` from the root directory or use the test runner in your IDE.
> [!Important]
> The `dotnet test` command does only invoke the `Unit tests` and not the `Integration tests`.
> The integration tests do not run on a local machine, as they require an environment with the right setup to run.

To run the Integration test you can use the following command:
```bash
cd tests/IntegrationTests/
docker build -t integration-test-image .
docker run integration-tests-image
```

This will build a docker container and run it with the integration tests as well as the application.

It is also not possible to run the `test-runner-ps1`. This script will run both hte unit and integration tests. Just make sure to have docker engine running.