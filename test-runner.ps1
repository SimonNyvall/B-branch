$projectPath = Get-Location
$dockerImageName = "integration-test-image"
$dockerContainerName = "integration-test-container"

function Write-ProgressBar($progress) {
    $progress = [int]$progress
    if ($progress -lt 0) { $progress = 0 }
    if ($progress -gt 100) { $progress = 100 }
    
    $barLength = 20  # Length of the bar (number of blocks to fill)
    $filledLength = [math]::Floor($progress / 5)  # 5% per block
    $emptyLength = $barLength - $filledLength

    $filledBar = "#" * $filledLength
    $emptyBar = " " * $emptyLength

    Write-Host "    [$filledBar$emptyBar] $progress%"

    Write-Host ""
}

Write-ProgressBar 0

Write-Host "[INFO] Cleaning up Docker resources..." -ForegroundColor Green
docker rm -f $dockerContainerName > $null 2>&1
docker rmi $dockerImageName > $null 2>&1

Write-ProgressBar 20

Write-Host "[INFO] Running unit tests..." -ForegroundColor Green
$unitTestOutput = & dotnet test "$projectPath/tests/UnitTests" -v n
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Unit tests encountered errors." -ForegroundColor Red
} else {
    Write-Host "[INFO] Unit tests completed." -ForegroundColor Green
}

$unitTestPassedCount = ($unitTestOutput | Select-String -Pattern "Passed: (\d+)" | ForEach-Object { $_.Matches.Groups[1].Value }) -join ''
$unitTestFailedCount = ($unitTestOutput | Select-String -Pattern "Failed: (\d+)" | ForEach-Object { $_.Matches.Groups[1].Value }) -join ''

if (-not $unitTestPassedCount) { $unitTestPassedCount = 0 }
if (-not $unitTestFailedCount) { $unitTestFailedCount = 0 }

Write-ProgressBar 40

Write-Host "[INFO] Building Docker image..." -ForegroundColor Green
docker build -t $dockerImageName $projectPath > $null 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Docker build failed." -ForegroundColor Red
    exit 1
} else {
    Write-Host "[INFO] Docker image built successfully." -ForegroundColor Green
}

Write-ProgressBar 60

Write-Host "[INFO] Running Docker container for integration tests..." -ForegroundColor Green
$integrationTestOutput = docker run --name $dockerContainerName $dockerImageName

$integrationTestPassedCount = ($integrationTestOutput | Select-String -Pattern "Passed: (\d+)" | ForEach-Object { $_.Matches.Groups[1].Value }) -join ''
$integrationTestFailedCount = ($integrationTestOutput | Select-String -Pattern "Failed: (\d+)" | ForEach-Object { $_.Matches.Groups[1].Value }) -join ''

if (-not $integrationTestPassedCount) { $integrationTestPassedCount = 0 }
if (-not $integrationTestFailedCount) { $integrationTestFailedCount = 0 }

Write-ProgressBar 80

Write-Host "[INFO] Cleaning up Docker resources..." -ForegroundColor Green

docker rm -f $dockerContainerName > $null 2>&1
docker rmi $dockerImageName > $null 2>&1

Write-ProgressBar 100

Write-Host "------------------------"
Write-Host "Test Summary:"
Write-Host "Unit Tests - Passed: " -NoNewline
Write-Host "$unitTestPassedCount" -ForegroundColor Green -NoNewline
Write-Host ", Failed: " -NoNewline
Write-Host "$unitTestFailedCount" -ForegroundColor Red
Write-Host "Integration Tests - Passed: " -NoNewline
Write-Host "$integrationTestPassedCount" -ForegroundColor Green -NoNewline
Write-Host ", Failed: " -NoNewline
Write-Host "$integrationTestFailedCount" -ForegroundColor Red
Write-Host "------------------------"
