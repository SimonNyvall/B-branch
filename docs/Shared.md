# Shared Documentation

## Overview
The Shared project contains common data models and utility functions that are used across both the CLI and GitService components. Its main responsibility is to represent and manage information related to Git branches, such as branch names, ahead/behind status, and last edit timestamps. It also ensures that this data is validated and properly structured before being used by other components.

## Project Structure
The Shared project consists of:

- GitBranch Class: Represents the data model for a Git branch, including metadata like the ahead/behind count and last modified time.
- Validation: Ensures the validity of branch-related data, such as verifying that branch names are not empty.
- Default Method: Provides a static method to create a new instance of GitBranch with default values for use in various scenarios, especially in testing or initial setups.

**Example:**

```csharp
public GitBranch SetBranch(Branch branch)
{
    if (string.IsNullOrEmpty(branch.Name))
    {
        throw new ArgumentException("Branch name should not be empty");
    }

    Branch = branch;

    return this;
}

public static GitBranch Default()
{
    return new GitBranch(
        new AheadBehind { Ahead = 0, Behind = 0 },
        new Branch { Name = "branchName", IsWorkingBranch = false },
        DateTime.MaxValue,
        string.Empty
    );
}
```