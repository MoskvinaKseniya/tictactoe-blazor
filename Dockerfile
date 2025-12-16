FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tictactoe blazor.csproj", "."]
RUN dotnet restore "./tictactoe blazor.csproj"
COPY . .
RUN dotnet build "./tictactoe blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./tictactoe blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "tictactoe_blazor.dll"]
