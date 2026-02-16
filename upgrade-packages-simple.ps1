# Simple package upgrade script
Write-Host "Upgrading packages..." -ForegroundColor Cyan

$files = Get-ChildItem -Path "src" -Filter "*.csproj" -Recurse

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $modified = $false
    
    # Microsoft packages to 10.0.3
    if ($content -replace 'Microsoft\.AspNetCore\.Authentication\.JwtBearer" Version="8\.0\.[0-9]+"', 'Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.AspNetCore\.Authentication\.JwtBearer" Version="8\.0\.[0-9]+"', 'Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.AspNetCore\.Authentication\.OpenIdConnect" Version="8\.0\.[0-9]+"', 'Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.AspNetCore\.Authentication\.OpenIdConnect" Version="8\.0\.[0-9]+"', 'Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.AspNetCore\.Identity\.EntityFrameworkCore" Version="8\.0\.[0-9]+"', 'Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.AspNetCore\.Identity\.EntityFrameworkCore" Version="8\.0\.[0-9]+"', 'Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.EntityFrameworkCore\.SqlServer" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.EntityFrameworkCore\.SqlServer" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.EntityFrameworkCore\.Tools" Version="8\.0\.([0-9]+)"', 'Microsoft.EntityFrameworkCore.Tools" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.EntityFrameworkCore\.Tools" Version="8\.0\.([0-9]+)"', 'Microsoft.EntityFrameworkCore.Tools" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.EntityFrameworkCore\.Design" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore.Design" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.EntityFrameworkCore\.Design" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore.Design" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.EntityFrameworkCore\.Relational" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore.Relational" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.EntityFrameworkCore\.Relational" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore.Relational" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.EntityFrameworkCore" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.EntityFrameworkCore" Version="8\.0\.[0-9]+"', 'Microsoft.EntityFrameworkCore" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Caching\.Abstractions" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Caching.Abstractions" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Caching\.Abstractions" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Caching.Abstractions" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Caching\.Memory" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Caching.Memory" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Caching\.Memory" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Caching.Memory" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Caching\.StackExchangeRedis" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Caching.StackExchangeRedis" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Caching\.StackExchangeRedis" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Caching.StackExchangeRedis" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Configuration" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Configuration" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Configuration" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Configuration" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Configuration\.Abstractions" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Configuration.Abstractions" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Configuration\.Abstractions" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Configuration.Abstractions" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Configuration\.Binder" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Configuration.Binder" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Configuration\.Binder" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Configuration.Binder" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.DependencyInjection" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.DependencyInjection" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.DependencyInjection" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.DependencyInjection" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.DependencyInjection\.Abstractions" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.DependencyInjection\.Abstractions" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.3"'; $modified = $true }
    if ($content -replace 'Microsoft\.Extensions\.Http" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Http" Version="10.0.3"' -ne $content) { $content = $content -replace 'Microsoft\.Extensions\.Http" Version="8\.0\.[0-9]+"', 'Microsoft.Extensions.Http" Version="10.0.3"'; $modified = $true }
    
    # Npgsql
    if ($content -replace 'Npgsql\.EntityFrameworkCore\.PostgreSQL" Version="8\.0\.[0-9]+"', 'Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0"' -ne $content) { $content = $content -replace 'Npgsql\.EntityFrameworkCore\.PostgreSQL" Version="8\.0\.[0-9]+"', 'Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0"'; $modified = $true }
    
    # Third-party
    $content = $content -replace 'MassTransit" Version="8\.2\.2"', 'MassTransit" Version="9.0.1"'
    $content = $content -replace 'MassTransit\.RabbitMQ" Version="8\.2\.2"', 'MassTransit.RabbitMQ" Version="9.0.1"'
    $content = $content -replace 'MediatR" Version="12\.3\.0"', 'MediatR" Version="14.0.0"'
    $content = $content -replace 'Hangfire" Version="1\.8\.14"', 'Hangfire" Version="1.8.23"'
    $content = $content -replace 'Hangfire\.Core" Version="1\.8\.14"', 'Hangfire.Core" Version="1.8.23"'
    $content = $content -replace 'Hangfire\.AspNetCore" Version="1\.8\.14"', 'Hangfire.AspNetCore" Version="1.8.23"'
    $content = $content -replace 'Hangfire\.SqlServer" Version="1\.8\.14"', 'Hangfire.SqlServer" Version="1.8.23"'
    $content = $content -replace 'StackExchange\.Redis" Version="2\.7\.33"', 'StackExchange.Redis" Version="2.11.0"'
    $content = $content -replace 'serilog\.aspnetcore" Version="8\.0\.1"', 'serilog.aspnetcore" Version="10.0.0"'
    $content = $content -replace 'Serilog\.Enrichers\.Process" Version="2\.0\.2"', 'Serilog.Enrichers.Process" Version="3.0.0"'
    $content = $content -replace 'Swashbuckle\.AspNetCore" Version="6\.6\.2"', 'Swashbuckle.AspNetCore" Version="10.1.3"'
    $content = $content -replace 'Newtonsoft\.Json" Version="13\.0\.3"', 'Newtonsoft.Json" Version="13.0.4"'
    $content = $content -replace 'Polly" Version="8\.4\.0"', 'Polly" Version="8.6.5"'
    $content = $content -replace 'FluentValidation" Version="11\.9\.2"', 'FluentValidation" Version="12.1.1"'
    $content = $content -replace 'FluentValidation\.AspNetCore" Version="11\.3\.0"', 'FluentValidation.AspNetCore" Version="11.3.1"'
    $content = $content -replace 'automapper" Version="12\.0\.1"', 'automapper" Version="16.0.0"'
    $content = $content -replace 'AutoMapper\.Extensions\.Microsoft\.DependencyInjection" Version="12\.0\.1"', 'AutoMapper.Extensions.Microsoft.DependencyInjection" Version="13.0.3"'
    $content = $content -replace 'FirebaseAdmin" Version="3\.0\.0"', 'FirebaseAdmin" Version="3.4.0"'
    $content = $content -replace 'Google\.Apis\.Auth" Version="1\.68\.0"', 'Google.Apis.Auth" Version="1.73.0"'
    $content = $content -replace 'JetBrains\.Annotations" Version="2023\.3\.0"', 'JetBrains.Annotations" Version="2025.2.4"'
    $content = $content -replace 'BouncyCastle\.Cryptography" Version="2\.4\.0"', 'BouncyCastle.Cryptography" Version="2.6.2"'
    $content = $content -replace 'Minio" Version="6\.0\.2"', 'Minio" Version="6.0.3"'
    $content = $content -replace 'AspNetCore\.HealthChecks\.Hangfire" Version="8\.0\.1"', 'AspNetCore.HealthChecks.Hangfire" Version="9.0.0"'
    $content = $content -replace 'AspNetCore\.HealthChecks\.RabbitMQ" Version="8\.0\.1"', 'AspNetCore.HealthChecks.RabbitMQ" Version="9.0.0"'
    $content = $content -replace 'AspNetCore\.HealthChecks\.SqlServer" Version="8\.0\.2"', 'AspNetCore.HealthChecks.SqlServer" Version="9.0.0"'
    $content = $content -replace 'AspNetCore\.HealthChecks\.UI\.Client" Version="8\.0\.1"', 'AspNetCore.HealthChecks.UI.Client" Version="9.0.0"'
    $content = $content -replace 'Unchase\.Swashbuckle\.AspNetCore\.Extensions" Version="2\.7\.1"', 'Unchase.Swashbuckle.AspNetCore.Extensions" Version="2.7.2"'
    $content = $content -replace 'Microsoft\.VisualStudio\.Azure\.Containers\.Tools\.Targets" Version="1\.20\.1"', 'Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.23.0"'
    $content = $content -replace 'System\.ServiceModel\.Federation" Version="8\.0\.0"', 'System.ServiceModel.Federation" Version="10.0.652802"'
    $content = $content -replace 'System\.ServiceModel\.Http" Version="8\.0\.0"', 'System.ServiceModel.Http" Version="10.0.652802"'
    $content = $content -replace 'System\.ServiceModel\.NetTcp" Version="8\.0\.0"', 'System.ServiceModel.NetTcp" Version="10.0.652802"'
    $content = $content -replace 'NikSms\.Library\.NetCore" Version="1\.0\.0"', 'NikSms.Library.NetCore" Version="1.1.0"'
    
    Set-Content -Path $file.FullName -Value $content -NoNewline
    Write-Host "Processed: $($file.Name)" -ForegroundColor Gray
}

Write-Host "`nDone! Run 'dotnet restore' next." -ForegroundColor Green
