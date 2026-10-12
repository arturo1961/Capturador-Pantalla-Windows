# 📸 CapturaLibre DNET 2.6.0 — C# / .NET 8

[Español](README.md) | [简体中文](README.zh-CN.md)

CapturaLibre es un capturador y editor de pantalla **gratis, multilingüe y de código abierto**, para Windows 10 y 11. El proyecto funciona **localmente**: no tiene publicidad, telemetría ni subida automática de capturas. Licencia [MIT](LICENSE).

> **Versión estable 2.6.0:** incorpora captura guiada con lupa, nuevas herramientas de edición, perfiles, nombres automáticos, historial local y miniaturas de ventanas. Incluye 62 idiomas, el instalador para Windows y la edición portátil autónoma.

**Compatibilidad:** esta variante apunta a **.NET 8**, compatible con **Visual Studio 2022 17.8 o posterior**, siempre que tengas instalado el SDK de .NET 8 y el componente de desarrollo de escritorio. Lee [SOLUCION_VISUAL_STUDIO.md](SOLUCION_VISUAL_STUDIO.md) si aparece el error sobre .NET 10.

## Idiomas disponibles

El mismo ejecutable permite cambiar entre **62 idiomas** desde el selector situado en la esquina superior derecha. La elección se conserva para el siguiente inicio. Están incluidos el español, asturiano, catalán, valenciano, gallego, euskera y aranés/occitano, además de una selección amplia de idiomas europeos, asiáticos y de otras regiones.

Los botones ajustan automáticamente el tamaño de sus textos para que las traducciones largas no queden cortadas. Árabe, hebreo, persa y urdu cambian también la disposición de la ventana al sentido de lectura de derecha a izquierda.

Consulta [IDIOMAS.md](IDIOMAS.md) para ver la lista completa y el sistema de paquetes ampliables. Se pueden añadir o corregir traducciones colocando un archivo JSON en la carpeta `Idiomas` situada junto al ejecutable, **sin recompilar el programa ni cambiar su versión**.

La versión 2.5 mantiene el **icono propio** para el EXE, las ventanas y la barra de tareas. Los archivos PNG e ICO están en `CapturaLibre/Assets/`. También conserva el botón **«Más colores…»** en el editor, con paletas de colores vivos, pastel, oscuros y neutros y un selector para elegir cualquier color personalizado.

## Qué puedes hacer

- Capturar una zona del escritorio, todas las pantallas o la pantalla principal.
- Seleccionar una zona con lupa, coordenadas y punto de mira para conseguir recortes precisos.
- **Seleccionar una ventana de una lista** (botón «Actualizar lista»). Se intenta restaurarla si está minimizada. La captura usa los píxeles visibles en pantalla: ventanas superpuestas, contenido protegido o ciertas aplicaciones aceleradas por GPU pueden no capturarse correctamente.
- Activar retardos de 0, 3, 5 y 10 segundos.
- **Minimizar otras ventanas al iniciar, sin cerrarlas**, o desactivar esa opción. El ajuste se guarda para siguientes inicios en `%LOCALAPPDATA%\CapturaLibre_DNET\ajustes.json`. También puedes restaurarlas mediante un botón; al cerrar CapturaLibre se intenta restaurar las que minimizó el programa.
- Editar con flechas, rectángulos, elipses, lápiz, pasos numerados y texto enriquecido; pixelar, desenfocar u ocultar datos; mover la última anotación y escoger colores básicos o personalizados.
- **Recortar**, **girar 90°** y **voltear** la imagen; deshacer hasta seis cambios.
- **Abrir un PNG/JPG del disco** para editarlo.
- Guardar PNG/JPG, guardado rápido en `Imágenes\CapturaLibre`, copiar la imagen o abrir su carpeta.
- Usar perfiles preparados para manuales, documentación, privacidad o capturas rápidas.
- Configurar nombres automáticos con fecha, hora, contador y nombre de ventana.
- Consultar un historial local con miniaturas y volver a abrir capturas recientes.
- Atajos **F6** para zona, **F7** para todas las pantallas, **F8** para la principal; en el editor **Ctrl+Z**, **Ctrl+S** y **Ctrl+C**.
- **Modo DNET**: guardado directo en PNG mediante el botón dedicado o el disparo externo, carpeta seleccionable y nombres correlativos, sin cambiar el foco de DNET.
- **Ajuste automático DNET**: elimina el espacio vacío situado a la derecha y debajo del contenido, conserva la zona útil con un margen de seguridad y evita incluir la barra de tareas.
- **Privacidad de la carpeta**: cada usuario elige dónde guardar las imágenes; la interfaz no muestra la ruta personal completa y la ubicación queda únicamente en la configuración local.
- Los botones normales, incluida **Capturar ventana**, abren siempre el editor para pintar, escribir, recortar y ocultar datos.
- El botón **?** junto al disparo externo explica dentro del programa la diferencia entre ambos modos.

