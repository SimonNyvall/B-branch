#!/bin/bash
set -e

BIN_DIR_UNIX="/usr/local/bin/b-branch"
BIN_DIR_WIN="AppData\\Local\\b-branch"
GITCONFIG_UNIX="${HOME}/.gitconfig"
GITCONFIG_WIN="$USERPROFILE\\.gitconfig"

maybe_remove_unzip() {
    if [ "${OS}" != "Linux" ]; then
        return 0
    fi

    if ! command -v unzip >/dev/null 2>&1; then
        return 0
    fi

    echo "Optional: Remove the 'unzip' package?"
    echo "Warning: Other programs may depend on it."
    printf "Do you want to uninstall unzip? [y/N]: "

    if [ -t 0 ]; then
        read -r answer
    else
        read -r answer < /dev/tty
    fi

    case "$answer" in
        y|Y)
            echo "Uninstalling unzip..."

            if command -v apt >/dev/null 2>&1; then
                sudo apt remove -y unzip
            elif command -v dnf >/dev/null 2>&1; then
                sudo dnf remove -y unzip
            elif command -v pacman >/dev/null 2>&1; then
                sudo pacman -R --noconfirm unzip
            else
                echo "Unsupported package manager."
                return 1
            fi

            echo "'unzip' removed."
            ;;
        *)
            echo "Keeping 'unzip' installed."
            ;;
    esac
}

# Helper function to detect OS
detect_os() {
    case "$(uname -s)" in
        Linux*)     OS=Linux;;
        Darwin*)    OS=Mac;;
        CYGWIN*|MINGW*|MSYS*|MINGW32*|MINGW64*) OS=Windows;;
        *)          OS="UNKNOWN";;
    esac
    echo "${OS}"
}

# Uninstallation function
cleanup() {
    echo "Starting pre-installation cleanup..."
    if [ "${OS}" = "Windows" ]; then
        echo "Removing previous installation from ${BIN_DIR_WIN}"
        rm -rf "$USERPROFILE\\${BIN_DIR_WIN}"
    else
        echo "Removing previous installation from ${BIN_DIR_UNIX}"
        sudo rm -rf "${BIN_DIR_UNIX}"
    fi
    git config --global --unset alias.bb
    echo "Cleanup completed."
}

main() {
    OS=$(detect_os)
    if [ "${OS}" = "UNKNOWN" ]; then
        echo "Unsupported OS." && exit 1
    fi

    cleanup || { echo "Cleanup failed"; exit 1; }

    maybe_remove_unzip

    echo "B-branch uninstallation completed!"
}

main "$@"
