<p align="center"><img src="./images/git-logo.png" alt="logo" width="128px"/></p>
<div align="center">
<pre>
██████╗       ██████╗ ██████╗  █████╗ ███╗   ██╗ ██████╗██╗  ██╗
██╔══██╗      ██╔══██╗██╔══██╗██╔══██╗████╗  ██║██╔════╝██║  ██║
██████╔╝█████╗██████╔╝██████╔╝███████║██╔██╗ ██║██║     ███████║
██╔══██╗╚════╝██╔══██╗██╔══██╗██╔══██║██║╚██╗██║██║     ██╔══██║
██████╔╝      ██████╔╝██║  ██║██║  ██║██║ ╚████║╚██████╗██║  ██║
╚═════╝       ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝╚═╝  ╚═╝
</pre>
</div>



<h3 align="center"><img width="20px" src="./images/mini-git-logo.png" style="vertical-align: middle;"/>  git branch, <strong>but make it better</strong> :wrench:</h3>
<p align="center"><strong>Download for</strong> macOS (<a href="https://github.com/SimonNyvall/B-branch/releases">Apple Silicon</a>) · Linux (<a href="https://github.com/SimonNyvall/B-branch/releases">x64</a>) · Windows (<a href="https://github.com/SimonNyvall/B-branch/releases">x64</a>)</p>

<div align="center">
  <hr/>
 <img src="https://img.shields.io/github/actions/workflow/status/SimonNyvall/B-branch/build-test.yml?style=flat&logoColor=white&label=test%2Fbuild&color=green" alt=".NET">&nbsp;&nbsp;
 <img src="https://img.shields.io/github/stars/SimonNyvall/B-branch?style=flat&color=yellow" alt="github stars"/>&nbsp;&nbsp;
 <img src="https://img.shields.io/github/downloads/SimonNyvall/B-branch/total?style=flat&logo=Github&color=blue" alt="download count">&nbsp;&nbsp
 <img src="https://img.shields.io/badge/GitButler-%23B9F4F2?logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyB3aWR0aD0iMzkiIGhlaWdodD0iMjgiIHZpZXdCb3g9IjAgMCAzOSAyOCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTI1LjIxNDUgMTIuMTk5N0wyLjg3MTA3IDEuMzg5MTJDMS41NDI5NSAwLjc0NjUzMiAwIDEuNzE0MDYgMCAzLjE4OTQ3VjI0LjgxMDVDMCAyNi4yODU5IDEuNTQyOTUgMjcuMjUzNSAyLjg3MTA3IDI2LjYxMDlMMjUuMjE0NSAxNS44MDAzQzI2LjcxOTcgMTUuMDcyMSAyNi43MTk3IDEyLjkyNzkgMjUuMjE0NSAxMi4xOTk3WiIgZmlsbD0iYmxhY2siLz4KPHBhdGggZD0iTTEzLjc4NTUgMTIuMTk5N0wzNi4xMjg5IDEuMzg5MTJDMzcuNDU3MSAwLjc0NjUzMiAzOSAxLjcxNDA2IDM5IDMuMTg5NDdWMjQuODEwNUMzOSAyNi4yODU5IDM3LjQ1NzEgMjcuMjUzNSAzNi4xMjg5IDI2LjYxMDlMMTMuNzg1NSAxNS44MDAzQzEyLjI4MDMgMTUuMDcyMSAxMi4yODAzIDEyLjkyNzkgMTMuNzg1NSAxMi4xOTk3WiIgZmlsbD0idXJsKCNwYWludDBfcmFkaWFsXzMxMF8xMjkpIi8%2BCjxkZWZzPgo8cmFkaWFsR3JhZGllbnQgaWQ9InBhaW50MF9yYWRpYWxfMzEwXzEyOSIgY3g9IjAiIGN5PSIwIiByPSIxIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgxNi41NzAxIDE0KSBzY2FsZSgxOS44NjQxIDE5LjgzODMpIj4KPHN0b3Agb2Zmc2V0PSIwLjMwMTA1NiIgc3RvcC1vcGFjaXR5PSIwIi8%2BCjxzdG9wIG9mZnNldD0iMSIvPgo8L3JhZGlhbEdyYWRpZW50Pgo8L2RlZnM%2BCjwvc3ZnPgo%3D" alt="gitbutler"/>
