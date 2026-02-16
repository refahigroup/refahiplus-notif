# Auto-upgrade packages in csproj files
Write-Host "Auto-upgrading packages in .csproj files..." -ForegroundColor Cyan

# Package version mappings
$upgrades = @{
    # Microsoft packages to 10.0.3
    'Version="8\.0\.6".*Microsoft\.AspNetCore\.Authentication\.JwtBearer' = 'Version="10.0.3" Include="Microsoft.AspNetCore.Authentication.JwtBearer'
    'Version="8\.0\.6".*Microsoft\.AspNetCore\.Authentication\.OpenIdConnect' = 'Version="10.0.3" Include="Microsoft.AspNetCore.Authentication.OpenIdConnect'
    'Version="8\.0\.6".*Microsoft\.AspNetCore\.Identity\.EntityFrameworkCore' = 'Version="10.0.3" Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore'
    'Version="8\.0\.6".*Microsoft\.EntityFrameworkCore\.SqlServer' = 'Version="10.0.3" Include="Microsoft.EntityFrameworkCore.SqlServer'
    'Version="8\.0\.6".*Microsoft\.EntityFrameworkCore\.Tools' = 'Version="10.0.3" Include="Microsoft.EntityFrameworkCore.Tools'
    'Version="8\.0\.6".*Microsoft\.EntityFrameworkCore\.Design' = 'Version="10.0.3" Include="Microsoft.EntityFrameworkCore.Design'
    'Version="8\.0\.10".*Microsoft\.EntityFrameworkCore\.Relational' = 'Version="10.0.3" Include="Microsoft.EntityFrameworkCore.Relational'
    'Version="8\.0\.[0-9]+".*Microsoft\.EntityFrameworkCore"' = 'Version="10.0.3" Include="Microsoft.EntityFrameworkCore"'
    'Version="8\.0\.0".*Microsoft\.Extensions\.Caching\.Abstractions' = 'Version="10.0.3" Include="Microsoft.Extensions.Caching.Abstractions'
    'Version="8\.0\.1".*Microsoft\.Extensions\.Caching\.Memory' = 'Version="10.0.3" Include="Microsoft.Extensions.Caching.Memory'
    'Version="8\.0\.[0-9]+".*Microsoft\.Extensions\.Caching\.StackExchangeRedis' = 'Version="10.0.3" Include="Microsoft.Extensions.Caching.StackExchangeRedis'
    'Version="8\.0\.0".*Microsoft\.Extensions\.Configuration' = 'Version="10.0.3" Include="Microsoft.Extensions.Configuration'
    'Version="8\.0\.0".*Microsoft\.Extensions\.Configuration\.Abstractions' = 'Version="10.0.3" Include="Microsoft.Extensions.Configuration.Abstractions'
    'Version="8\.0\.[0-9]+".*Microsoft\.Extensions\.Configuration\.Binder' = 'Version="10.0.3" Include="Microsoft.Extensions.Configuration.Binder'
    'Version="8\.0\.0".*Microsoft\.Extensions\.DependencyInjection' = 'Version="10.0.3" Include="Microsoft.Extensions.DependencyInjection'
    'Version="8\.0\.[0-9]+".*Microsoft\.Extensions\.DependencyInjection\.Abstractions' = 'Version="10.0.3" Include="Microsoft.Extensions.DependencyInjection.Abstractions'
    'Version="8\.0\.0".*Microsoft\.Extensions\.Http' = 'Version="10.0.3" Include="Microsoft.Extensions.Http'
    
    # Npgsql
    'Version="8\.0\.10".*Npgsql\.EntityFrameworkCore\.PostgreSQL' = 'Version="10.0.0" Include="Npgsql.EntityFrameworkCore.PostgreSQL'
    
    # Third-party
    'Version="8\.2\.2".*MassTransit"' = 'Version="9.0.1" Include="MassTransit"'
    'Version="8\.2\.2".*MassTransit\.RabbitMQ' = 'Version="9.0.1" Include="MassTransit.RabbitMQ'
    'Version="12\.3\.0".*MediatR' = 'Version="14.0.0" Include="MediatR'
    'Version="1\.8\.14".*Hangfire"' = 'Version="1.8.23" Include="Hangfire"'
    'Version="1\.8\.14".*Hangfire\.Core' = 'Version="1.8.23" Include="Hangfire.Core'
    'Version="1\.8\.14".*Hangfire\.AspNetCore' = 'Version="1.8.23" Include="Hangfire.AspNetCore'
    'Version="1\.8\.14".*Hangfire\.SqlServer' = 'Version="1.8.23" Include="Hangfire.SqlServer'
    'Version="2\.7\.33".*StackExchange\.Redis' = 'Version="2.11.0" Include="StackExchange.Redis'
    'Version="8\.0\.1".*serilog\.aspnetcore' = 'Version="10.0.0" Include="serilog.aspnetcore'
    'Version="2\.0\.2".*Serilog\.Enrichers\.Process' = 'Version="3.0.0" Include="Serilog.Enrichers.Process'
    'Version="6\.6\.2".*Swashbuckle\.AspNetCore' = 'Version="10.1.3" Include="Swashbuckle.AspNetCore'
    'Version="13\.0\.3".*Newtonsoft\.Json' = 'Version="13.0.4" Include="Newtonsoft.Json'
    'Version="8\.4\.0".*Polly' = 'Version="8.6.5" Include="Polly'
    'Version="11\.9\.2".*FluentValidation' = 'Version="12.1.1" Include="FluentValidation'
    'Version="11\.3\.0".*FluentValidation\.AspNetCore' = 'Version="11.3.1" Include="FluentValidation.AspNetCore'
    'Version="12\.0\.1".*automapper' = 'Version="16.0.0" Include="automapper'
    'Version="12\.0\.1".*AutoMapper\.Extensions\.Microsoft\.DependencyInjection' = 'Version="13.0.3" Include="AutoMapper.Extensions.Microsoft.DependencyInjection'
    'Version="3\.0\.0".*FirebaseAdmin' = 'Version="3.4.0" Include="FirebaseAdmin'
    'Version="1\.68\.0".*Google\.Apis\.Auth' = 'Version="1.73.0" Include="Google.Apis.Auth'
    'Version="2023\.3\.0".*JetBrains\.Annotations' = 'Version="2025.2.4" Include="JetBrains.Annotations'
    'Version="2\.4\.0".*BouncyCastle\.Cryptography' = 'Version="2.6.2" Include="BouncyCastle.Cryptography'
    'Version="6\.0\.2".*Minio' = 'Version="6.0.3" Include="Minio'
    'Version="8\.0\.1".*AspNetCore\.HealthChecks\.Hangfire' = 'Version="9.0.0" Include="AspNetCore.HealthChecks.Hangfire'
    'Version="8\.0\.1".*AspNetCore\.HealthChecks\.RabbitMQ' = 'Version="9.0.0" Include="AspNetCore.HealthChecks.RabbitMQ'
    'Version="8\.0\.2".*AspNetCore\.HealthChecks\.SqlServer' = 'Version="9.0.0" Include="AspNetCore.HealthChecks.SqlServer'
    'Version="8\.0\.1".*AspNetCore\.HealthChecks\.UI\.Client' = 'Version="9.0.0" Include="AspNetCore.HealthChecks.UI.Client'
    'Version="2\.7\.1".*Unchase\.Swashbuckle\.AspNetCore\.Extensions' = 'Version="2.7.2" Include="Unchase.Swashbuckle.AspNetCore.Extensions'
    'Version="1\.20\.1".*Microsoft\.VisualStudio\.Azure\.Containers\.Tools\.Targets' = 'Version="1.23.0" Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets'
    'Version="8\.0\.0".*System\.ServiceModel\.Federation' = 'Version="10.0.652802" Include="System.ServiceModel.Federation'
    'Version="8\.0\.0".*System\.ServiceModel\.Http' = 'Version="10.0.652802" Include="System.ServiceModel.Http'
    'Version="8\.0\.0".*System\.ServiceModel\.NetTcp' = 'Version="10.0.652802" Include="System.ServiceModel.NetTcp'
    'Version="1\.0\.0".*NikSms\.Library\.NetCore' = 'Version="1.1.0" Include="NikSms.Library.NetCore'
}

$files = Get-ChildItem -Path "src" -Filter "*.csproj" -Recurse
$updatedCount = 0

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    $fileUpdated = $false
    
    foreach ($pattern in $upgrades.Keys) {
        if ($content -match $pattern) {
            $content = $content -replace $pattern, $upgrades[$pattern]
            $fileUpdated = $true
        }
    }
    
    if ($fileUpdated) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Updated: $($file.Name)" -ForegroundColor Green
        $updatedCount++
    }
}

Write-Host "`nTotal files updated: $updatedCount" -ForegroundColor Cyan
Write-Host "Next: Run 'dotnet restore' and 'dotnet build'" -ForegroundColor Yellow
