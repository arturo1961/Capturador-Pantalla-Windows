# Publicar CapturaLibre DNET 2.5 en GitHub

El paquete de publicación contiene el **código fuente**, la licencia y el flujo automático que genera el EXE. Necesitas una cuenta de GitHub para publicarlo.

1. Crea un repositorio público llamado, por ejemplo, `CapturaLibre-DNET`, sin archivos iniciales.
2. Sube **el contenido del proyecto**, no la carpeta exterior, a la raíz del repositorio. Incluye el directorio oculto `.github`.
3. En **Actions**, elige «Compilar CapturaLibre para Windows». Si GitHub lo requiere, permite ejecutar los flujos de trabajo.
4. Espera a que la compilación termine en verde. Dentro de la ejecución, descarga el artefacto `CapturaLibre-DNET-2.5.2-Windows-x64`, que contendrá el EXE portátil y el instalador.
5. Prueba el EXE en Windows 10/11 siguiendo [PRUEBAS.md](PRUEBAS.md). Si falla, conserva el error del registro de Actions para corregirlo.
6. Tras superar las pruebas, podrás crear un **Release** con el EXE; no lo anuncies como estable antes de comprobarlo.
7. Para el botón de patrocinio, edita `.github/FUNDING.yml` con el identificador **real** de tu cuenta de donaciones. No es obligatorio; todas las funciones siguen siendo gratuitas.

Para subir con git desde el directorio del proyecto:

```bash
git init
git add .
git commit -m "Publicar CapturaLibre DNET 2.5.2 con instalador"
git branch -M main
git remote add origin https://github.com/arturo1961/Capturador-Pantalla-Windows.git
git push -u origin main
```

El repositorio es `arturo1961/Capturador-Pantalla-Windows`.
