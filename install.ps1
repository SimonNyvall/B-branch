$current_dir = Get-Location

Set-Location "$current_dir/src/CLI" -ErrorAction Stop

Write-Output "Building CLI"
dotnet build -ErrorAction Stop

Write-Output "Installing CLI"

$publish_dir = "$env:USERPROFILE\AppData\Local\B-branch"

if (-Not (Test-Path -Path $publish_dir -PathType Container)) {
    New-Item -ItemType Directory -Path $publish_dir -Force | Out-Null
}

dotnet publish -c Release -o $publish_dir -ErrorAction Stop

$cli_executable = "$publish_dir\CLI.exe"

if (-Not (Test-Path -Path $cli_executable -PathType Leaf)) {
    Write-Output "Error: CLI executable not found at $cli_executable"
    exit 1
}

$alias_command = "!powershell -NoProfile -Command \"& { & '$cli_executable' $([String]::Join(' ', $args)) }\""
git config --global alias.bb $alias_command

Write-Output "CLI installed successfully"
