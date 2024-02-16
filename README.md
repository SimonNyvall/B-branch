# B-branch [![.NET](https://github.com/SimonNyvall/B-branch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/SimonNyvall/B-branch/actions/workflows/dotnet.yml)

[B-branch] is a .NET-based application designed to enhance the way developers interact with Git repositories directly from their terminal. Inspired by innovative ideas shared in a video by GitButler on YouTube, this tool aims to streamline the workflow for managing and visualizing Git branches, making it easier to see branch details at a glance.
Features

Branch Table Visualization: Displays an organized table of branches in your Git repository, including ahead/behind information relative to the main branch and the date of the last commit.
Easy Integration: Seamlessly integrates with your existing .NET projects, enhancing your development workflow with minimal setup.
Cross-Platform Support: Built on .NET, [B-branch] runs on any platform supported by .NET 8.0, including Windows, Linux, and macOS.

The output of the B-branch will look like this

| Ahead | Behind | Branch Name         | Last Commit |
| ----- | ------ | ------------------- | ----------- |
| 0     | 1      | feature/new-feature | 2024-02-13  |
| 2     | 0      | bugfix/issue-fix    | 2024-02-12  |
| 1     | 1      | development         | 2024-02-11  |

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
Prerequisites

    .NET 8.0 SDK installed on your machine.

## Installation

### Option 1

    1. Download the compiled src file
    2. Run the command below

```sh
git config --global alias.bb '!sh -c "/path/to/bin/src"'
```

This will set a alias in the `.gitconfig` file to point to that executable.
Run the script by running `git bb`

### Option 2

    Clone the repository:

```sh
git clone https://github.com/[SimonNyvall]/[B-branch].git
```

Navigate to the project directory:

```sh
cd [B-branch]/src
```

Restore the project dependencies:

```sh
dotnet restore
```

Build the project:

```sh
    dotnet build
```

Make an alias in the bashrc to point at the src executable:

```sh
.bachrc << "alias 'git bb'=/home/.../path/to/bin/src"
```

The src file will be under the `bin/debug/net8.0` in the project.

Additional thing is to run

## Usage

To run [B-branch], run `git bb`

This command will start the application, scanning the local Git repository for branch information and displaying it in a structured table format.
Contributing

We welcome contributions to [B-branch]! If you have suggestions or improvements, please fork the repo and create a pull request, or open an issue with the tag "enhancement". Don't forget to give the project a star! Thanks again!
Acknowledgments

    This project was inspired by the innovative ideas shared by GitButler. Check out their video for more insights into enhancing Git workflows.

License

This project is licensed under the MIT License - see the LICENSE.md file for details.
