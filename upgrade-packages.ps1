# Script to upgrade NuGet packages to .NET 10 compatible versions
Write-Host "Upgrading NuGet packages to .NET 10 compatible versions..." -ForegroundColor Cyan

$sln = "Refahi.Notif.sln"

# Function to upgrade package
function Upgrade-Package {
    param($package, $version)
    Write-Host "  Upgrading $package to $version..." -ForegroundColor Gray
    dotnet add package $package --version $version 2>&1 | Out-Null
}

# Upgrade Microsoft.* packages to 10.0.3
Write-Host "`n1. Upgrading Microsoft packages to 10.0.3..." -ForegroundColor Yellow
$microsoftPackages = @(
    "Microsoft.AspNetCore.Authentication.JwtBearer"
    "Microsoft.AspNetCore.Authentication.OpenIdConnect"
    "Microsoft.AspNetCore.Identity.EntityFrameworkCore"
    "Microsoft.EntityFrameworkCore"
    "Microsoft.EntityFrameworkCore.SqlServer"
    "Microsoft.EntityFrameworkCore.Tools"
    "Microsoft.EntityFrameworkCore.Design"
    "Microsoft.EntityFrameworkCore.Relational"
    "Microsoft.Extensions.Caching.Abstractions"
    "Microsoft.Extensions.Caching.Memory"
    "Microsoft.Extensions.Caching.StackExchangeRedis"
    "Microsoft.Extensions.Configuration"
    "Microsoft.Extensions.Configuration.Abstractions"
    "Microsoft.Extensions.Configuration.Binder"
    "Microsoft.Extensions.DependencyInjection"
    "Microsoft.Extensions.DependencyInjection.Abstractions"
    "Microsoft.Extensions.Http"
)

foreach ($pkg in $microsoftPackages) {
    dotnet list $sln package | Select-String $pkg | ForEach-Object {
        $project = ($_ -split "`n")[0]
        if ($project) {
            Write-Host "  $pkg -> 10.0.3" -ForegroundColor Gray
        }
    }
}

# Upgrade third-party packages
Write-Host "`n2. Upgrading third-party packages..." -ForegroundColor Yellow

$upgrades = @{
    "MassTransit" = "9.0.1"
    "MassTransit.RabbitMQ" = "9.0.1"
    "MediatR" = "14.0.0"
    "Hangfire" = "1.8.23"
    "Hangfire.Core" = "1.8.23"
    "Hangfire.AspNetCore" = "1.8.23"
    "Hangfire.SqlServer" = "1.8.23"
    "Hangfire.PostgreSql" = "1.20.10"
    "Npgsql.EntityFrameworkCore.PostgreSQL" = "10.0.0"
    "StackExchange.Redis" = "2.11.0"
    "Serilog.AspNetCore" = "10.0.0"
    "Serilog.Enrichers.Process" = "3.0.0"
    "Serilog.Exceptions" = "8.4.0"
    "Serilog.Sinks.Elasticsearch" = "10.0.0"
    "Swashbuckle.AspNetCore" = "10.1.3"
    "Newtonsoft.Json" = "13.0.4"
    "Polly" = "8.6.5"
    "FluentValidation" = "12.1.1"
    "FluentValidation.AspNetCore" = "11.3.1"
    "AutoMapper.Extensions.Microsoft.DependencyInjection" = "13.0.3"
    "FirebaseAdmin" = "3.4.0"
    "Google.Apis.Auth" = "1.73.0"
    "JetBrains.Annotations" = "2025.2.4"
    "BouncyCastle.Cryptography" = "2.6.2"
    "Minio" = "6.0.3"
    "prometheus-net" = "8.2.1"
    "prometheus-net.AspNetCore" = "8.2.1"
    "prometheus-net.AspNetCore.HealthChecks" = "8.2.1"
    "AspNetCore.HealthChecks.Hangfire" = "9.0.0"
    "AspNetCore.HealthChecks.RabbitMQ" = "9.0.0"
    "AspNetCore.HealthChecks.SqlServer" = "9.0.0"
    "AspNetCore.HealthChecks.UI.Client" = "9.0.0"
    "Unchase.Swashbuckle.AspNetCore.Extensions" = "2.7.2"
    "Microsoft.VisualStudio.Azure.Containers.Tools.Targets" = "1.23.0"
    "System.ServiceModel.Federation" = "10.0.652802"
    "System.ServiceModel.Http" = "10.0.652802"
    "System.ServiceModel.NetTcp" = "10.0.652802"
    "NikSms.Library.NetCore" = "1.1.0"
}

Write-Host "`nNote: Manual upgrade required using dotnet add package commands" -ForegroundColor Yellow
Write-Host "Run the following commands to upgrade all projects:" -ForegroundColor Cyan
Write-Host "cd src" -ForegroundColor Gray
Write-Host "dotnet add package <PackageName> --version <Version>" -ForegroundColor Gray

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Package list prepared!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
