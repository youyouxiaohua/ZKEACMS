FROM mcr.microsoft.com/dotnet/aspnet:5.0
ARG source
WORKDIR /zkeacms
EXPOSE 80
COPY ${source:-bin/Release/PublishOutput} .
ENTRYPOINT ["dotnet", "ZKEACMS.WebHost.dll"]
