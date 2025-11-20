# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Set NuGet packages location
ENV NUGET_PACKAGES=/app/nugetpackages

# Copy project files for caching
COPY *.sln ./
COPY Directory.Packages.props ./
COPY src/Api/Api.csproj ./src/Api/
COPY src/Core/Core.csproj ./src/Core/
COPY src/Library/Library.csproj ./src/Library/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR /app/src/Api
RUN dotnet build --configuration Release --no-restore
RUN dotnet publish --configuration Release --no-build --output /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Create non-root user
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

WORKDIR /app
COPY --from=build /app/publish ./

# Data directory with permissions
RUN mkdir -p ./data && chown -R appuser:appgroup ./data && chmod -R 755 ./data

# Environment variables
ENV ASPNETCORE_URLS=http://+:5000 \
    ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 5000

# Switch to non-root user
USER appuser

# Entry point
ENTRYPOINT ["dotnet", "Api.dll"]

# Metadata
LABEL maintainer="nasim@gmail.com" \
      description="Rental API" \
      version="1.0"
