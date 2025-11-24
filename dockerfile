# ===========================
# Stage 1: Build
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Set NuGet location for caching
ENV NUGET_PACKAGES=/src/nugetpackages

# Copy solution + dependency files (cache restore layer)
COPY *.sln .
COPY Directory.Packages.props .
COPY src/Api/Api.csproj src/Api/
COPY src/Core/Core.csproj src/Core/
COPY src/Library/Library.csproj src/Library/

# Restore dependencies (cached)
RUN dotnet restore

# Copy full source
COPY . .

# Build & publish
WORKDIR /src/src/Api
RUN dotnet publish -c Release -o /app/publish --no-restore


# ===========================
# Stage 2: Runtime
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create non-root runtime user (recommended)
RUN addgroup --system appgroup \
    && adduser --system --ingroup appgroup appuser

# Copy runtime build
COPY --from=build /app/publish .

# Create persistent app data dir
RUN mkdir -p /app/data \
    && chown -R appuser:appgroup /app/data \
    && chmod -R 755 /app/data

# Env variables (must match your docker-compose)
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:5000

# Expose API port
EXPOSE 5000

# Switch to non-root user
USER appuser

# Start application
ENTRYPOINT ["dotnet", "Api.dll"]

LABEL maintainer="nasim@gmail.com" \
      description="Rental API (Optimized)" \
      version="1.0"
