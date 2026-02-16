#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EndPoint/Refahi.Notif.EndPoint.PushProxy/Refahi.Notif.EndPoint.PushProxy.csproj", "EndPoint/Refahi.Notif.EndPoint.PushProxy/"]
COPY ["Infrastructure/Refahi.Notif.Infrastructure.Messaging/Refahi.Notif.Infrastructure.Messaging.PushNotification.csproj", "Infrastructure/Refahi.Notif.Infrastructure.Messaging/"]
COPY ["Domain/Refahi.Notif.Domain.Contract/Refahi.Notif.Domain.Contract.csproj", "Domain/Refahi.Notif.Domain.Contract/"]
COPY ["Domain/Refahi.Notif.Domain.Core/Refahi.Notif.Domain.Core.csproj", "Domain/Refahi.Notif.Domain.Core/"]
COPY ["Common/Refahi.Notif.Messages/Refahi.Notif.Messages.csproj", "00_Common/Refahi.Notif.Messages/"]
RUN dotnet restore "EndPoint/Refahi.Notif.EndPoint.PushProxy/Refahi.Notif.EndPoint.PushProxy.csproj"
COPY . .
WORKDIR "/src/EndPoint/Refahi.Notif.EndPoint.PushProxy"
RUN dotnet build "Refahi.Notif.EndPoint.PushProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Refahi.Notif.EndPoint.PushProxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Refahi.Notif.EndPoint.PushProxy.dll"]