#!/bin/sh
set -e

# Constants
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

# Pre-installation Cleanup
cleanup() {
    echo "Starting pre-installation cleanup..."
    if [ "${OS}" = "Windows" ]; then
        echo "Removing previous installation from ${BIN_DIR_WIN}"
        rm -rf "$USERPROFILE\\${BIN_DIR_WIN}"
        sed -i '/alias.bb/d' "$GITCONFIG_WIN" || true
    else
        echo "Removing previous installation from ${BIN_DIR_UNIX}"
        sudo rm -rf "${BIN_DIR_UNIX}"
        sed -i '/alias.bb/d' "${GITCONFIG_UNIX}" || true
    fi
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
        *) echo "Unsupported architecture: ${ARCH}" && exit 1 ;;
    esac

    case "${OS}" in
        Linux) OS="linux" ;;
        Darwin) OS="mac" ;;
        CYGWIN*|MINGW*|MSYS*|MINGW32*|MINGW64*) OS="win" ;;
        *) echo "Unsupported OS: ${OS}" && exit 1 ;;
    esac

    # Construct download URL based on OS and architecture
    URL="https://your-download-url.com/b-branch-${OS}-${ARCH}.zip"
    echo "Downloading B-branch from ${URL}"

    # Download the file using curl or wget
    if command -v curl >/dev/null 2>&1; then
        curl -sSfLO "${URL}"
    elif command -v wget >/dev/null 2>&1; then
        wget "${URL}"
    else
        echo "Error: curl or wget is required to download files." && exit 1
    fi

    # Extract the downloaded archive
    ZIP_FILE="b-branch-${OS}-${ARCH}.zip"
    if [ -f "${ZIP_FILE}" ]; then
        echo "Extracting B-branch archive..."
        unzip "${ZIP_FILE}" || { echo "Unzip failed"; exit 1; }
    else
        echo "Download failed: ${ZIP_FILE} not found." && exit 1
    fi
}

# Move Binary to the Correct Directory
install_binary() {
    echo "Installing B-branch..."

    if [ "${OS}" = "Windows" ]; then
        DEST_PATH="$USERPROFILE\\${BIN_DIR_WIN}"
        mkdir -p "${DEST_PATH}"
        mv "b-branch-${ARCH}" "${DEST_PATH}"
    else
        sudo mkdir -p "${BIN_DIR_UNIX}"
        sudo mv "b-branch-${ARCH}" "${BIN_DIR_UNIX}"
    fi
    echo "B-branch installed successfully."
}

# Configure Git Alias
configure_git_alias() {
    echo "Setting up git alias..."
    if [ "${OS}" = "Windows" ]; then
        git config --global alias.bb "!f() { $USERPROFILE\\${BIN_DIR_WIN}\\CLI.exe \"\$@\"; }; f"
    else
        git config --global alias.bb "!bash -c '\"${BIN_DIR_UNIX}/CLI\" \"\$@\"' bash"
    fi
    echo "Git alias 'bb' configured successfully."
}

# Main installation process
main() {
    OS=$(detect_os)
    if [ "${OS}" = "UNKNOWN" ]; then
        echo "Unsupported OS." && exit 1
    fi

    cleanup
    download_binary
    install_binary
    configure_git_alias

    echo "B-branch installation completed!"
}

# Run the main function
main "$@"
