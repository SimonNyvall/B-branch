# Contributing to B-branch

Welcome to B-branch! We are thrilled to have you here, and we appreciate your interest in contributing to our project. By contributing, you help make this project even better for everyone.
How to Contribute

## Get Started

- Fork the repository on GitHub.

- Clone your fork of the repository locally:

```sh
git clone https://github.com/<your-username>/B-branch.git && cd B-branch
```

Create a new branch for your changes:
```sh
git checkout -b feature/your-feature-name
```
Please use a descriptive branch name that reflects the feature or fix you are working on.

Make your changes and commit them to your branch:

```sh
git commit -m 'feat: add new feature'
```

## Creating a development environment

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
Prerequisites

- [git](https://git-scm.com/downloads) 2.39.2 or later

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) SDK

- [nerd-fonts](https://www.nerdfonts.com/font-downloads) 2.1.0 or later **(optional)**

> [!INFO]
> Nerd fonts are optional but recommended for the best experience. The font is used to display the icon information in the terminal.

In case on installing B-branch on your system, you can find the alias link in the the `.gitconfig` file. The alias is `bb` and points to the executable. The executable is located in the `bin` folder of the project. Below is the common location of the `.gitconfig` file.

| OS      | Location of .gitconfig       |
| ------- | ---------------------------- |
| Linux   | ~/.gitconfig                 |
| MacOS   | ~/.gitconfig                 |
| Windows | C:\Users\USERNAME\.gitconfig |

## Conversations

- **File nameing**: Please make sure to follow the naming conventions of the project. Where top level directories or files follow the [camelCase](https://en.wikipedia.org/wiki/Camel_case) naming convention, meanwhile project directories and files follow the [PascalCase](https://en.wikipedia.org/wiki/PascalCase) naming convention.

- **Commit messages**: Please write clear commit messages, more clear then the one above at least.
The repo typically follows the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.

- **PRs**: Please make sure to follow the PR template when creating a PR.

## Code of Conduct

Please note that we have a Code of Conduct in place to ensure a friendly and welcoming environment for all contributors. By participating in this project, you are expected to uphold this code.
Help and Support

If you need any help or have questions, feel free to open an issue in the repository. We are here to assist you.

## Reporting Bugs

Please help us improve B-branch by reporting any bugs you encounter.
Bugs can be reported in the projects [issue tracker](https://github.com/SimonNyvall/B-branch/issues).

## Feature Requests

If you have a feature request, please open an [issue](https://github.com/SimonNyvall/B-branch/issues) and explain the feature you would like to see.
License.

By contributing to B-branch, you agree that your contributions will be licensed under the [MIT LICENSE](./LICENSE).