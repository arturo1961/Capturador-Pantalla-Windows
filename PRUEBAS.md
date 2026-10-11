# Lista de pruebas de CapturaLibre 2.5, edición DNET (Windows)

Registra versión de Windows, resolución/escalado de cada pantalla y el resultado de cada punto antes de distribuir el EXE.

- [ ] `dotnet build CapturaLibre/CapturaLibre.csproj -c Release` termina correctamente.
- [ ] El EXE publicado inicia sin instalar el runtime .NET.
- [ ] El selector muestra Español, English, Français, Deutsch, Italiano y Português.
- [ ] Cambiar el idioma actualiza la ventana principal, el editor, las paletas y la ayuda DNET sin reiniciar.
- [ ] El idioma seleccionado se conserva después de cerrar y volver a abrir el programa.
- [ ] En los seis idiomas, el selector y los botones DNET se muestran completos, sin texto cortado.
- [ ] La ventana principal es legible y desplazable si hay escalado al 125 % / 150 %.
- [ ] Al iniciar, minimiza otras ventanas **sin cerrarlas**; al pulsar Restaurar, vuelven.
- [ ] Al desmarcar «Minimizar otras ventanas al iniciar», el ajuste se conserva después de reiniciar.
- [ ] Escogiendo ventana en el desplegable y pulsando «Capturar ventana» se obtiene una imagen útil; actualizar lista incorpora ventanas recién abiertas.
- [ ] Seleccionar una zona funciona y Esc/clic derecho cancelan.
- [ ] Capturas con F6/F7/F8 y los botones de pantalla/zona funcionan.
- [ ] Retardos de 0, 3, 5 y 10 segundos funcionan.
- [ ] Se capturan monitores múltiples, incluso uno situado a la izquierda del principal.
- [ ] La ventana de CapturaLibre no aparece en sus propias capturas.
- [ ] Puedes dibujar flechas, figuras, texto y lápiz; cambiar grosor y color.
- [ ] Redimensionar el editor hasta su ancho mínimo sigue mostrando TODOS los botones, grosor, los 4 colores, «Más colores…» y la vista previa; las barras se adaptan en varias filas, sin desplazamiento horizontal.
- [ ] «Más colores…» despliega cuatro familias de colores con seis tonos cada una.
- [ ] Seleccionar un tono cambia el color de flechas, textos, figuras y lápiz; el cuadro de vista previa refleja el elegido.
- [ ] «Elegir cualquier color…» abre el selector de Windows; al aceptar se usa el color elegido y al cancelar se mantiene el anterior.
- [ ] Cambiar entre colores básicos y los nuevos conserva el grosor, y no altera dibujos ya hechos.
- [ ] Con Windows al 100 %, 125 % y 150 % de escala, los textos de los botones no se cortan (en especial «Ocultar datos», «Guardar como…» y «Guardado rápido»).
- [ ] «Ocultar datos» rellena realmente de negro los píxeles en el PNG final.
- [ ] Recortar altera tamaño y contenido; Deshacer recupera la imagen anterior.
- [ ] Girar 90° y voltear funcionan y se pueden deshacer.
- [ ] Abrir una imagen PNG/JPG y editarla funciona.
- [ ] Guardar como PNG/JPG, guardado rápido, abrir carpeta y copiar al portapapeles funcionan.
- [ ] Cerrar CapturaLibre intenta restaurar las ventanas que haya minimizado.

## Modo DNET

- [ ] Activar «Activar disparo externo DNET» y elegir una carpeta conserva ambas preferencias tras reiniciar.
- [ ] Con el disparo externo DNET activado, «Capturar ventana» sigue abriendo el editor y permite pintar sobre la imagen.
- [ ] «Captura DNET en 5 s» guarda directamente el PNG sin abrir el editor.
- [ ] «Captura DNET en 5 s» oculta CapturaLibre antes de la cuenta atrás y permite dejar desplegado un menú de DNET.
- [ ] La captura se guarda directamente en PNG y no abre el editor.
- [ ] Los nombres automáticos comienzan por `001`, continúan correlativamente y no sobrescriben imágenes anteriores.
- [ ] Con CapturaLibre minimizado y DNET en primer plano, crear `%LOCALAPPDATA%\CapturaLibre_DNET\capturar_dnet.txt` produce una captura sin cambiar el foco.
- [ ] Si el archivo de disparo contiene `017_propiedades_boton`, el PNG se guarda con ese nombre.
- [ ] El sonido se reproduce después de que el PNG haya quedado guardado.
- [ ] Las capturas conservan la resolución nativa y el texto de DNET se ve nítido al 100 %.

**Limitación esperada:** «Capturar ventana» usa píxeles visibles, así que el contenido tapado/protegido puede no salir correctamente. No adjuntes capturas con información privada a informes de fallos.


## Icono de la versión 2.4

- [ ] Al compilar y ejecutar con F5, la ventana principal muestra el nuevo icono en su barra de título y en la barra de tareas.
- [ ] Al hacer una captura, el editor muestra el mismo icono.
- [ ] Al compilar con `Compilar_EXE.cmd`, el archivo `publicar\CapturaLibre_DNET.exe` muestra el icono en el Explorador de Windows.
- [ ] Todas las herramientas y paletas de CapturaLibre 2.3 siguen funcionando.
