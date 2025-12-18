# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the csproj files first, keeping the folder structure
COPY ["ingestion/DataProcessing.Ingestion.Api/DataProcessing.Ingestion.Api.csproj", "ingestion/DataProcessing.Ingestion.Api/"]
COPY ["ingestion/DataProcessing.Ingestion.Application/DataProcessing.Ingestion.Application.csproj", "ingestion/DataProcessing.Ingestion.Application/"]
COPY ["ingestion/DataProcessing.Ingestion.Domain/DataProcessing.Ingestion.Domain.csproj", "ingestion/DataProcessing.Ingestion.Domain/"]
COPY ["ingestion/DataProcessing.Ingestion.Infrastructure/DataProcessing.Ingestion.Infrastructure.csproj", "ingestion/DataProcessing.Ingestion.Infrastructure/"]
COPY ["DataProcessing.Monitoring/DataProcessing.Monitoring/DataProcessing.Monitoring.csproj", "DataProcessing.Monitoring/DataProcessing.Monitoring/"]

# Restore dependencies
RUN dotnet restore "ingestion/DataProcessing.Ingestion.Api/DataProcessing.Ingestion.Api.csproj"

# Copy all source files (preserve the folder structure)
COPY ingestion/ ingestion/
COPY DataProcessing.Monitoring/ DataProcessing.Monitoring/

WORKDIR "/src/ingestion/DataProcessing.Ingestion.Api"

# Build and publish
RUN dotnet build "DataProcessing.Ingestion.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataProcessing.Ingestion.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Ingestion.Api.dll"]