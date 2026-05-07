FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/Tixora.Bootstrapper/Tixora.Bootstrapper.csproj", "src/API/Tixora.Bootstrapper/"]
COPY ["src/Modules/Events.Api/Events.Api.csproj", "src/Modules/Events.Api/"]
RUN dotnet restore "src/API/Tixora.Bootstrapper/Tixora.Bootstrapper.csproj"
COPY . .
WORKDIR "/src/src/API/Tixora.Bootstrapper"
RUN dotnet build "./Tixora.Bootstrapper.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tixora.Bootstrapper.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tixora.Bootstrapper.dll"]
