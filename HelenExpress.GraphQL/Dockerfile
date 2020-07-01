FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["HelenExpress.GraphQL/HelenExpress.GraphQL.csproj", "HelenExpress.GraphQL/"]
COPY ["HelenExpress.Data/HelenExpress.Data.csproj", "HelenExpress.Data/"]
COPY ["BackgroundTaskQueueNet/BackgroundTaskQueueNet.csproj", "BackgroundTaskQueueNet/"]
COPY ["AuthService.Firebase/AuthService.Firebase.csproj", "AuthService.Firebase/"]
RUN dotnet restore "HelenExpress.GraphQL/HelenExpress.GraphQL.csproj"
COPY . .
WORKDIR "/src/HelenExpress.GraphQL"
RUN dotnet build "HelenExpress.GraphQL.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HelenExpress.GraphQL.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HelenExpress.GraphQL.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet AR.ProjectService.GraphQL.dll