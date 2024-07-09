#!/usr/bin/env bash

current_dir=$(pwd)

cd "$current_dir/src/CLI" || { echo "Failed to change directory to $current_dir/src/CLI"; exit 1; }

echo "Building CLI"
dotnet build || { echo "Failed to build the CLI"; exit 1; }

echo "Installing CLI"

publish_dir="$HOME/.local/share/B-branch"

if [ ! -d "$publish_dir" ]; then
  mkdir -p "$publish_dir" || { echo "Failed to create directory $publish_dir"; exit 1; }
fi

dotnet publish -c Release -o "$publish_dir" || { echo "Failed to publish the CLI"; exit 1; }

cli_executable="$publish_dir/CLI"

if [ ! -f "$cli_executable" ]; then
  echo "Error: CLI executable not found at $cli_executable"
  exit 1
fi

git config --global alias.bb '!bash -c '\''"'"$cli_executable"'" "$@"'\'' bash' || { echo "Failed to set git alias"; exit 1; }

echo "CLI installed successfully"
