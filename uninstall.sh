#!/bin/bash
set -e

BIN_DIR_UNIX="/usr/local/bin/b-branch"
BIN_DIR_WIN="AppData\\Local\\b-branch"
GITCONFIG_UNIX="${HOME}/.gitconfig"
GITCONFIG_WIN="$USERPROFILE\\.gitconfig"

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

    echo "B-branch uninstallation completed!"
}

main "$@"
