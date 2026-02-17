# Dockerfile for Refahi.Notif.EndPoint.Api
# Uses pre-built artifacts from CI pipeline for faster builds

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "Refahi.Notif.EndPoint.Api.dll"]