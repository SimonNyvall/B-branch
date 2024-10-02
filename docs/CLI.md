# CLI Documentation

## Overview
The CLI project provides a command-line interface for interacting with the application, allowing users to perform various operations on Git repositories through commands. It serves as the main entry point to the system and leverages functionalities from the [GitService](./GitService.md) and [Shared](./Shared.md) components.

## Project Structure

The CLI is composed of several key components:
- **User Input Parsing**: Handles parsing and validation of user arguments.
- **Branch Assembly**: Manages the construction and sorting of branches.
- **Output Formatting**: Displays results in a formatted output, with support for pagination.

### User input parsing
User input is captured through the `args` array passed to the CLI. These inputs are parsed and converted into typed objects in the `./CLI/Arguments` directory.

- **Input Parsing**: The raw string input is parsed and validated. Classes in `Arguments` handle different user commands and options.
- **Validation**: The parsed arguments are validated to ensure they adhere to expected formats and constraints before further processing.

**Example:**
```csharp
/* Where options are the user flag as the key and the value as the value in the dictionary */
private static FlagCollection MapOptionsToFlags(Dictionary<string, string?> options)
{
    FlagCollection flags = [];

    foreach (KeyValuePair<string, string?> option in options)
    {
        IFlag flag = option.Key switch
        {
            "--all" or "-a" => IFlag<AllFlag>.Create(null),
            "--help" or "-h" => IFlag<HelpFlag>.Create(null),
            "--contains" or "-c" => IFlag<ContainsFlag>.Create(option.Value),
            "--no-contains" or "-n" => IFlag<NoContainsFlag>.Create(option.Value),
            "--print-top" or "-p" => IFlag<PrintTopFlag>.Create(option.Value),
            "--quite" or "-q" => IFlag<QuiteFlag>.Create(null),
            "--remote" or "-r" => IFlag<RemoteFlag>.Create(null),
            "--sort" or "-s" => IFlag<SortFlag>.Create(option.Value),
            "--track" or "-t" => IFlag<TrackFlag>.Create(null),
            "--version" or "-v" => IFlag<VersionFlag>.Create(null),
            _ => throw new ArgumentException($"Unknown option: {option.Key}")
        };

        flags.Add(flag);
    }

    return flags;
}
```

### Branch assembly
Once the user input is parsed, the next step is to gather and organize the list of branches. Initially, the CLI creates an empty list of branches containing default or placeholder data. This raw data is then processed and reformatted by the [GitService](./GitService.md), which fetches detailed information about each branch from the Git repository.

The CLI is responsible for orchestrating the sequence of operations required to prepare the branch list. For instance, the CLI may apply additional operations such as:

- Sorting the branches alphabetically or by specific criteria.
- Filtering branches based on user-specified arguments.
- Modifying the list based on flags like `--print-top`, which sorts and limits the displayed branches.

The CLI ensures that these tasks are performed in the correct order, ensuring a smooth and predictable workflow for handling branch data.

### Output formatting
After assembling and processing the branches, the final step is to present the results to the user in a structured format. This is handled in the `./CLI/Output ` directory.

- **Tabular Display**: The branches are displayed in a neatly formatted table, making it easy for users to view branch names, statuses, and other relevant information.

- **Pagination Support**: If the list of branches is too long to fit on the screen, the CLI provides a paging mechanism. The `Paging` class allows users to scroll through the branch list in chunks, ensuring smooth navigation through large sets of data.

- The `Paging` class uses a strategy pattern for output formatting, where different printing strategies can be injected to customize how the data is displayed. This flexibility allows for different output formats, such as:
  - **Simple Table**: A basic table view for quick overviews.
  - **Verbose Mode**: A more detailed format that includes additional metadata for each branch.

With these mechanisms, the CLI ensures that branch data is not only processed correctly but also presented in a way that is both user-friendly and customizable.