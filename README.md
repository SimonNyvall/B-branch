
<p align="center"><img src="./images/logo.png" alt="logo" width="250px"/></p>
<h1 align="center">B-branch</h1>
<h3 align="center">A git branch tool extension to git, helping developers manage git branches</h3>

<div align="center">
  <hr/>
 <img src="https://img.shields.io/github/actions/workflow/status/SimonNyvall/B-branch/dotnet.yml?style=flat&label=test%2Fbuild" alt=".NET">
 <img src ="https://img.shields.io/github/stars/SimonNyvall/B-branch?style=flat" alt="github stars"/>
</div>


- [B-branch](#b-branch)
  - [Primise](#premise)
    - [Example](#example)
  - [Usage](#usage)
  - [Getting Started](#getting-started)
    - [Installation](#installation)
      - [Option 1 (Download the compiled B-branch file)](#option-1-download-the-compiled-b-branch-file)
      - [Option 2 (Build from source)](#option-2-build-from-source)
  - [Usage](#usage)
  - [Contributing](#contributing)
  - [Acknowledgments](#acknowledgments)
  - [License](#license)

## Premise
B-branch is a .NET-based application that enhances Git repository management via  a git alias. Inspired by [**GitButlers**](https://www.youtube.com/watch?v=aolI_Rz0ZqY&t=472s) talk, B-branch provides a structured view of branch information, simplifying workflows for developers.

The triditional git branch sorts branches alphabetically, making it difficult to identify the most recent branches. B-branch addresses this issue by providing a structured view of branch information, including the date of the last commit and the number of commits ahead or behind the upstream branch.


### Example
Let's say you have a git repository with a lot of branches. You want to remove the branches that are no longer needed.

```sh
git bb --no-contains "main;development" -q \
| awk '{print substr($0, 3)}' \
| xargs -I {} git branch -D {}
```
`OBS!` This command will delete all branches that do not contain the strings "main" or "development". Be careful when using this command.

---

The ability to see the branch description is also a feature that is not available in the standard `git branch` command. To add a description to a branch, use the following command:

```sh
$ git bb --edit-description
```

Keep in mind that the description is stored in the `.git/EDIT_DESCRIPTION` and does not support a description on multiple branches.

<img src="./images/screen.jpg" alt="screen" width="500"/>

## Usage

```sh
$ git bb [options]
```

### Options

- `-t, --track <String>`: Displays information about how many commits the specified branch is ahead or behind relative to its upstream branch.

- `-q, --quiet`: Only displays the names of the branches without any additional information or 
formatting.

- `-v, --version`: Shows the current version of the `B-branch` tool.

- `-s, --sort <String>`: Sorts the branches based on the specified criterion. Valid options are `[date]`, `[name]`, `[ahead]`, or `[behind]`.

- `-a, --all`: Displays all branches, both local and remote.

- `-n, --no-contains <String>`, `"String1;String2;..."`: Filters the list to only show branches that 
do not contain the specified string. Valid options are `<String>` OR `"String1;String2;..."`.

- `-c, --contains <String>`, `"String1;String2;..."`: Filters the list to only show branches that contain the specified string. Valid options are `<String>` OR `"String1;String2;..."`.

- `-r, --remote`: Includes remote branches in the output.

- `-h, --help`: Displays the help message with information about all available options.

- `-p, --print-top <Number>`: Prints the top N branches based on the specified sort criterion.

---

Cross-Platform Support: Built on .NET, **B-branch** runs on any platform supported by .NET 8.0, including Windows, Linux, and macOS.

The output of the B-branch will look like this

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
Prerequisites

    git 2.39.2 or later

    nerd-fonts 2.1.0 or later

    .NET 8.0 SDK installed on your machine.

    OBS! .NET 8.0 SDK only applies if you build from source.

## Installation

### Option 1 (Download the compiled B-branch file)

1. Download the compiled src file
2. Run the command below

```sh
git config --global alias.bb '!bash -c '\"/home/nyvall/Code/B-branch/src/bin/Debug/net8.0/B-branch\" \"$@\"' bash''
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
.bachrc << "alias 'git bb'=/home/.../path/to/bin/B-branch"
```

The B-branch file will be under the `bin/debug/net8.0` in the project.

Additional thing is to run

## Contributing

We welcome contributions to **B-branch**! If you have suggestions or improvements, please fork the repo and create a pull request, or open an issue with the tag "enhancement". Don't forget to give the project a star! Thanks again!

## Acknowledgments

This project was inspired by the innovative ideas shared by **GitButler**. Check out their video for more insights into enhancing Git workflows.

## License

This project is licensed under the <a src="./LICENSE">MIT License</a> - see the LICENSE.md file for details.
