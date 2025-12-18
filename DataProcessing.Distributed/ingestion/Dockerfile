# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["DataProcessing.Ingestion.Api/DataProcessing.Ingestion.Api.csproj", "DataProcessing.Ingestion.Api/"]
COPY ["DataProcessing.Ingestion.Application/DataProcessing.Ingestion.Application.csproj", "DataProcessing.Ingestion.Application/"]
COPY ["DataProcessing.Ingestion.Domain/DataProcessing.Ingestion.Domain.csproj", "DataProcessing.Ingestion.Domain/"]
COPY ["DataProcessing.Ingestion.Infrastructure/DataProcessing.Ingestion.Infrastructure.csproj", "DataProcessing.Ingestion.Infrastructure/"]
RUN dotnet restore "DataProcessing.Ingestion.Api/DataProcessing.Ingestion.Api.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/DataProcessing.Ingestion.Api"

# Build and publish
RUN dotnet build "DataProcessing.Ingestion.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "DataProcessing.Ingestion.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Ingestion.Api.dll"]