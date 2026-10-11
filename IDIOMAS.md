# Idiomas de CapturaLibre

CapturaLibre incluye **61 idiomas** en un único ejecutable. El idioma se elige en la esquina superior derecha y queda guardado para el siguiente inicio.

## España

- Español
- Català
- Valencià
- Galego
- Euskara
- Aranés / Occitan

## Europa

- English
- Français
- Deutsch
- Italiano
- Português
- Nederlands
- Polski
- Română
- Русский
- Dansk
- Svenska
- Norsk
- Suomi
- Íslenska
- Čeština
- Slovenčina
- Magyar
- Ελληνικά
- Български
- Українська
- Hrvatski
- Српски
- Slovenščina
- Türkçe
- Eesti
- Latviešu
- Lietuvių
- Македонски
- Shqip
- Gaeilge
- Cymraeg
- Malti

## América, Asia, África y otros idiomas

- Português (Brasil)
- 简体中文
- 繁體中文
- 日本語
- 한국어
- हिन्दी
- বাংলা
- தமிழ்
- తెలుగు
- मराठी
- ગુજરાતી
- ਪੰਜਾਬੀ
- العربية
- עברית
- فارسی
- اردو
- Bahasa Indonesia
- Bahasa Melayu
- ไทย
- Tiếng Việt
- Filipino
- Kiswahili
- Afrikaans

Los idiomas árabe, hebreo, persa y urdu activan automáticamente la disposición de derecha a izquierda. Los botones ajustan su texto para aprovechar el espacio disponible.

## Añadir o corregir un idioma sin recompilar

Al iniciarse, CapturaLibre busca archivos `.json` dentro de una carpeta llamada `Idiomas`, situada junto a `CapturaLibre_DNET.exe`. Un paquete externo con el mismo código que uno incorporado corrige o sustituye sus textos; un código nuevo añade otra opción al selector.

Ejemplo mínimo:

```json
{
  "code": "xx",
  "displayName": "Nombre del idioma",
  "rightToLeft": false,
  "sortOrder": 1000,
  "texts": {
    "Main.Language": "Idioma:",
    "Capture.Region": "Seleccionar zona con el ratón [F6]"
  }
}
```

Los textos ausentes usan la versión inglesa como respaldo. Esto permite corregir una frase concreta sin copiar todo el diccionario. Mantén sin traducir los nombres `CapturaLibre` y `DNET`, las combinaciones como `F6`, `Ctrl+S` y la ruta `%LOCALAPPDATA%\CapturaLibre_DNET\capturar_dnet.txt`.

Las traducciones iniciales de los paquetes incorporados se han generado con ayuda automática y deben revisarse por hablantes nativos. Las correcciones son bienvenidas mediante una propuesta en GitHub.
