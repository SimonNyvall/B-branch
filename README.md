# B-branch [![.NET](https://github.com/SimonNyvall/B-branch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/SimonNyvall/B-branch/actions/workflows/dotnet.yml)

[B-branch] is a .NET-based application designed to enhance the way developers interact with Git repositories directly from their terminal. Inspired by innovative ideas shared in a video by GitButler on YouTube, this tool aims to streamline the workflow for managing and visualizing Git branches, making it easier to see branch details at a glance.
Features

    Branch Table Visualization: Displays an organized table of branches in your Git repository, including ahead/behind information relative to the main branch and the date of the last commit.
    Easy Integration: Seamlessly integrates with your existing .NET projects, enhancing your development workflow with minimal setup.
    Cross-Platform Support: Built on .NET, [Your Project Name] runs on any platform supported by .NET 8.0, including Windows, Linux, and macOS.

Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
Prerequisites

    .NET 8.0 SDK installed on your machine.

Installation

    Clone the repository:

```sh
git clone https://github.com/[SimonNyvall]/[B-branch].git
```

Navigate to the project directory:

```sh
cd [B-branch]/src
```

Restore the project dependencies:

sh

dotnet restore

Build the project:

```sh
    dotnet build
```

Usage

To run [B-branch], follow these steps after building the project:

```sh
dotnet run
```

This command will start the application, scanning the local Git repository for branch information and displaying it in a structured table format.
Contributing

We welcome contributions to [B-branch]! If you have suggestions or improvements, please fork the repo and create a pull request, or open an issue with the tag "enhancement". Don't forget to give the project a star! Thanks again!
Acknowledgments

    This project was inspired by the innovative ideas shared by GitButler. Check out their video for more insights into enhancing Git workflows.

License

This project is licensed under the MIT License - see the LICENSE.md file for details.

