# 📸 CapturaLibre DNET 2.5.1 — C# / .NET 8

CapturaLibre es un capturador y editor de pantalla **gratis, multilingüe y de código abierto**, para Windows 10 y 11. El proyecto funciona **localmente**: no tiene publicidad, telemetría ni subida automática de capturas. Licencia [MIT](LICENSE).

> **Versión estable 2.5.1:** añade el ajuste automático al contenido útil de DNET y conserva la interfaz multilingüe de la versión 2.5.0.

**Compatibilidad:** esta variante apunta a **.NET 8**, compatible con **Visual Studio 2022 17.8 o posterior**, siempre que tengas instalado el SDK de .NET 8 y el componente de desarrollo de escritorio. Lee [SOLUCION_VISUAL_STUDIO.md](SOLUCION_VISUAL_STUDIO.md) si aparece el error sobre .NET 10.

## Idiomas disponibles

El mismo ejecutable permite cambiar el idioma desde el selector situado en la esquina superior derecha. La elección se conserva para el siguiente inicio.

- Español
- English
- Français
- Deutsch
- Italiano
- Português

La versión 2.5 mantiene el **icono propio** para el EXE, las ventanas y la barra de tareas. Los archivos PNG e ICO están en `CapturaLibre/Assets/`. También conserva el botón **«Más colores…»** en el editor, con paletas de colores vivos, pastel, oscuros y neutros y un selector para elegir cualquier color personalizado.

## Qué puedes hacer

- Capturar una zona del escritorio, todas las pantallas o la pantalla principal.
- **Seleccionar una ventana de una lista** (botón «Actualizar lista»). Se intenta restaurarla si está minimizada. La captura usa los píxeles visibles en pantalla: ventanas superpuestas, contenido protegido o ciertas aplicaciones aceleradas por GPU pueden no capturarse correctamente.
- Activar retardos de 0, 3, 5 y 10 segundos.
- **Minimizar otras ventanas al iniciar, sin cerrarlas**, o desactivar esa opción. El ajuste se guarda para siguientes inicios en `%LOCALAPPDATA%\CapturaLibre_DNET\ajustes.json`. También puedes restaurarlas mediante un botón; al cerrar CapturaLibre se intenta restaurar las que minimizó el programa.
- Editar con flechas, rectángulos, elipses, lápiz, texto y ocultación de datos por relleno negro; escoger colores básicos o usar **«Más colores…»** para paletas y tono personalizado.
- **Recortar**, **girar 90°** y **voltear** la imagen; deshacer hasta seis cambios.
- **Abrir un PNG/JPG del disco** para editarlo.
- Guardar PNG/JPG, guardado rápido en `Imágenes\CapturaLibre`, copiar la imagen o abrir su carpeta.
- Atajos **F6** para zona, **F7** para todas las pantallas, **F8** para la principal; en el editor **Ctrl+Z**, **Ctrl+S** y **Ctrl+C**.
- **Modo DNET**: guardado directo en PNG mediante el botón dedicado o el disparo externo, carpeta seleccionable y nombres correlativos, sin cambiar el foco de DNET.
- **Ajuste automático DNET**: elimina el espacio vacío situado a la derecha y debajo del contenido, conserva la zona útil con un margen de seguridad y evita incluir la barra de tareas.
- Los botones normales, incluida **Capturar ventana**, abren siempre el editor para pintar, escribir, recortar y ocultar datos.
- El botón **?** junto al disparo externo explica dentro del programa la diferencia entre ambos modos.

**Privacidad:** ocultar datos reemplaza por negro los píxeles de la imagen que guardes; no confíes en dibujos semitransparentes para ocultar información sensible. La función «Deshacer» mantiene copias temporales en la memoria hasta cerrar el editor.

## Conseguir el ejecutable EXE para Windows

### Opción 1 — GitHub Actions (no hace falta Visual Studio)

1. Sube **el contenido de esta carpeta**, incluyendo `.github`, a la raíz de un repositorio de GitHub. Lee [PUBLICAR_EN_GITHUB.md](PUBLICAR_EN_GITHUB.md) para los pasos.
2. En el repositorio abre **Actions** y busca «Compilar CapturaLibre para Windows».
3. Ejecuta el flujo (o espera a que se ejecute con el push). Solo si sale **verde**, abre la ejecución y descarga el artefacto **CapturaLibre-DNET-2.5.1-Windows-x64**.
4. Extrae el ZIP descargado del artefacto: ahí encontrarás el `.exe` creado por GitHub. **Pruébalo** antes de distribuirlo como versión estable.

