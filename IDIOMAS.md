# Idiomas de CapturaLibre

CapturaLibre incluye **62 idiomas** en un único ejecutable. En el primer inicio adopta automáticamente el idioma de Windows cuando existe una traducción compatible. El idioma también se puede elegir en la esquina superior derecha y queda guardado para los siguientes inicios.

## España

- Español
- Asturianu
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

Los textos en árabe, hebreo, persa y urdu conservan su escritura natural de derecha a izquierda, pero la estructura de la aplicación permanece igual que en español. La cabecera, los botones y las columnas no cambian de sitio al seleccionar estos idiomas.

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
