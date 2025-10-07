FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProjectPlanner/ProjectPlanner.Client/ProjectPlanner.Client.csproj", "ProjectPlanner/ProjectPlanner.Client/"]
RUN dotnet restore "ProjectPlanner/ProjectPlanner.Client/ProjectPlanner.Client.csproj"
COPY . .
WORKDIR "/src/ProjectPlanner/ProjectPlanner.Client"
RUN dotnet build "./ProjectPlanner.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectPlanner.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectPlanner.Client.dll"]