### Opción 2 — Compilar desde Windows o Visual Studio 2022

Instala el **SDK .NET 8** y, en la raíz del proyecto, haz doble clic en `Compilar_EXE.cmd`, o ejecuta:

```powershell
dotnet publish CapturaLibre/CapturaLibre.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=none -p:DebugSymbols=false -o publicar
```

Si usas Visual Studio 2022, abre `CapturaLibre/CapturaLibre.csproj`, compila y ejecuta con F5.

**Icono de CapturaLibre:** el archivo `CapturaLibre/Assets/CapturaLibre.ico` está conectado al proyecto como `ApplicationIcon`, de modo que aparecerá en el EXE compilado; se incrusta también como recurso para mostrarlo en la barra de título de la ventana principal y el editor. El archivo PNG en la misma carpeta es una copia para la documentación. Si Windows sigue mostrando el icono anterior, cierra el programa, recompila y actualiza el Explorador de archivos (Windows puede mantener iconos en caché).

El resultado del comando `dotnet publish` se guarda en `publicar\CapturaLibre_DNET.exe` y no necesita tener el runtime .NET instalado. También puedes ejecutar el código con `Ejecutar_con_Dotnet.cmd` si tienes el SDK.

**Aviso de Windows:** el EXE aún no está firmado digitalmente y SmartScreen podría mostrar una advertencia. Descarga únicamente de un repositorio de confianza; no desactives la protección general de Windows para usarlo.

## Uso básico

1. Abre la aplicación. La edición DNET no minimiza otras ventanas al iniciar. Puedes activar esa opción manualmente si alguna vez la necesitas.
2. Pulsa uno de los botones para elegir qué capturar. Si quieres una ventana, selecciónala en la lista y pulsa «Capturar ventana».
3. En el editor, usa herramientas, recorta o gira. Para ampliar los colores, pulsa **«Más colores…»**, selecciona una paleta o «Elegir cualquier color…». El color elegido se usa en flechas, texto, figuras y lápiz. Guarda o copia la imagen.
4. Si necesitas las ventanas anteriores, pulsa «Restaurar ventanas».

## Modo DNET para crear el manual visual

1. Activa **«Activar disparo externo DNET»**.
2. Pulsa **«Elegir carpeta…»** y selecciona la carpeta `01_CAPTURAS` del manual.
3. Deja activado **«Ajustar automáticamente al contenido útil de DNET»** cuando quieras imágenes más legibles y sin grandes márgenes vacíos.
4. Deja CapturaLibre minimizado y prepara en DNET la pantalla o el menú que quieras documentar.
5. Para un disparo controlado desde otro programa, crea este archivo:

   `%LOCALAPPDATA%\CapturaLibre_DNET\capturar_dnet.txt`

   El contenido es opcional. Si escribes, por ejemplo, `017_propiedades_boton`, la captura se guardará como `017_propiedades_boton.png`. Si se deja vacío, CapturaLibre asigna el siguiente número disponible.
6. CapturaLibre toma la pantalla principal a resolución nativa, ajusta la imagen si la opción está activa, guarda el PNG sin abrir el editor y emite un sonido cuando el archivo ya está escrito.

El archivo de disparo se elimina automáticamente después de procesarlo. Esto permite capturar menús desplegados y otras situaciones que se cerrarían al cambiar el foco hacia el capturador.

## Participar y donar

Este proyecto es de código abierto. Consulta [CONTRIBUTING.md](CONTRIBUTING.md), [SECURITY.md](SECURITY.md), [DONACIONES.md](DONACIONES.md) y el archivo [LICENSE](LICENSE). Las donaciones son **opcionales**, ayudarán a continuar el desarrollo de **DNET 2.0.0** y se canalizarán mediante [GitHub Sponsors de arturo1961](https://github.com/sponsors/arturo1961) cuando GitHub apruebe el perfil. Todas las funciones seguirán siendo gratuitas.

## Limitaciones conocidas

- Programa diseñado exclusivamente para Windows: requiere WinForms y las APIs de captura del escritorio de Windows.
- La captura de una ventana obtiene el contenido visible de su área en pantalla, **no** una imagen garantizada de ventanas tapadas, de contenido protegido o de ventanas fuera de pantalla.
- Se ha verificado en Windows 11. Para nuevas versiones, monitores múltiples o configuraciones distintas, sigue `PRUEBAS.md`.
- Todavía no hay instalador MSI, firma digital ni actualización automática.
