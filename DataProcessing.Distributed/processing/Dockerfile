# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 9464

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["processing/DataProcessing.Processing.Api/DataProcessing.Processing.Api.csproj", "processing/DataProcessing.Processing.Api/"]
COPY ["processing/DataProcessing.Processing.Application/DataProcessing.Processing.Application.csproj", "processing/DataProcessing.Processing.Application/"]
COPY ["processing/DataProcessing.Processing.Domain/DataProcessing.Processing.Domain.csproj", "processing/DataProcessing.Processing.Domain/"]
COPY ["processing/DataProcessing.Processing.Infrastructure/DataProcessing.Processing.Infrastructure.csproj", "processing/DataProcessing.Processing.Infrastructure/"]
COPY ["DataProcessing.Monitoring/DataProcessing.Monitoring/DataProcessing.Monitoring.csproj", "DataProcessing.Monitoring/DataProcessing.Monitoring/"]

# Restore dependencies
RUN dotnet restore "processing/DataProcessing.Processing.Api/DataProcessing.Processing.Api.csproj"

# Copy all sources
COPY processing/ processing/
COPY DataProcessing.Monitoring/ DataProcessing.Monitoring/

WORKDIR "/src/processing/DataProcessing.Processing.Api"

# Build and publish
RUN dotnet build "DataProcessing.Processing.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataProcessing.Processing.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Processing.Api.dll"]