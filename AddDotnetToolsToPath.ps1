# Add dotnet tools to Windows PATH permanently
Write-Host "Adding dotnet tools to System PATH..." -ForegroundColor Cyan

$dotnetToolsPath = "C:\Users\USER\.dotnet\tools"

# Get current PATH from User Environment Variables
$currentPath = [Environment]::GetEnvironmentVariable("Path", "User")

# Check if the path is already added
if ($currentPath -notlike "*$dotnetToolsPath*") {
    # Add to User PATH
    $newPath = $currentPath + ";" + $dotnetToolsPath
    [Environment]::SetEnvironmentVariable("Path", $newPath, "User")
    
    Write-Host "Successfully added to User PATH" -ForegroundColor Green
    Write-Host "Path added: $dotnetToolsPath" -ForegroundColor Gray
    Write-Host "`nPlease restart PowerShell/Terminal for changes to take effect." -ForegroundColor Yellow
}
else {
    Write-Host "Path already exists in environment variables" -ForegroundColor Green
}

# Also update current session
$env:PATH += ";$dotnetToolsPath"
Write-Host "`nCurrent session PATH updated" -ForegroundColor Green

# Verify dotnet-ef is accessible
Write-Host "`nVerifying dotnet-ef..." -ForegroundColor Cyan
$efTool = Get-Command dotnet-ef -ErrorAction SilentlyContinue
if ($efTool) {
    Write-Host "dotnet-ef found at: $($efTool.Source)" -ForegroundColor Green
    Write-Host "`nVersion info:" -ForegroundColor Cyan
    & dotnet-ef --version
}
else {
    Write-Host "dotnet-ef not found. Please restart PowerShell." -ForegroundColor Red
}
