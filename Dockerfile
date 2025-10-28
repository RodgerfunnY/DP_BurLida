# Multi-stage Dockerfile for ASP.NET Core 9.0 (Cloud Run friendly)
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY DP_BurLida.csproj ./
RUN dotnet restore "DP_BurLida.csproj"

# Copy the rest of the source
COPY . .

# Publish (framework-dependent)
RUN dotnet publish "DP_BurLida.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Cloud Run uses port 8080 by default
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Copy published output
COPY --from=build /app/publish .

# Expose for local runs
EXPOSE 8080

ENTRYPOINT ["dotnet", "DP_BurLida.dll"]
