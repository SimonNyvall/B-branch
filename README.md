
<p align="center"><img src="./images/logo.png" alt="logo" width="250px"/></p>
<h1 align="center">B-branch</h1>
<h3 align="center">A git branch tool extension to git, helping developers manage git branches</h3>

<div align="center">
  <hr/>
 <img src="https://img.shields.io/github/actions/workflow/status/SimonNyvall/B-branch/dotnet.yml?style=flat&label=test%2Fbuild" alt=".NET">&nbsp;&nbsp;
 <img src="https://img.shields.io/github/stars/SimonNyvall/B-branch?style=flat" alt="github stars"/>&nbsp;&nbsp;
 <img src="https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/SimonNyvall/55abca133507cbeebf5256477924be67/raw/clone_count.json" alt="clone count">
</div>

- [Premise](#premise-rocket)
  - [Why B-branch?](#why-b-branch)
  - [Features](#features)
  - [Example](#example)
- [Usage](#usage)
  - [Options](#options)
- [Download](#download-computer)
- [Contributing](#contributing)
- [Acknowledgments](#acknowledgments-mega)
- [License](#license-book)
- [FQAs](#fqas)

## Premise :rocket:

B-branch is a .NET-based application that enhances Git repository management via a git alias. Inspired by **GitButlers** talk, [**So You Think You Know Git?** by Scott Chacon](https://www.youtube.com/watch?v=aolI_Rz0ZqY&t=472s), B-branch provides a structured view of branch information, simplifying workflows for developers.

The triditional git branch sorts branches alphabetically, making it difficult to identify the most recent branches. B-branch addresses this issue by providing a structured view of branch information, including the date of the last commit and the number of commits ahead or behind the upstream branch.

Cross-Platform Support: Built on .NET, **B-branch** runs on any platform supported by .NET 8.0, including Windows, Linux, and macOS. [Download](#download) the latest release for free! No need for dependencies when using the AOT compiled executable.

<img align="center" width="100%" src="./images/screen.jpg" alt="screen" width="500"/>

### Why B-branch?

Imagine you are working in your feature branch, but suddenly you need to switch branch due to a critical bug. You stash your files, switch branch and fix the issue at hand. When you return to your feature branch, you realize that you have forgotten the name of the branch. You could use the `git branch` command to list all the branches, but this command does not provide any additional information about the branches.

This is where B-branch comes in handy. By using the `git bb` command, you can sort the branches on the date of the last commit, the number of commits ahead or behind the upstream branch, or the name of the branch. This makes it easier to identify the branch you are looking for.

1. Make it easier to identify the branch you are looking for.
2. See additional information about the branches, that is not available in the standard `git branch` command.
3. Reliability and performance + cross-platform support.

### Features

- **Branch information**: Displays the branch name, the date of the last commit, the number of commits ahead or behind the upstream branch, and the branch description.

- **Branch description**: Git offers the ability to add a description to a branch. B-branch displays this description in the output.

- **Pager interface**: If the output is too large to fit on the screen, the output will be displayed in a pager interface. While in the pager interface.
  - `q`: Quit the pager interface.
  - `j` OR `Down Arrow`: Move down one line.
  - `k` OR `Up Arrow`: Move up one line.
  - `f` OR `Space`: Move down one page.
  - `b`: Move up one page.
  - `g` OR `HOME`: Move to the top of the output.
  - `G` OR `END`: Move to the bottom of the output.
  - `/`: Search for a string in the output.
  - `Escape`: Clear the search.

### Example

Let's say you have a git repository with a lot of branches. You want to remove the branches that are no longer needed.

```sh
$ git bb --no-contains "main;development" -q \
  | awk '{print substr($0, 3)}' \
  | xargs -I {} git branch -D {}
```

> [!IMPORTANT]
> This command will delete all branches that do not contain the strings "main" or "development". Be careful when using this command.

---

The ability to see the branch description is also a feature that is not available in the standard `git branch` command. To add a description to a branch, use the following command:

```sh
git branch --edit-description
```

> [!NOTE]
> Keep in mind that the description is stored in the `.git/EDIT_DESCRIPTION` and does not support a description on multiple branches.

## Usage

```sh
git bb [options]
```

### Options

- `-t, --track <String>`: Displays information about how many commits the specified branch is ahead or behind relative to its upstream branch.
- `-q, --quiet`: Only displays the names of the branches without any additional information or formatting.
- `-v, --version`: Shows the current version of the `B-branch` tool.
- `-s, --sort <String>`: Sorts the branches based on the specified criterion. Valid options are `[date]`, `[name]`, `[ahead]`, or `[behind]`.
- `-a, --all`: Displays all branches, both local and remote.
- `-n, --no-contains <String>`, `"String1;String2;..."`: Filters the list to only show branches that do not contain the specified string. Valid options are `<String>` OR `"String1;String2;..."`.
- `-c, --contains <String>`, `"String1;String2;..."`: Filters the list to only show branches that contain the specified string. Valid options are `<String>` OR `"String1;String2;..."`.
- `-r, --remote`: Includes remote branches in the output.
- `-h, --help`: Displays the help message with information about all available options.
- `-p, --print-top <Number>`: Prints the top N branches based on the specified sort criterion.

---

## Download :computer:

[Download](https://github.com/SimonNyvall/B-branch/releases) the latest release for free! In the release, you will find the compiled executable for **Windows**, **Linux**, and **MacOS** with an installation script.

> [!NOTE]
> For full experience, download the latest version of the [nerd-fonts](https://www.nerdfonts.com/font-downloads) and install it on your system.

## Contributing

We welcome contributions to **B-branch**! If you have suggestions or improvements, please adhere to the following [guidelines](./CONTRIBUTE.md) when contributing to the project. Don't forget to give [the project](https://github.com/SimonNyvall/B-branch) a **star!** ⭐ Thanks again!

## Acknowledgments :mega:

This project was inspired by the innovative ideas shared by [**GitButler**](https://www.youtube.com/watch?v=aolI_Rz0ZqY&t=472s). Check out their video for more insights into enhancing Git workflows.

## License :book:

This project is licensed under the [MIT License](./LICENSE) - see the LICENSE.md file for details.

## FQAs

- **Q**: How do I add a description to a branch?
  - **A**: Use the following command: `git branch --edit-description`

- **Q**: Can I use this instead of the standard `git branch` command?
  - **A**: B-branch only helps developers view branch information. It does not replace the standard `git branch` command.

- **Q**: Will B-branch slow down git?
  - **A**: No, the extension works with an alias and does not affect the performance of git.

- **Q**: What is a common use case for B-branch?
  - **A**: A common use case I use is to check how many commits I am behind or ahead of the upstream main branch. To see if I need to pull or merge the changes. This can be done by running `git bb -t "origin/main"`.
