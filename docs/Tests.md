# Tests Documentation

## Overview

The `Tests` directory contains a XUnit test project using TestContainers for the integration tests. These tests are designed to ensure the functionality and reliability of the components within the application, including the CLI, GitService, and Shared modules.

## Project Structure

```plaintext
Tests/
├── Unit/
└── Integration/
```

### Unit Tests

- Purpose: Unit tests validate the smallest testable parts of the application, such as individual classes and methods. They ensure that each component behaves as expected in isolation.
- Framework: The unit tests are built using [xUnit](https://xunit.net/) with custom, hand-crafted mock implementations for testing dependencies.
- Mocking Strategy: Instead of using a mocking framework, the project uses simple, focused mock implementations that directly implement the required interfaces. This approach provides better control and maintainability.

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

**Example Mock Implementation**

```csharp
// Simple mock implementation of IGitRepository for testing
public class MockGitRepository : IGitRepository
{
    private readonly string _workingBranch;
    private readonly HashSet<GitBranch> _localBranches;

    public MockGitRepository(string workingBranch, HashSet<GitBranch> localBranches)
    {
        _workingBranch = workingBranch;
        _localBranches = localBranches;
    }

    public string GetWorkingBranch() => _workingBranch;
    public HashSet<GitBranch> GetLocalBranchNames() => _localBranches;

    // Implement other interface methods as needed for specific tests...
}

[Fact]
public void Given_LocalBranchOptions_When_ExecuteRun_Then_ReturnLocalBranches()
{
    // Arrange
    var mockBranches = new HashSet<GitBranch>
    {
        GitBranch.Default().SetBranch(new Branch { Name = "main" }),
        GitBranch.Default().SetBranch(new Branch { Name = "feature" })
    };
    var mockRepo = new MockGitRepository("main", mockBranches);
    var localOption = new BranchLocalOptions(mockRepo);

    // Act
    var result = localOption.Execute(new HashSet<GitBranch>());

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Contains(result, b => b.Branch.Name == "main");
    Assert.Contains(result, b => b.Branch.Name == "feature");
}
```

### Integration Tests

- Purpose: Integration tests validate the interactions between different components of the application. They check how modules work together and ensure that data flows correctly across the system.
- Framework: Like the unit tests, integration tests are also built using xUnit.

## How to Run Tests

To run the tests, either run `dotnet test` from the root directory or use the test runner in your IDE.

> [!Important]
> Make sure to have docker engine running.

The application will pull a half completed image and attach the current changed code to test your changes.
