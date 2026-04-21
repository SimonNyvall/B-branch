# Contributing to B-branch

Welcome to **B-branch** 👋
We’re glad you’re here and appreciate your interest in contributing! Every contribution helps improve the project for everyone.

---

## 🚀 Getting Started

1. **Fork the repository** on GitHub

2. **Clone your fork locally**

```sh
git clone https://github.com/<your-username>/B-branch.git
cd B-branch
```

3. **Create a new branch**

```sh
git checkout -b feature/your-feature-name
```

Use a descriptive branch name that reflects your change.

4. **Make your changes and commit**

```sh
git commit -m "feat: add new feature"
```

---

## 🛠 Development Environment

These instructions will help you set up a local development environment.

### Prerequisites

* [Git](https://git-scm.com/downloads) (2.39.2 or later)
* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
* [Docker](https://docs.docker.com/desktop/) *(for integration tests)*
* [Nerd Fonts](https://www.nerdfonts.com/font-downloads) *(optional but recommended)*

> ℹ️ Nerd Fonts improve how icons are displayed in the terminal.

---

## ⚙️ Git Alias Setup (Optional)

B-branch can be used via a Git alias (`bb`).
This is defined in your `.gitconfig` file.

Common locations:

| OS      | Path                           |
| ------- | ------------------------------ |
| Linux   | `~/.gitconfig`                 |
| macOS   | `~/.gitconfig`                 |
| Windows | `C:\Users\USERNAME\.gitconfig` |

The alias points to the compiled executable, typically found in the `bin` directory.

---

## ▶️ Running, Building & Testing

### Run the Cli

```sh
cd ./src/Cli
dotnet run
```

Or press **F5** in VS Code.

---

### Build the project

```sh
dotnet build
```

Run this from the project root (where `B-branch.sln` is located).

---

### Run Unit Tests

```sh
cd ./tests/UnitTests
dotnet test
```

---

### Run Integration Tests

Make sure Docker is installed and running:

```sh
docker build -t b-branch-integration-tests .
docker run b-branch-integration-tests
```

---

## 📏 Conventions

### Naming

* Top-level files/folders: **camelCase**
* Project files/folders: **PascalCase**

---

### Commit Messages

* Follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
* Write clear and descriptive messages

Example:

```sh
feat: add branch filtering
fix: handle null reference in Cli output
```

---

### Pull Requests

* Link to an existing issue if possible
* If no issue exists, explain **why** the change is needed
* Keep PRs focused and small when possible

---

## 🐛 Reporting Bugs

Found a bug? Please report it in the issue tracker:
https://github.com/SimonNyvall/B-branch/issues

---

## 💡 Feature Requests

Have an idea? Open an issue and describe the feature you’d like to see:
https://github.com/SimonNyvall/B-branch/issues

---

## 🤝 Code of Conduct

Please follow the project’s Code of Conduct to help maintain a welcoming and respectful community.

---

## 📄 License

By contributing to B-branch, you agree that your contributions will be licensed under the **GPL-3.0 License**.
