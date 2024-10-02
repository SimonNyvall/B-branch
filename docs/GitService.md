# GitService Documentation

## Overview
The GitService project is responsible for interfacing with the Git repository. It gathers information such as branches, commit history, and other metadata by directly accessing the `.git` directory or executing Git commands. This data is then formatted and provided to the [CLI](./CLI.md) and [Shared](Shared.md) components, enabling them to display branch information and perform Git operations.

## Project Structure
The GitService is composed of two main components:

- **GitBase**: Fetches raw Git data by reading from the `.git` directory or executing Git commands.
- **GitOptions**: Provides customizable options for fetching and formatting Git data, typically invoked by the [CLI](./CLI.md) to get specific branch details.

### GitBase
The GitBase class serves as the core utility for interacting with the local Git repository. It is responsible for:

- Reading the .git directory to gather metadata about branches, commits, and other repository details.
- Executing Git commands to fetch real-time data when needed, ensuring the information is always up-to-date.

The data retrieved from the Git repository is parsed and structured into a format that can be easily processed by other components, such as GitOptions.

Key Responsibilities:

- Fetching branch details (local and remote).
- Retrieving commit history.
- Reading repository metadata (e.g., latest commits, tracked branches).

Example:
```csharp
private static List<GitBranch> CollectBranchNames(string directoryPath)
{
    List<GitBranch> branches = [];
    var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

    foreach (string file in files)
    {
        string relativePath = Path.GetRelativePath(directoryPath, file);
        string branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');

        Branch branch = new() { Name = branchName, IsWorkingBranch = false };

        branches.Add(GitBranch.Default().SetBranch(branch));
    }

    return branches;
}
```

### GitOptions
The GitOptions class is invoked by the CLI to retrieve specific Git data based on user input. It constructs a chain of operations to fetch and format the data according to the user’s commands or flags, such as filtering branches or sorting them by specific criteria.

**Key Responsibilities**:

- Building a pipeline of Git operations based on user input.
- Formatting branch data for the CLI to display.
- Fetching additional details when needed, such as whether branches are ahead or behind their remote counterparts.


```csharp
public interface IOption
{
    List<GitBranch> Execute(List<GitBranch> branches);
}

public class CompositeOptionStrategy : IOption
{
    private readonly List<IOption> _options;

    public CompositeOptionStrategy(List<IOption> options)
    {
        _options = options;
    }

    public void AddStrategyOption(IOption option)
    {
        _options.Add(option);
    }

    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (IOption strategyOption in _options)
        {
            branches = strategyOption.Execute(branches);
        }

        return branches;
    }
}

public class SortByNameOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return [.. branches.OrderBy(branch => branch.Branch.Name)];
    }
}
```