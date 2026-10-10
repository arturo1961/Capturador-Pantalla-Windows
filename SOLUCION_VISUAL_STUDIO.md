# Solución al error «Visual Studio no admite .NET 10.0 de destino»

Esta edición de CapturaLibre DNET 2.4 beta utiliza **C# / Windows Forms / .NET 8**.
No necesitas Visual Studio 2026 (18.0) ni cambiar el lenguaje del proyecto.

## Pasos para abrirlo en Visual Studio 2022

1. Extrae completamente el ZIP a una carpeta (no abras el proyecto dentro del ZIP).
2. Abre `CapturaLibre/CapturaLibre.csproj` desde Visual Studio.
3. Comprueba que tienes **Visual Studio 2022 17.8 o posterior** y la carga de trabajo **Desarrollo de escritorio con .NET**. Si te pide el SDK de .NET 8, instálalo con Visual Studio Installer o desde https://dotnet.microsoft.com/download/dotnet/8.0.
4. En el menú **Compilar > Compilar solución** (o `Ctrl + Mayús + B`).
5. Para arrancar desde Visual Studio, pulsa `F5`.
6. Si quieres crear un EXE independiente de .NET, ejecuta `Compilar_EXE.cmd` desde la carpeta raíz. Lo encontrarás en `publicar/CapturaLibre_DNET.exe` si la publicación finaliza correctamente.

## Si vuelve a aparecer un error

- Si te pide un framework, comprueba que estás usando **esta copia nueva** y que el archivo `.csproj` dice `<TargetFramework>net8.0-windows</TargetFramework>`.
- Si tienes Visual Studio 2022 anterior a **17.8**, actualízalo antes de compilar .NET 8.
- Si tienes algún error distinto, copia el **texto completo del primer error** (y, si puedes, el número de versión de Visual Studio) para corregirlo.

## Estado

El paquete de GitHub incluye el código fuente y se entrega además un ZIP de Windows con el EXE autónomo ya compilado. La edición sigue siendo beta hasta completar toda la lista de pruebas en varios equipos.
