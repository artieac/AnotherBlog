# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Install Node.js for LESS compilation
RUN apt-get update && apt-get install -y nodejs npm && rm -rf /var/lib/apt/lists/*

# Copy solution and project files first for better layer caching
COPY AnotherBlog/src/AlwaysMoveForward.EntityFramework.sln ./AnotherBlog/src/
COPY AnotherBlog/src/Web/AlwaysMoveForward.AnotherBlog.Web.csproj ./AnotherBlog/src/Web/
COPY AnotherBlog/src/BusinessLayer/AlwaysMoveForward.AnotherBlog.BusinessLayer.csproj ./AnotherBlog/src/BusinessLayer/
COPY AnotherBlog/src/Common/AlwaysMoveForward.AnotherBlog.Common.csproj ./AnotherBlog/src/Common/
COPY AnotherBlog/src/DataLayer.EntityFramework/AlwaysMoveForward.AnotherBlog.DataLayer.EntityFramework.csproj ./AnotherBlog/src/DataLayer.EntityFramework/
COPY AlwaysMoveForward/src/Common/AlwaysMoveForward.Common/AlwaysMoveForward.Common.csproj ./AlwaysMoveForward/src/Common/AlwaysMoveForward.Common/
COPY AlwaysMoveForward/src/Common/DataLayer.EntityFramework/AlwaysMoveForward.Common.DataLayer.EntityFramework.csproj ./AlwaysMoveForward/src/Common/DataLayer.EntityFramework/
COPY AnotherBlog/src/UnitTest/AlwaysMoveForward.AnotherBlog.UnitTest.csproj ./AnotherBlog/src/UnitTest/

# Restore NuGet packages
RUN dotnet restore ./AnotherBlog/src/AlwaysMoveForward.EntityFramework.sln

# Copy all source code
COPY AnotherBlog/ ./AnotherBlog/
COPY AlwaysMoveForward/ ./AlwaysMoveForward/

# Install npm dependencies and build LESS files
WORKDIR /src/AnotherBlog/src/Web
RUN npm install && npm run build:less

# Build the application
WORKDIR /src
RUN dotnet publish ./AnotherBlog/src/Web/AlwaysMoveForward.AnotherBlog.Web.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser

# Copy published application
COPY --from=build /app/publish .

# Set ownership to non-root user
RUN chown -R appuser:appuser /app

USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "AlwaysMoveForward.AnotherBlog.Web.dll"]
