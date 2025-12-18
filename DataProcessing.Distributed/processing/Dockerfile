# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["DataProcessing.Processing.Api/DataProcessing.Processing.Api.csproj", "DataProcessing.Processing.Api/"]
COPY ["DataProcessing.Processing.Application/DataProcessing.Processing.Application.csproj", "DataProcessing.Processing.Application/"]
COPY ["DataProcessing.Processing.Domain/DataProcessing.Processing.Domain.csproj", "DataProcessing.Processing.Domain/"]
COPY ["DataProcessing.Processing.Infrastructure/DataProcessing.Processing.Infrastructure.csproj", "DataProcessing.Processing.Infrastructure/"]
RUN dotnet restore "DataProcessing.Processing.Api/DataProcessing.Processing.Api.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/DataProcessing.Processing.Api"

# Build and publish
RUN dotnet build "DataProcessing.Processing.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "DataProcessing.Processing.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProcessing.Processing.Api.dll"]