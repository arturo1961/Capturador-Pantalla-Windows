@echo off
chcp 65001 >nul
cd /d "%~dp0"
where dotnet >nul 2>&1
if errorlevel 1 (
    echo Necesitas instalar el SDK de .NET 8 para ejecutar este CODIGO FUENTE.
    echo Alternativamente, descarga el EXE generado por GitHub Actions.
    pause
    exit /b 1
)
dotnet run --project "CapturaLibre\CapturaLibre.csproj" -c Release
if errorlevel 1 pause
