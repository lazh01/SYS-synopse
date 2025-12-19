# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004
EXPOSE 5005

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the csproj files first, keeping the folder structure
COPY ["dashboard/DataProcessing.Dashboard.Api/DataProcessing.Dashboard.Api.csproj", "dashboard/DataProcessing.Dashboard.Api/"]
COPY ["dashboard/DataProcessing.Dashboard.Application/DataProcessing.Dashboard.Application.csproj", "dashboard/DataProcessing.Dashboard.Application/"]
COPY ["dashboard/DataProcessing.Dashboard.Domain/DataProcessing.Dashboard.Domain.csproj", "dashboard/DataProcessing.Dashboard.Domain/"]
COPY ["dashboard/DataProcessing.Dashboard.Infrastructure/DataProcessing.Dashboard.Infrastructure.csproj", "dashboard/DataProcessing.Dashboard.Infrastructure/"]
COPY ["DataProcessing.Monitoring/DataProcessing.Monitoring/DataProcessing.Monitoring.csproj", "DataProcessing.Monitoring/DataProcessing.Monitoring/"]

# Restore dependencies
RUN dotnet restore "dashboard/DataProcessing.Dashboard.Api/DataProcessing.Dashboard.Api.csproj"

# Copy all source files (preserve the folder structure)
COPY dashboard/ dashboard/
COPY DataProcessing.Monitoring/ DataProcessing.Monitoring/

WORKDIR "/src/dashboard/DataProcessing.Dashboard.Api"

# Build and publish
RUN dotnet build "DataProcessing.Dashboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataProcessing.Dashboard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Dashboard.Api.dll"]