**Privacidad:** ocultar datos reemplaza por negro los píxeles de la imagen que guardes; no confíes en dibujos semitransparentes para ocultar información sensible. La función «Deshacer» mantiene copias temporales en la memoria hasta cerrar el editor.

## Descargar para Windows

La publicación ofrece dos opciones:

- `Instalar_CapturaLibre_DNET_2.6.0.exe`: opción recomendada, con asistente, menú Inicio, acceso directo opcional y desinstalador de Windows.
- `CapturaLibre_DNET.exe`: edición portátil que se abre directamente sin instalación.

Ambas son autónomas y no necesitan que .NET 8 esté instalado.

## Compilar el proyecto

### Opción 1 — GitHub Actions (no hace falta Visual Studio)

1. Sube **el contenido de esta carpeta**, incluyendo `.github`, a la raíz de un repositorio de GitHub. Lee [PUBLICAR_EN_GITHUB.md](PUBLICAR_EN_GITHUB.md) para los pasos.
2. En el repositorio abre **Actions** y busca «Compilar CapturaLibre para Windows».
3. Ejecuta el flujo (o espera a que se ejecute con el push). Solo si sale **verde**, abre la ejecución y descarga el artefacto **CapturaLibre-DNET-2.6.0-Windows-x64**.
4. Extrae el ZIP descargado del artefacto: ahí encontrarás el `.exe` creado por GitHub. **Pruébalo** antes de distribuirlo como versión estable.

### Opción 2 — Compilar desde Windows o Visual Studio 2022

Instala el **SDK .NET 8** y, en la raíz del proyecto, haz doble clic en `Compilar_EXE.cmd`, o ejecuta:

```powershell
dotnet publish CapturaLibre/CapturaLibre.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=none -p:DebugSymbols=false -o publicar
```

Si usas Visual Studio 2022, abre `CapturaLibre/CapturaLibre.csproj`, compila y ejecuta con F5.

**Icono de CapturaLibre:** el archivo `CapturaLibre/Assets/CapturaLibre.ico` está conectado al proyecto como `ApplicationIcon`, de modo que aparecerá en el EXE compilado; se incrusta también como recurso para mostrarlo en la barra de título de la ventana principal y el editor. El archivo PNG en la misma carpeta es una copia para la documentación. Si Windows sigue mostrando el icono anterior, cierra el programa, recompila y actualiza el Explorador de archivos (Windows puede mantener iconos en caché).

El resultado del comando `dotnet publish` se guarda en `publicar\CapturaLibre_DNET.exe` y no necesita tener el runtime .NET instalado. El instalador se genera con `Instalador\CapturaLibre_DNET.iss`. También puedes ejecutar el código con `Ejecutar_con_Dotnet.cmd` si tienes el SDK.

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

**CAPTURALIBRE ES GRATUITO Y LO SEGUIRÁ SIENDO. DONAR O NO DONAR ES DECISIÓN EXCLUSIVA DE CADA PERSONA.** ❤️

Si CapturaLibre te resulta útil y quieres apoyar voluntariamente su mantenimiento y evolución, puedes hacerlo por cualquiera de estos medios:

- [PayPal: paypal.me/acarballoflandes](https://paypal.me/acarballoflandes)
- [Ko-fi: ko-fi.com/arturocarballo](https://ko-fi.com/arturocarballo)
- **Bizum:** `+34 662 024 169`
- [GitHub Sponsors de arturo1961](https://github.com/sponsors/arturo1961), cuando el perfil esté disponible.

Donar no desbloquea funciones, no ofrece ventajas especiales y no modifica la licencia ni las condiciones del programa. También puedes colaborar comunicando errores, proponiendo mejoras o difundiendo el proyecto.

Consulta [CONTRIBUTING.md](CONTRIBUTING.md), [SECURITY.md](SECURITY.md), [DONACIONES.md](DONACIONES.md) y el archivo [LICENSE](LICENSE).

## Limitaciones conocidas

- Programa diseñado exclusivamente para Windows: requiere WinForms y las APIs de captura del escritorio de Windows.
- La captura de una ventana obtiene el contenido visible de su área en pantalla, **no** una imagen garantizada de ventanas tapadas, de contenido protegido o de ventanas fuera de pantalla.
- Se ha verificado en Windows 11. Para nuevas versiones, monitores múltiples o configuraciones distintas, sigue `PRUEBAS.md`.
- Todavía no hay instalador MSI, firma digital ni actualización automática.
