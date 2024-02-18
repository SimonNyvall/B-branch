# B-branch [![.NET](https://github.com/SimonNyvall/B-branch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/SimonNyvall/B-branch/actions/workflows/dotnet.yml)

- [B-branch](#b-branch)
  - [Features](#features)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
      - [Option 1 (Download the compiled src file)](#option-1-download-the-compiled-src-file)
      - [Option 2 (Build from source)](#option-2-build-from-source)
  - [Usage](#usage)
  - [Contributing](#contributing)
  - [Acknowledgments](#acknowledgments)
  - [License](#license)

## Features

**B-branch** is a .NET-based application designed to enhance the way developers interact with Git repositories directly from their terminal. Inspired by innovative ideas shared in a [video by **GitButler** on YouTube](https://www.youtube.com/watch?v=aolI_Rz0ZqY&t=472s), this tool aims to streamline the workflow for managing and visualizing Git branches, making it easier to see branch details at a glance.
Features

Branch Table Visualization: Displays an organized table of branches in your Git repository, including ahead/behind information relative to the main branch and the date of the last commit.
Instead of using the `git branch` command, which only lists branch names in a flat list organized by alphabetical order, **B-branch** provides a more structured view of branch information, making it easier to see the status of each branch in relation to the main branch.

The ability to see the branch description is also a feature that is not available in the standard `git branch` command. To add a description to a branch, use the following command:

    git branch --edit-description

Keep in mind that the description is stored in the `.git/EDIT_DESCRIPTION` and does not support a description on multiple branches.

<img src="./images/screen.jpg" alt="screen" width="500"/>

---

Cross-Platform Support: Built on .NET, **B-branch** runs on any platform supported by .NET 8.0, including Windows, Linux, and macOS.

The output of the B-branch will look like this

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
Prerequisites

    git

    .NET 8.0 SDK installed on your machine.

    OBS! .NET 8.0 SDK only applies if you build from source.

## Installation

### Option 1 (Download the compiled src file)

1. Download the compiled src file
2. Run the command below

```sh
git config --global alias.bb '!sh -c "/path/to/bin/src"'
```

This will set a alias in the `.gitconfig` file to point to that executable.
Run the script by running `git bb`

### Option 2 (Build from source)

Clone the repository:

```sh
git clone https://github.com/SimonNyvall/B-branch.git
```

Navigate to the project directory:

```sh
cd B-branch/src
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

To run **B-branch**, run `git bb`

This command will start the application, scanning the local Git repository for branch information and displaying it in a structured table format.
Contributing

We welcome contributions to **B-branch**! If you have suggestions or improvements, please fork the repo and create a pull request, or open an issue with the tag "enhancement". Don't forget to give the project a star! Thanks again!

## Acknowledgments

This project was inspired by the innovative ideas shared by **GitButler**. Check out their video for more insights into enhancing Git workflows.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.