</div>

<details>
<summary>Table of content</summary>

- [Premise](#premise-rocket)
  - [Features](#features)
  - [Example](#example)
- [Usage](#usage)
  - [Options](#options)
- [Download](#download-computer)
  - [Next Release](#next-release)
- [Contributing](#contributing)
- [Acknowledgments](#acknowledgments-mega)
- [License](#license-book)
- [FQAs](#fqas)

</details>

## 🌿 What is B-branch?

You've been staring at `git branch` output for years, an alphabetical wall of names with zero context. *When was this branch last touched? How far behind is it? What was I even doing here?*

**B-branch fixes that.**

It replaces the blunt `git branch` command with a rich, structured view that shows you what actually metters; **recency, divergence, and description**

> [!IMPORTANT]
> Inspired by Scott Chacon's talk [So You Think You Know Git? by Scott Chacon](https://www.youtube.com/watch?v=aolI_Rz0ZqY&t=472s) and the workflow philosophy behind **GitButler**

<img align="center" width="100%" src="./images/banner2.png" alt="screen" width="500"/>

---

## ✨ Features at a Glance
| Feature     | Description      |
| ------------- | ------------- |
| Sorting  | Sort branches how **you** want |
| Ahead / Behind | See how many commits each branch is ahead or behind upstream or compare different branches |
| Branch description  | Display custom per-branch description |
| Pagenation  | Same pagenation as our favorite `git` |
| Regex filtering  | `--contains` and `--no-contains` with full regex support |
| Zero dependencies | Single compiled binary, run on Windows, Linux or Mac |
| Nerd Font Icons  | Beautiful glyphs when you have Nerd Font installed |
| Stale branch detection  | View all of your old stale branches |
| Git branch  | All of the `git branch` command features we already love |

## 🚀 Quick Start
**Download** (recommended)

Grab the latest binary for your platform from the [Releases page](https://github.com/SimonNyvall/B-branch/releases) - no runtime required.

**Now run it:**
``` sh
git bb
```

That's it. You're already living better.

## 🔧 Usage
``` sh
# List branches (sorted by recency)
git bb

# Show all branches including remote
git bb --all

# Filter branches by string
git bb --contains "feature"

# Show stale branches (upcoming feature)
git bb --stale

# Quiet mode (just branch names, useful for scripting)
git bb -q
```

## 📝 Branch Description
Git supports per-branch description natively... B-branch actually shows them.
``` sh
# Add a description to the current branch
git branch --edit-description
```
This will open an editor where you can set what description should be on what branch.
```text
[main]
This is the description that will show up on the main branch.
```

## 🖥️ Pager Keybindings
| Key     | Action      |
| ------------- | ------------- |
| j / k | Scroll down / up |
| G / END | Jump to bottom |
| g / HOME  | Jump to top |
| /  | Search |
| n / N  | Next / previous search result |
| ESC | Clear search |
| q  | Quit |
| ! | Execute shell command |

## 💡 Nerd Fonts
For the full icon experience, install [Nerd Font]() and enable it:
``` sh
git config --global vars.useNerdFonts true
```

## Contributing

We welcome contributions to **B-branch**! If you have suggestions or improvements, please adhere to the following [guidelines](./CONTRIBUTE.md) when contributing to the project.

Don't forget to ⭐ **star** the repo if B-branch makes your day a little less painful.

## Acknowledgments :mega:

This project was inspired by the innovative ideas shared by [**GitButler**](https://www.youtube.com/watch?v=aolI_Rz0ZqY&t=472s). Check out their video for more insights into enhancing Git workflows.

## License :book:

This project is licensed under the [GPL-3.0 License](./LICENSE) - see the LICENSE.md file for details.

---

## *Your workflow deserver better.*
# ⏬ [Download latest release](https://github.com/SimonNyvall/B-branch/releases)