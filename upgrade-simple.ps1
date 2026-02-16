# Simple .NET 10 upgrade script
Write-Host "Starting .NET 10 upgrade..." -ForegroundColor Cyan

# Step 1: Update TargetFramework
$files = Get-ChildItem -Path "src" -Filter "*.csproj" -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $content = $content -replace 'net8\.0', 'net10.0'
    $content = $content -replace 'net6\.0', 'net10.0'
    Set-Content -Path $file.FullName -Value $content -NoNewline
    Write-Host "Updated: $($file.Name)" -ForegroundColor Gray
}

Write-Host "`nTargetFramework updated to net10.0" -ForegroundColor Green
Write-Host "Next: Run dotnet restore and dotnet build" -ForegroundColor Yellow
