# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004
EXPOSE 5005

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["DataProcessing.Dashboard.Api/DataProcessing.Dashboard.Api.csproj", "DataProcessing.Dashboard.Api/"]
COPY ["DataProcessing.Dashboard.Application/DataProcessing.Dashboard.Application.csproj", "DataProcessing.Dashboard.Application/"]
COPY ["DataProcessing.Dashboard.Domain/DataProcessing.Dashboard.Domain.csproj", "DataProcessing.Dashboard.Domain/"]
COPY ["DataProcessing.Dashboard.Infrastructure/DataProcessing.Dashboard.Infrastructure.csproj", "DataProcessing.Dashboard.Infrastructure/"]
RUN dotnet restore "DataProcessing.Dashboard.Api/DataProcessing.Dashboard.Api.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/DataProcessing.Dashboard.Api"

# Build and publish
RUN dotnet build "DataProcessing.Dashboard.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "DataProcessing.Dashboard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Dashboard.Api.dll"]