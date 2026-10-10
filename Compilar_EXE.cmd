@echo off
setlocal
cd /d "%~dp0"
where dotnet >nul 2>nul
if errorlevel 1 (
    echo No se ha encontrado el SDK de .NET 8.
    echo Instala el SDK de .NET 8 desde microsoft.com/dotnet.
    pause
    exit /b 1
)
echo Compilando CapturaLibre para Windows x64...
dotnet publish "CapturaLibre\CapturaLibre.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=none -p:DebugSymbols=false -o "publicar"
if errorlevel 1 (
    echo La compilacion ha fallado. Copia los errores para revisarlos.
    pause
    exit /b 1
)
echo EXE preparado en: "%~dp0publicar\CapturaLibre_DNET.exe"
pause
