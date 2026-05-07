@echo off
SETLOCAL

:: Configuration
:: Get the directory where the script is located (D:\personal\sourcecode\AnotherBlog)
SET CURRENT_DIR=%~dp0
:: The project root is the parent directory (D:\personal\sourcecode) to include sibling dependencies
SET PROJECT_ROOT=%CURRENT_DIR%..
SET DOCKER_COMPOSE_FILE=%CURRENT_DIR%src\docker-compose.yaml
SET IMAGE_NAME=artieac/anotherblog

echo ==========================================================
echo  Rebuilding and Restarting AnotherBlog
echo ==========================================================

:: Change to the project root directory
echo [1/3] Navigating to project root: %PROJECT_ROOT%
cd /d "%PROJECT_ROOT%"
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to navigate to project root.
    pause
    exit /b %ERRORLEVEL%
)

:: Build the Docker image
echo [2/3] Building Docker image: %IMAGE_NAME%
:: The build context MUST be the parent directory of AnotherBlog to include AlwaysMoveForward
docker build -t %IMAGE_NAME% -f AnotherBlog/src/Dockerfile .
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Docker build failed.
    pause
    exit /b %ERRORLEVEL%
)

:: Restart the container using Docker Compose
echo [3/3] Restarting container...
:: Use the compose file which now has container_name: anotherblog and port 8080:8080
docker compose -f "%DOCKER_COMPOSE_FILE%" down --remove-orphans
docker compose -f "%DOCKER_COMPOSE_FILE%" up -d
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Docker compose failed.
    pause
    exit /b %ERRORLEVEL%
)

echo ==========================================================
echo  Successfully rebuilt and restarted AnotherBlog!
echo  Container: anotherblog
echo  Ports: 8080:8080
echo ==========================================================
pause
ENDLOCAL
