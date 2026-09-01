FROM registry.refahi.xyz/platform/ci-images/aspnet:10.0

WORKDIR /app

EXPOSE 8080
EXPOSE 80
EXPOSE 443

COPY ./publish/notif .

ENTRYPOINT ["dotnet", "Refahi.Notif.EndPoint.Api.dll"]