# Script to upgrade Refahi.Notif solution to .NET 10
# Run this from the notif folder

Write-Host "Starting .NET 10 upgrade process..." -ForegroundColor Cyan

# Step 1: Update all .csproj files to target net10.0
Write-Host "`nStep 1: Updating TargetFramework to net10.0..." -ForegroundColor Yellow

# Get all .csproj files
$csprojFiles = Get-ChildItem -Path "src" -Filter "*.csproj" -Recurse

foreach ($file in $csprojFiles) {
    Write-Host "  Processing $($file.Name)..." -ForegroundColor Gray
    
    $content = Get-Content $file.FullName -Raw
    
    # Replace target frameworks
    $content = $content -replace '<TargetFramework>net8\.0</TargetFramework>', '<TargetFramework>net10.0</TargetFramework>'
    $content = $content -replace '<TargetFramework>net6\.0</TargetFramework>', '<TargetFramework>net10.0</TargetFramework>'
    
    # Save the file
    Set-Content -Path $file.FullName -Value $content -NoNewline
}

Write-Host "  ✓ TargetFramework updated in all projects" -ForegroundColor Green

# Step 2: Update NuGet packages
Write-Host "`nStep 2: Updating NuGet packages to .NET 10 compatible versions..." -ForegroundColor Yellow

# Navigate to src folder
Push-Location src

# Define package updates (package name -> version)
$packagesToUpdate = @{
    # Microsoft packages
    "Microsoft.AspNetCore.Authentication.JwtBearer" = "10.0.0"
    "Microsoft.AspNetCore.Authentication.OpenIdConnect" = "10.0.0"
    "Microsoft.AspNetCore.Identity.EntityFrameworkCore" = "10.0.0"
    "Microsoft.EntityFrameworkCore" = "10.0.0"
    "Microsoft.EntityFrameworkCore.SqlServer" = "10.0.0"
    "Microsoft.EntityFrameworkCore.Tools" = "10.0.0"
    "Microsoft.EntityFrameworkCore.Design" = "10.0.0"
    "Microsoft.EntityFrameworkCore.Relational" = "10.0.0"
    "Microsoft.Extensions.Caching.Abstractions" = "10.0.0"
    "Microsoft.Extensions.Caching.Memory" = "10.0.0"
    "Microsoft.Extensions.Caching.StackExchangeRedis" = "10.0.0"
    "Microsoft.Extensions.Configuration" = "10.0.0"
    "Microsoft.Extensions.Configuration.Abstractions" = "10.0.0"
    "Microsoft.Extensions.Configuration.Binder" = "10.0.0"
    "Microsoft.Extensions.DependencyInjection" = "10.0.0"
    "Microsoft.Extensions.DependencyInjection.Abstractions" = "10.0.0"
    "Microsoft.Extensions.Http" = "10.0.0"
    "Microsoft.Data.SqlClient" = "5.2.2"
    
    # Npgsql
    "Npgsql.EntityFrameworkCore.PostgreSQL" = "10.0.0"
    
    # Third-party packages
    "AutoMapper.Extensions.Microsoft.DependencyInjection" = "13.0.1"
    "FluentValidation" = "11.11.0"
    "FluentValidation.AspNetCore" = "11.3.0"
    "MassTransit" = "8.3.4"
    "MassTransit.RabbitMQ" = "8.3.4"
    "MediatR" = "12.4.1"
    "Hangfire" = "1.8.17"
    "Hangfire.Core" = "1.8.17"
    "Hangfire.AspNetCore" = "1.8.17"
    "Hangfire.SqlServer" = "1.8.17"
    "Hangfire.PostgreSql" = "1.20.10"
    "StackExchange.Redis" = "2.8.16"
    "Serilog.AspNetCore" = "8.0.3"
    "Serilog.Enrichers.Process" = "3.0.0"
    "Serilog.Exceptions" = "8.4.0"
    "Serilog.Sinks.Elasticsearch" = "10.0.0"
    "Swashbuckle.AspNetCore" = "7.2.0"
    "Newtonsoft.Json" = "13.0.3"
    "Polly" = "8.5.0"
    "FirebaseAdmin" = "3.0.1"
    "Google.Apis.Auth" = "1.69.0"
    "JetBrains.Annotations" = "2024.3.0"
    "BouncyCastle.Cryptography" = "2.4.0"
    "prometheus-net" = "8.2.1"
    "prometheus-net.AspNetCore" = "8.2.1"
    "prometheus-net.AspNetCore.HealthChecks" = "8.2.1"
    "Minio" = "6.0.3"
    "AspNetCore.HealthChecks.Hangfire" = "8.0.2"
    "AspNetCore.HealthChecks.RabbitMQ" = "8.0.2"
    "AspNetCore.HealthChecks.SqlServer" = "8.0.2"
    "AspNetCore.HealthChecks.UI.Client" = "8.0.2"
    "Unchase.Swashbuckle.AspNetCore.Extensions" = "2.7.1"
    "Microsoft.VisualStudio.Azure.Containers.Tools.Targets" = "1.21.0"
    "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" = "5.1.0"
    "System.ServiceModel.Duplex" = "10.0.*"
    "System.ServiceModel.Federation" = "10.0.*"
    "System.ServiceModel.Http" = "10.0.*"
    "System.ServiceModel.NetTcp" = "10.0.*"
    "System.ServiceModel.Security" = "10.0.*"
}

# Get all project files
$projectFiles = Get-ChildItem -Filter "*.csproj" -Recurse

foreach ($project in $projectFiles) {
    Write-Host "`n  Updating packages in $($project.Name)..." -ForegroundColor Cyan
    
    # Read project file
    $xml = [xml](Get-Content $project.FullName)
    $modified = $false
    
    # Update package references
    foreach ($packageRef in $xml.Project.ItemGroup.PackageReference) {
        if ($packageRef.Include -and $packagesToUpdate.ContainsKey($packageRef.Include)) {
            $newVersion = $packagesToUpdate[$packageRef.Include]
            $currentVersion = $packageRef.Version
            
            if ($currentVersion -ne $newVersion) {
                Write-Host "    - $($packageRef.Include): $currentVersion -> $newVersion" -ForegroundColor Gray
                $packageRef.Version = $newVersion
                $modified = $true
            }
        }
    }
    
    # Save if modified
    if ($modified) {
        $xml.Save($project.FullName)
        Write-Host "    ✓ Saved" -ForegroundColor Green
    }
}

Pop-Location

Write-Host "`n✓ Package updates completed!" -ForegroundColor Green

# Step 3: Remove deprecated packages
Write-Host "`nStep 3: Checking for deprecated packages..." -ForegroundColor Yellow
Write-Host "  ! IdentityServer4.AccessTokenValidation is deprecated - manual review needed" -ForegroundColor Red
Write-Host "  ! Old Microsoft.AspNetCore.* packages (v2.2.0) should be removed or updated - manual review needed" -ForegroundColor Red

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Upgrade script completed!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "1. Review deprecated packages (IdentityServer4, old ASP.NET Core packages)" -ForegroundColor Gray
Write-Host "2. Run: dotnet restore" -ForegroundColor Gray
Write-Host "3. Run: dotnet build" -ForegroundColor Gray
Write-Host "4. Fix any compilation errors" -ForegroundColor Gray
Write-Host "5. Test the application" -ForegroundColor Gray
