# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the csproj files first (keep folder structure)
COPY ["DataProcessing.Api/DataProcessing.Api.csproj", "DataProcessing.Api/"]
COPY ["DataProcessing.Application/DataProcessing.Application.csproj", "DataProcessing.Application/"]
COPY ["DataProcessing.Domain/DataProcessing.Domain.csproj", "DataProcessing.Domain/"]
COPY ["DataProcessing.Infrastructure/DataProcessing.Infrastructure.csproj", "DataProcessing.Infrastructure/"]
COPY ["DataProcessing.Monitoring/DataProcessing.Monitoring/DataProcessing.Monitoring.csproj", "DataProcessing.Monitoring/DataProcessing.Monitoring/"]

# Restore dependencies
RUN dotnet restore "DataProcessing.Api/DataProcessing.Api.csproj"

# Copy all source files (preserve structure)
COPY DataProcessing.Api/ DataProcessing.Api/
COPY DataProcessing.Application/ DataProcessing.Application/
COPY DataProcessing.Domain/ DataProcessing.Domain/
COPY DataProcessing.Infrastructure/ DataProcessing.Infrastructure/
COPY DataProcessing.Monitoring/ DataProcessing.Monitoring/

# Build
WORKDIR "/src/DataProcessing.Api"
RUN dotnet build "DataProcessing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DataProcessing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Api.dll"]