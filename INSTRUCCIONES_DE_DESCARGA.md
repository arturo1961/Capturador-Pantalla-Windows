# CapturaLibre DNET 2.4 para Windows

## Puesta en marcha

1. Descomprime el ZIP completo.
2. Ejecuta `CapturaLibre_DNET.exe`.
3. Activa **Activar disparo externo DNET** solamente cuando quieras controlar capturas desde otro programa.
4. Los botones normales abren siempre el editor; **Captura DNET en 5 s** guarda directamente en la carpeta elegida.

El ejecutable es autónomo para Windows x64 y no necesita instalar .NET 8.

## Capturas automáticas para documentación

Con CapturaLibre DNET abierto y el modo DNET activado, otro programa puede solicitar una captura creando:

`%LOCALAPPDATA%\CapturaLibre_DNET\capturar_dnet.txt`

El archivo puede estar vacío para utilizar un nombre correlativo. También puede contener el nombre deseado, por ejemplo `017_propiedades_boton`.

## Seguridad y privacidad

- Las capturas se procesan y guardan localmente.
- El programa no incorpora publicidad, telemetría ni subida automática de archivos.
- Revisa siempre las imágenes antes de compartirlas para evitar publicar datos personales.
- El EXE todavía no está firmado digitalmente; Windows puede mostrar una advertencia de SmartScreen.

## Proyecto gratuito

CapturaLibre DNET es gratuito y de código abierto. Las futuras aportaciones voluntarias ayudarán a mantener el programa y a continuar el desarrollo de DNET 2.0.0.
