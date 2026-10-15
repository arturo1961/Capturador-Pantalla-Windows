# Historial de versiones

## 2.6.1 (captura sin perder el foco)

- Nuevo botón **«Sin perder el foco · 5 s»**: oculta CapturaLibre y deja tiempo para abrir un menú o desplegable antes de capturar la pantalla principal.
- Nuevo atajo global **Impr Pant** mientras CapturaLibre está en ejecución: toma la captura sin activar la ventana del programa y conserva abierto el menú que se está documentando.
- La captura obtenida abre el editor normal para poder recortar, señalar, escribir u ocultar información.
- El nuevo botón está traducido en los 62 idiomas incluidos.

## 2.6.0 (captura y edición avanzada)

- Lupa con coordenadas y punto de mira al seleccionar una zona.
- Pasos numerados automáticos para tutoriales.
- Pixelado y desenfoque para ocultar información privada.
- Texto enriquecido con tamaño, negrita y fondo opcional.
- Movimiento de la última anotación para corregir su posición.
- Perfiles Manual, Documentación, Privacidad y Rápida.
- Nombres automáticos configurables con fecha, hora, contador y ventana.
- Historial local de capturas recientes, sin enviar datos a internet.
- Miniaturas al elegir una ventana concreta.
- Herramienta de validación y lista visual para revisar los idiomas antes de publicar.
- Las funciones nuevas, preferencias, historial, mensajes de estado y errores quedan traducidos en los 62 idiomas; ningún paquete depende del inglés como sustituto.
- Se incorpora la interfaz completa en asturiano (`ast`) dentro de la misma versión estable 2.6.0.
- El validador impide compilar una revisión de idiomas incompleta o con marcadores internos alterados.

## 2.5.3 (61 idiomas y paquetes ampliables)

- Se revisa la traducción catalana: «Actualitzar llista» y «Desar com a...».
- La interfaz conserva la misma distribución en los idiomas de derecha a izquierda; el texto mantiene su escritura natural sin invertir la cabecera ni las columnas.
- Un único ejecutable permite elegir entre 61 idiomas.
- Se incluyen español, catalán, valenciano, gallego, euskera y aranés/occitano.
- Árabe, hebreo, persa y urdu adaptan la interfaz al sentido de lectura de derecha a izquierda.
- El selector de idioma y su lista desplegable son más anchos para mostrar completos los nombres.
- Los botones reducen automáticamente el tamaño del texto cuando una traducción necesita más espacio.
- Se pueden añadir o corregir traducciones mediante archivos JSON en la carpeta `Idiomas`, sin recompilar el programa.
- Se distribuyen de nuevo el ejecutable portátil autónomo y el instalador para Windows.

## 2.5.2 (instalador para Windows)

- Nuevo instalador profesional `Instalar_CapturaLibre_DNET_2.5.2.exe`.
- Instalación por usuario, sin solicitar privilegios de administrador.
- Acceso en el menú Inicio, acceso directo opcional en el escritorio y desinstalador de Windows.
- Asistente disponible en Español, English, Français, Deutsch, Italiano y Português.
- Se conserva `CapturaLibre_DNET.exe` como edición portátil.
- No cambia el funcionamiento de CapturaLibre respecto a la versión 2.5.1.

## 2.5.1 (ajuste automático DNET)

- Nueva opción para ajustar automáticamente la captura al contenido útil de DNET.
- Elimina grandes márgenes vacíos a la derecha y debajo del contenido, conservando un margen de seguridad.
- La captura directa evita incluir la barra de tareas cuando el ajuste está activado.
- La preferencia se guarda y está disponible en los seis idiomas de la interfaz.
- Por privacidad, la interfaz no muestra la ruta personal: cada usuario elige la carpeta con el botón correspondiente y la ubicación queda guardada únicamente en su configuración local.

## 2.5.0 (interfaz multilingüe)

- Un único ejecutable permite elegir Español, English, Français, Deutsch, Italiano o Português.
- El idioma seleccionado se guarda y se recupera al volver a abrir CapturaLibre.
- Se traducen la ventana principal, el editor, las herramientas, las paletas de colores y la ayuda DNET.
- La ayuda DNET utiliza una ventana propia para traducir también el botón de aceptación.
- Se amplía el selector de idioma y se da más espacio al bloque DNET para evitar textos y botones cortados.

## 2.4.1 (corrección del editor)

- Los botones normales de captura, incluida **Capturar ventana**, abren siempre la imagen en el editor.
- El guardado PNG sin editor queda reservado al botón **Captura DNET en 5 s** y al disparo externo DNET.
- El control del modo DNET se denomina ahora **Activar disparo externo DNET** para evitar confusiones.
- Se añade un botón de ayuda **?** que explica la diferencia entre captura normal y captura directa DNET.

