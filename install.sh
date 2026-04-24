#!/bin/sh
set -e

# Constants
BIN_DIR_UNIX="~/.local/bin/b-branch"
BIN_DIR_WIN="AppData\\Local\\b-branch"
GITCONFIG_UNIX="${HOME}/.gitconfig"
GITCONFIG_WIN="$USERPROFILE\\.gitconfig"
DEFAULT_VERSION="1.2.0"
VERSION="${DEFAULT_VERSION}"
FINAL_BINARY=""
LESS_COMMAND_PATH=""

# Parse command line arguments
while [ $# -gt 0 ]; do
    case "$1" in
        --version=*)
            VERSION="${1#*=}"
            ;;
        --version)
            if [ -n "$2" ] && [ "${2:0:1}" != "-" ]; then
                VERSION="$2"
                shift
            else
                echo "Error: --version requires a version number" >&2
                exit 1
            fi
            ;;
        *)
            echo "Unknown parameter: $1" >&2
            echo "Usage: $0 [--version=X.Y.Z]" >&2
            exit 1
            ;;
    esac
    shift
done

echo "Installing B-branch version ${VERSION}..."

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

# Pre-installation Cleanup
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

# Download and Extract the Appropriate Binary
download_binary() {
    ARCH=$(uname -m)
    OS=$(uname -s)
    echo "Detected OS: ${OS}"
    echo "Detected architecture: ${ARCH}"

    # Set architecture and OS name to match the file naming format
    case "${ARCH}" in
        x86_64) ARCH="x64" ;;
        aarch64) ARCH="arm64" ;;
        arm64) ARCH="arm64" ;;
        *) echo "Unsupported architecture: ${ARCH}" && exit 1 ;;
    esac

    case "${OS}" in
        Linux) OS="linux" ;;
        Darwin) OS="mac" ;;
        CYGWIN*|MINGW*|MSYS*|MINGW32*|MINGW64*) OS="win" ;;
        *) echo "Unsupported OS: ${OS}" && exit 1 ;;
    esac

    # Construct download URL based on OS and architecture
    URL="https://github.com/SimonNyvall/B-branch/releases/download/v${VERSION}/b-branch-${OS}-${ARCH}.zip"
    echo "Downloading B-branch from ${URL}"

    # Download the file using curl
    if command -v curl >/dev/null 2>&1; then
        if [ "${OS}" = "win" ]; then
            mkdir -p "${USERPROFILE}\\AppData\\Local\\b-branch"
            curl -sSfLo "${USERPROFILE}\\${BIN_DIR_WIN}\\b-branch-${OS}-${ARCH}.zip" "${URL}"
        else
            mkdir -p "${BIN_DIR_UNIX}"
            curl -sSfLo "${BIN_DIR_UNIX}/b-branch-${OS}-${ARCH}.zip" "${URL}"
        fi
    else
        echo "Error: curl is required to download files." && exit 1
    fi

    # Extract the downloaded archive
    ZIP_FILE=""
    TARGET_DIR=""

    if [ "${OS}" = "win" ]; then
        ZIP_FILE="${USERPROFILE}\\${BIN_DIR_WIN}\\b-branch-${OS}-${ARCH}.zip"
        TARGET_DIR="${USERPROFILE}\\${BIN_DIR_WIN}"
        USERPROFILE_UNIX=$(echo "$USERPROFILE" | sed 's/\\/\//g')
        BIN_DIR_WIN_UNIX=$(echo "$BIN_DIR_WIN" | sed 's/\\/\//g')
        FINAL_BINARY="${USERPROFILE_UNIX}/${BIN_DIR_WIN_UNIX}/b-branch-${OS}-${ARCH}"
        LESS_COMMAND_PATH=$(where less | head -n 1)
    else
        ZIP_FILE="${BIN_DIR_UNIX}/b-branch-${OS}-${ARCH}.zip"
        TARGET_DIR="${BIN_DIR_UNIX}"
        FINAL_BINARY="${BIN_DIR_UNIX}/b-branch-${OS}-${ARCH}"
        LESS_COMMAND_PATH=$(which less | head -n 1)
    fi

    if [ -f "${ZIP_FILE}" ]; then
        echo "Extracting B-branch archive..."
        unzip -o "${ZIP_FILE}" -d "${TARGET_DIR}" || { echo "Unzip failed"; exit 1; }

        rm -f "${ZIP_FILE}"
    else
        echo "Download failed: ${ZIP_FILE} not found." && exit 1
    fi
}

# Configure Git Alias
configure_git_alias() {
    echo "Linking B-branch Cli to local git..."

    if [ "${OS}" = "win" ]; then
        git config --global alias.bb "!f() { $FINAL_BINARY/Cli.exe \"$LESS_COMMAND_PATH\" \"\$@\"; }; f"
    else
        git config --global alias.bb "!bash -c '\"${FINAL_BINARY}/Cli\" \"$LESS_COMMAND_PATH\" \"\$@\"' bash"
    fi

    echo "Git alias 'bb' configured successfully."
    echo "B-branch installation was successful!"
}

ensure_unzip() {
    if [ "$(uname -s)" != "Linux"]; then
        return 0
    fi

    # Already installed
    if command -v unzip >/dev/null 2>&1; then
        return 0
    fi

    echo "The 'unzip' utility is required but not installed."
    printf "Would you like to install it now? [Y/n]: "
    read -r answer

    case "$answer" in
        ""|Y|y)
            echo "Installing unzip..."

            if command -v apt >/dev/null 2>&1; then
                sudo apt update && sudo apt install -y unzip
            elif command -v dnf >/dev/null 2>&1; then
                sudo dnf install -y unzip
            elif command -v pacman >/dev/null 2>&1; then
                sudo pacman -S --noconfirm unzip
            else
                echo "Unsupported package manager. Please install 'unzip' manually."
                exit 1
            fi

            # Verify installation
            if ! command -v unzip >/dev/null 2>&1; then
                echo "Installation failed or unzip still not found."
                exit 1
            fi
            ;;
        *)
            echo "Cannot continue without 'unzip'."
            exit 1
            ;;
    esac
}

main() {
    OS=$(detect_os)
    if [ "${OS}" = "UNKNOWN" ]; then
        echo "Unsupported OS." && exit 1
    fi

    ensure_unzip || { echo "Install of unzip failed"; exit 1; }
    cleanup || { echo "Cleanup failed"; exit 1; }
    download_binary || { echo "Download failed"; exit 1; }
    configure_git_alias || { echo "Git alias configuration failed"; exit 1; }

    echo "B-branch installation completed!"
}

main "$@"
