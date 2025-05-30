# Copilot Instructions for B-branch

You are an AI assistant helping with the B-branch project, a Git branch management tool. The project is divided into three main components: CLI, GitService, and Shared. When suggesting code changes or additions, keep these key architectural principles and component relationships in mind.

## Component Structure

1. **CLI Component**
   - Main entry point for the application
   - Handles command line interface and user input
   - Responsible for branch assembly
   - Depends on GitService and Shared components

2. **GitService Component**
   - Core functionality for Git operations
   - Fetches and parses git information
   - Provides data to CLI component
   - Depends only on Shared component

3. **Shared Component**
   - Contains common classes and utilities
   - Used by both CLI and GitService
   - No external dependencies
   - Houses core data structures like `GitBranch`

## Core Architecture

1. **Composite Strategy Pattern**
   - The project uses a composite strategy pattern for handling Git branch operations
   - All strategies implement the `IOption` interface with the `Execute` method
   - `CompositeOptionStrategy` aggregates multiple strategies and executes them in sequence
   - Each strategy modifies or filters the `HashSet<GitBranch>` based on specific criteria

2. **Flag-Based Strategy Selection**
   - Branch processing strategies are selected based on command-line flags
   - The `BranchTableAssembler` class is responsible for assembling the appropriate strategies
   - Each flag (e.g., `--sort`, `--track`, `--contains`) corresponds to specific strategy implementations

3. **Git Repository Access**
   - Git operations are abstracted through the `IGitRepository` interface
   - The `GitRepository` class is implemented as a singleton using `GetInstance()`
   - Repository operations handle both local and remote branch information

## Code Organization

When suggesting new features or modifications:

1. **Strategy Implementation**
   - Place new strategy classes in appropriate subdirectories under `GitService/OptionStrategies/`
   - Ensure new strategies implement the `IOption` interface
   - Follow the pattern of modifying the branch collection through the `Execute` method

2. **Flag System**
   - Add new flags to the `FlagSystem` namespace
   - Integrate flag handling in `BranchTableAssembler`
   - Maintain the pattern of strategy selection based on flag values

3. **Git Operations**
   - Add new Git operations to the `IGitRepository` interface
   - Implement operations in `GitRepository` class
   - Use async/await for I/O operations where appropriate

## Best Practices

1. Maintain immutability by returning new `HashSet<GitBranch>` instances in strategy `Execute` methods
2. Follow existing error handling patterns using nullable types and try-catch blocks
3. Keep Git operations isolated in the repository layer
4. Use async/await for file and process operations
5. Follow the existing pattern of strategy composition for new features

## Common Patterns

When implementing new features, follow these patterns:

```csharp
// New strategy implementation
public class NewFeatureStrategy : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return [.. branches.Where(/* feature criteria */)];
    }
}

// Strategy assembly in BranchTableAssembler
private static void AddNewFeatureOption(FlagCollection arguments, CompositeOptionStrategy optionStrategies)
{
    if (arguments.Contains<NewFeatureFlag>(out var flag))
    {
        IOption newFeatureOption = new NewFeatureStrategy(/* dependencies */);
        optionStrategies.AddStrategyOption(newFeatureOption);
    }
}
```

## Testing Standards

1. **Test Naming Convention**
   - Format: `Given_[Context]_When_[Action]_Then_[ExpectedResult]`
   - Example: `Given_SortByAheadOptions_When_ExecuteRun_Then_Return_SortedBranches`
   - Use underscores to separate parts of the test name
   - Be specific about the context and expected outcome

2. **Test Structure**
   ```csharp
   [Fact]
   public void Given_Context_When_Action_Then_Result()
   {
       // Arrange - Set up test data and context
       var testData = new HashSet<GitBranch> { /* test data */ };
       var sut = new TestedClass();

       // Act - Execute the method being tested
       var result = sut.Execute(testData);

       // Assert - Verify the results
       Assert.Equal(expected, result.First().Property);
   }
   ```

3. **Test Organization**
   - Group tests by feature in appropriate directories
   - Unit tests mirror the main project structure:
     - `CLI/` - Command line interface tests
     - `GitService/` - Git operation tests
     - `Shared/` - Common utility tests
   - Keep test classes focused and cohesive
   - Include both happy path and edge cases

4. **Test Coverage**
   - Every strategy implementation must have corresponding tests
   - Test both success and failure scenarios
   - Include tests for already-sorted data when testing sort operations
   - Verify immutability of input collections
   - Test async operations with proper async/await patterns

5. **Testing Tools and Patterns**
   - Use xUnit for unit testing framework
   - Implement custom mock objects for testing instead of using mocking frameworks
   - Create focused, minimal mock implementations that match the interface being tested
   - Utilize test data builders and helper methods for common setup
   - Follow AAA (Arrange-Act-Assert) pattern in test methods
   - Keep mocks simple and maintainable with clear verification of expected calls

   Example of custom mock implementation:
   ```csharp
   // Focused mock implementation for testing
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
       
       // Implement only the methods needed for specific tests
   }
   ```

## Implementation Details

1. **Git Data Access**
   - Direct `.git` directory access for efficiency
   - Reading repository metadata (branches, commits, etc.)
   - Executing Git commands when needed for real-time data
   - Caching mechanisms for performance optimization

2. **Branch Data Processing**
   - Branch data is structured using the `GitBranch` class
   - Operations are performed on `HashSet<GitBranch>` collections
   - Each strategy transforms the branch collection immutably
   - Chain of responsibility pattern for sequential operations

3. **Error Handling and Performance**
   - Use nullable types for optional data
   - Implement caching for frequently accessed data
   - Proper exception handling for Git operations
   - Async operations for I/O-bound tasks

Remember to maintain the separation of concerns and follow the established patterns when implementing new features or modifying existing ones.