## 2.4.0 (versión estable)

- Primera publicación estable de CapturaLibre DNET.
- Interfaz verificada con escalado alto de Windows y todas las opciones visibles.
- Modo DNET con guardado PNG directo, nombres correlativos y disparo externo.
- Ejecutable autónomo de Windows x64 en un único archivo.
- Código fuente, licencia MIT, documentación, compilación automática y patrocinio integrados en GitHub.

## 2.4.0-beta.2 (edición DNET)

- Nuevo **Modo DNET** para guardar directamente en PNG, sin abrir el editor después de cada captura.
- CapturaLibre se oculta antes de iniciar la cuenta atrás para no cerrar los menús desplegados en DNET.
- Carpeta de destino seleccionable y nombres correlativos automáticos (`001_captura_dnet_...png`).
- Disparo externo mediante `%LOCALAPPDATA%\CapturaLibre_DNET\capturar_dnet.txt`, sin llevar el foco a CapturaLibre.
- El texto opcional del archivo de disparo se utiliza como nombre de la captura.
- Aviso sonoro únicamente después de guardar correctamente la imagen.
- La opción de minimizar otras ventanas al iniciar queda desactivada de manera predeterminada en instalaciones nuevas.

## 2.4.0-beta.1 (nuevo icono de aplicación)

- Nuevo icono original de CapturaLibre (cámara, selección y lápiz), incluido en PNG e ICO multirresolución.
- Icono establecido para `CapturaLibre.exe` en el archivo de proyecto C#.
- Icono visible en la barra de título de las ventanas principal, editor y selector (este último sin barra visible).
- Se conserva el editor, la captura y la paleta de colores de la versión 2.3 sin cambios funcionales.
- Preparado para compilar y validar en Windows con Visual Studio 2022 y .NET 8.


## 2.3.0-beta.1 (más paletas de colores)

- Nuevo botón **«Más colores…»** al lado de los cuatro colores básicos del editor.
- Paletas desplegables de tonos vivos, pastel, oscuros y neutros (24 tonos adicionales).
- Opción «Elegir cualquier color…» con el selector de colores integrado en Windows.
- Vista previa del color activo junto al botón, también cuando se elige uno de los cuatro originales.
- Se mantienen las demás funciones y el diseño adaptable de CapturaLibre 2.2.
- Pendiente de compilar y validar esta actualización en Windows/Visual Studio; el código anterior de captura y guardado no se ha modificado.

## 2.2.0-beta.1 (editor adaptable)

- Barras de herramientas del editor que se distribuyen en varias filas según el ancho de la ventana.
- Altura automática de cada barra para evitar botones cortados y scroll horizontal.
- Márgenes y espacios ampliados; botones dimensionados al texto, incluidos «Ocultar datos» y «Guardar como…».
- Grosor y paleta de colores agrupados para que no se separen de sus etiquetas.
- Se conservan las funciones de captura y edición; sigue usando .NET 8 y Visual Studio 2022.
- Pendiente de probar esta modificación en Windows a distintas escalas DPI.

## 2.1.1-beta.1 (compatibilidad con Visual Studio 2022)

- Se cambia el destino de `net10.0-windows` a `net8.0-windows` para abrir y compilar el proyecto con Visual Studio 2022 17.8 o posterior.
- Se actualizan GitHub Actions, las instrucciones y los archivos de ejecución para .NET 8.
- No se han cambiado las funciones de captura o edición. Continúa pendiente de compilación y pruebas en Windows.

## 2.1.0-beta.1 (código fuente; pendiente de pruebas)

- C# y Windows Forms: ventana principal amplia, de 1050 × 830 píxeles, con controles desplazables.
- Nuevo selector de ventanas abiertas con botón de actualización y captura de ventana.
- Preferencia persistente para minimizar otras ventanas al iniciar, sin cerrarlas.
- Editor: recortar, girar 90 grados y voltear imagen.
- Abrir archivos PNG/JPEG desde la ventana principal.
- Atajos F6/F7/F8 para iniciar capturas.
- Se conserva la edición, el guardado PNG/JPG, el portapapeles y la restauración de ventanas.
- Se añade `Compilar_EXE.cmd` y se mantiene GitHub Actions para preparar un EXE descargable.
- No se ha compilado ni probado en Windows desde el entorno de generación.

## 2.0.0-beta.1

- Migración inicial desde PowerShell a C# con Windows Forms y .NET 10.
- Capturas, anotaciones y guardado local.
