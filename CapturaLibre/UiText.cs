namespace CapturaLibre;

internal sealed record LanguageOption(string Code, string DisplayName)
{
    public override string ToString() => DisplayName;
}

internal static class UiText
{
    public static readonly LanguageOption[] Languages =
    [
        new("es", "Español"),
        new("en", "English"),
        new("fr", "Français"),
        new("de", "Deutsch"),
        new("it", "Italiano"),
        new("pt", "Português")
    ];

    private static readonly Dictionary<string, Dictionary<string, string>> Texts = new()
    {
        ["es"] = new()
        {
            ["Brand"] = "CapturaLibre",
            ["Main.Title"] = "CapturaLibre 2.5.1 - edición DNET",
            ["Main.Tagline"] = "Capturas útiles, privadas y sin complicaciones.",
            ["Main.Heading"] = "¿Qué deseas capturar?",
            ["Main.Language"] = "Idioma:",
            ["Capture.Region"] = "Seleccionar zona con el ratón   [F6]",
            ["Capture.All"] = "Todas las pantallas   [F7]",
            ["Capture.Primary"] = "Pantalla principal   [F8]",
            ["Capture.WindowSection"] = "Capturar una ventana específica",
            ["Capture.Refresh"] = "Actualizar lista",
            ["Capture.Window"] = "Capturar ventana",
            ["Capture.Delay"] = "Retardo antes de capturar:",
            ["Capture.Seconds"] = "segundos",
            ["Main.Restore"] = "Restaurar ventanas",
            ["Main.Minimize"] = "Minimizar otras ventanas",
            ["Main.MinimizeOnStart"] = "Minimizar otras ventanas al iniciar (sin cerrarlas)",
            ["Main.OpenImage"] = "Abrir imagen...",
            ["Dnet.Enable"] = "Activar disparo externo DNET",
            ["Dnet.ChooseFolder"] = "Elegir carpeta...",
            ["Dnet.Capture5"] = "Captura DNET en 5 s",
            ["Dnet.AutoCrop"] = "Ajustar automáticamente al contenido útil de DNET",
            ["Dnet.Folder"] = "Carpeta: {0}",
            ["Dnet.Trigger"] = "Disparo externo sin cambiar el foco: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
            ["Dnet.HelpAccessible"] = "Ayuda sobre el disparo externo DNET",
            ["Dnet.HelpTitle"] = "Ayuda del modo DNET",
            ["Dnet.HelpBody"] = "Hay dos formas de trabajar:\n\n• Capturas normales: los botones Zona, Pantallas, Pantalla principal y Capturar ventana abren siempre el editor para pintar, escribir, recortar u ocultar datos.\n\n• Captura directa DNET: el botón 'Captura DNET en 5 s' y el archivo capturar_dnet.txt guardan el PNG directamente, sin abrir el editor y sin cambiar el foco.\n\nLa opción de ajuste automático elimina el espacio vacío situado a la derecha y debajo del contenido DNET, conservando la zona útil y una separación de seguridad.\n\nLa primera casilla solamente activa o desactiva el disparo externo DNET. No cambia el funcionamiento de los botones normales.",
            ["Main.Help"] = "F6: zona - F7: todas las pantallas - F8: pantalla principal. Puedes editar, recortar, girar y ocultar datos. El contenido de las capturas no se envía a internet.",
            ["Main.Ready"] = "Listo para capturar. Todo se procesa en tu ordenador.",
            ["Editor.Title"] = "Editar captura - {0} x {1} - CapturaLibre",
            ["Tool.Arrow"] = "Flecha", ["Tool.Rectangle"] = "Rectángulo",
            ["Tool.Ellipse"] = "Elipse", ["Tool.Pencil"] = "Lápiz",
            ["Tool.Text"] = "Texto", ["Tool.Redact"] = "Ocultar datos",
            ["Tool.Crop"] = "Recortar", ["Tool.Thickness"] = "Grosor:",
            ["Action.Undo"] = "Deshacer", ["Action.Rotate"] = "Girar 90°",
            ["Action.Flip"] = "Voltear", ["Action.SaveAs"] = "Guardar como...",
            ["Action.QuickSave"] = "Guardado rápido", ["Action.Copy"] = "Copiar imagen",
            ["Action.OpenFolder"] = "Abrir carpeta", ["Color.Label"] = "Color:",
            ["Color.More"] = "Más colores...", ["Color.Vivid"] = "Colores vivos",
            ["Color.Pastel"] = "Tonos pastel", ["Color.Dark"] = "Colores oscuros",
            ["Color.Neutral"] = "Colores neutros", ["Color.Any"] = "Elegir cualquier color...",
            ["Color.Orange"] = "Naranja", ["Color.Yellow"] = "Amarillo",
            ["Color.Fuchsia"] = "Fucsia", ["Color.Turquoise"] = "Turquesa",
            ["Color.Violet"] = "Violeta", ["Color.BrightGreen"] = "Verde brillante",
            ["Color.PastelPink"] = "Rosa pastel", ["Color.SkyBlue"] = "Azul cielo",
            ["Color.Mint"] = "Verde menta", ["Color.Lavender"] = "Lavanda",
            ["Color.Peach"] = "Melocotón", ["Color.SoftYellow"] = "Amarillo suave",
            ["Color.Navy"] = "Azul marino", ["Color.Burgundy"] = "Burdeos",
            ["Color.Forest"] = "Verde bosque", ["Color.DarkPurple"] = "Púrpura oscuro",
            ["Color.Brown"] = "Marrón", ["Color.Slate"] = "Gris pizarra",
            ["Color.White"] = "Blanco", ["Color.LightGray"] = "Gris claro",
            ["Color.Gray"] = "Gris", ["Color.DarkGray"] = "Gris oscuro",
            ["Color.Black"] = "Negro", ["Color.Beige"] = "Beige",
            ["Editor.Ready"] = "Arrastra para anotar; haz clic para añadir texto. Ctrl+Z: deshacer - Ctrl+S: guardar - Ctrl+C: copiar.",
            ["Dialog.AddText"] = "Añadir texto a la captura",
            ["Dialog.TextPrompt"] = "Texto que deseas escribir sobre la imagen:",
            ["Dialog.OK"] = "Aceptar", ["Dialog.Cancel"] = "Cancelar",
            ["Selection.Title"] = "Seleccionar zona - Esc para cancelar"
        },
        ["en"] = new()
        {
            ["Main.Title"] = "CapturaLibre 2.5.1 - DNET edition",
            ["Main.Tagline"] = "Useful, private screenshots without complications.",
            ["Main.Heading"] = "What would you like to capture?",
            ["Main.Language"] = "Language:",
            ["Capture.Region"] = "Select an area with the mouse   [F6]",
            ["Capture.All"] = "All screens   [F7]", ["Capture.Primary"] = "Primary screen   [F8]",
            ["Capture.WindowSection"] = "Capture a specific window", ["Capture.Refresh"] = "Refresh list",
            ["Capture.Window"] = "Capture window", ["Capture.Delay"] = "Delay before capture:",
            ["Capture.Seconds"] = "seconds", ["Main.Restore"] = "Restore windows",
            ["Main.Minimize"] = "Minimize other windows",
            ["Main.MinimizeOnStart"] = "Minimize other windows on startup (without closing them)",
            ["Main.OpenImage"] = "Open image...", ["Dnet.Enable"] = "Enable external DNET trigger",
            ["Dnet.ChooseFolder"] = "Choose folder...", ["Dnet.Capture5"] = "DNET capture in 5 s",
            ["Dnet.AutoCrop"] = "Automatically fit to useful DNET content",
            ["Dnet.Folder"] = "Folder: {0}",
            ["Dnet.Trigger"] = "External trigger without changing focus: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
            ["Dnet.HelpAccessible"] = "Help for the external DNET trigger",
            ["Dnet.HelpTitle"] = "DNET mode help",
            ["Dnet.HelpBody"] = "There are two ways to work:\n\n• Normal captures: the Area, All screens, Primary screen and Capture window buttons always open the editor so you can draw, write, crop or hide data.\n\n• Direct DNET capture: the 'DNET capture in 5 s' button and the capturar_dnet.txt file save the PNG directly, without opening the editor or changing focus.\n\nThis checkbox only enables or disables the external DNET trigger. It does not change how the normal buttons work.",
            ["Main.Help"] = "F6: area - F7: all screens - F8: primary screen. You can edit, crop, rotate and hide data. Screenshot content is never sent to the internet.",
            ["Main.Ready"] = "Ready to capture. Everything is processed on your computer.",
            ["Editor.Title"] = "Edit capture - {0} x {1} - CapturaLibre",
            ["Tool.Arrow"] = "Arrow", ["Tool.Rectangle"] = "Rectangle", ["Tool.Ellipse"] = "Ellipse",
            ["Tool.Pencil"] = "Pencil", ["Tool.Text"] = "Text", ["Tool.Redact"] = "Hide data",
            ["Tool.Crop"] = "Crop", ["Tool.Thickness"] = "Thickness:",
            ["Action.Undo"] = "Undo", ["Action.Rotate"] = "Rotate 90°", ["Action.Flip"] = "Flip",
            ["Action.SaveAs"] = "Save as...", ["Action.QuickSave"] = "Quick save",
            ["Action.Copy"] = "Copy image", ["Action.OpenFolder"] = "Open folder",
            ["Color.Label"] = "Color:", ["Color.More"] = "More colors...",
            ["Color.Vivid"] = "Vivid colors", ["Color.Pastel"] = "Pastel tones",
            ["Color.Dark"] = "Dark colors", ["Color.Neutral"] = "Neutral colors",
            ["Color.Any"] = "Choose any color...", ["Color.Orange"] = "Orange",
            ["Color.Yellow"] = "Yellow", ["Color.Fuchsia"] = "Fuchsia", ["Color.Turquoise"] = "Turquoise",
            ["Color.Violet"] = "Violet", ["Color.BrightGreen"] = "Bright green",
            ["Color.PastelPink"] = "Pastel pink", ["Color.SkyBlue"] = "Sky blue",
            ["Color.Mint"] = "Mint green", ["Color.Lavender"] = "Lavender",
            ["Color.Peach"] = "Peach", ["Color.SoftYellow"] = "Soft yellow",
            ["Color.Navy"] = "Navy blue", ["Color.Burgundy"] = "Burgundy",
            ["Color.Forest"] = "Forest green", ["Color.DarkPurple"] = "Dark purple",
            ["Color.Brown"] = "Brown", ["Color.Slate"] = "Slate gray",
            ["Color.White"] = "White", ["Color.LightGray"] = "Light gray", ["Color.Gray"] = "Gray",
            ["Color.DarkGray"] = "Dark gray", ["Color.Black"] = "Black", ["Color.Beige"] = "Beige",
            ["Editor.Ready"] = "Drag to annotate; click to add text. Ctrl+Z: undo - Ctrl+S: save - Ctrl+C: copy.",
            ["Dialog.AddText"] = "Add text to the capture", ["Dialog.TextPrompt"] = "Text to write on the image:",
            ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "Cancel",
            ["Selection.Title"] = "Select an area - Esc to cancel"
        },
        ["fr"] = new()
        {
            ["Main.Title"] = "CapturaLibre 2.5.1 - édition DNET", ["Main.Tagline"] = "Des captures utiles, privées et sans complications.",
            ["Main.Heading"] = "Que souhaitez-vous capturer ?", ["Main.Language"] = "Langue :",
            ["Capture.Region"] = "Sélectionner une zone à la souris   [F6]", ["Capture.All"] = "Tous les écrans   [F7]",
            ["Capture.Primary"] = "Écran principal   [F8]", ["Capture.WindowSection"] = "Capturer une fenêtre précise",
            ["Capture.Refresh"] = "Actualiser la liste", ["Capture.Window"] = "Capturer la fenêtre",
            ["Capture.Delay"] = "Délai avant la capture :", ["Capture.Seconds"] = "secondes",
            ["Main.Restore"] = "Restaurer les fenêtres", ["Main.Minimize"] = "Réduire les autres fenêtres",
            ["Main.MinimizeOnStart"] = "Réduire les autres fenêtres au démarrage (sans les fermer)",
            ["Main.OpenImage"] = "Ouvrir une image...", ["Dnet.Enable"] = "Activer le déclencheur DNET externe",
            ["Dnet.ChooseFolder"] = "Choisir un dossier...", ["Dnet.Capture5"] = "Capture DNET dans 5 s",
            ["Dnet.AutoCrop"] = "Ajuster automatiquement au contenu DNET utile",
            ["Dnet.Folder"] = "Dossier : {0}", ["Dnet.Trigger"] = "Déclencheur externe sans changer le focus : %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
            ["Dnet.HelpAccessible"] = "Aide sur le déclencheur DNET externe",
            ["Dnet.HelpTitle"] = "Aide du mode DNET",
            ["Dnet.HelpBody"] = "Il existe deux façons de travailler :\n\n• Captures normales : les boutons Zone, Tous les écrans, Écran principal et Capturer la fenêtre ouvrent toujours l'éditeur pour dessiner, écrire, rogner ou masquer des données.\n\n• Capture DNET directe : le bouton 'Capture DNET dans 5 s' et le fichier capturar_dnet.txt enregistrent directement le PNG, sans ouvrir l'éditeur ni changer le focus.\n\nCette case active ou désactive uniquement le déclencheur DNET externe. Elle ne modifie pas le fonctionnement des boutons normaux.",
            ["Main.Help"] = "F6 : zone - F7 : tous les écrans - F8 : écran principal. Vous pouvez modifier, rogner, pivoter et masquer des données. Les captures ne sont jamais envoyées sur Internet.",
            ["Main.Ready"] = "Prêt à capturer. Tout est traité sur votre ordinateur.",
            ["Editor.Title"] = "Modifier la capture - {0} x {1} - CapturaLibre",
            ["Tool.Arrow"] = "Flèche", ["Tool.Rectangle"] = "Rectangle", ["Tool.Ellipse"] = "Ellipse",
            ["Tool.Pencil"] = "Crayon", ["Tool.Text"] = "Texte", ["Tool.Redact"] = "Masquer les données",
            ["Tool.Crop"] = "Rogner", ["Tool.Thickness"] = "Épaisseur :",
            ["Action.Undo"] = "Annuler", ["Action.Rotate"] = "Pivoter 90°", ["Action.Flip"] = "Retourner",
            ["Action.SaveAs"] = "Enregistrer sous...", ["Action.QuickSave"] = "Enregistrement rapide",
            ["Action.Copy"] = "Copier l'image", ["Action.OpenFolder"] = "Ouvrir le dossier",
            ["Color.Label"] = "Couleur :", ["Color.More"] = "Plus de couleurs...",
            ["Color.Vivid"] = "Couleurs vives", ["Color.Pastel"] = "Tons pastel", ["Color.Dark"] = "Couleurs sombres",
            ["Color.Neutral"] = "Couleurs neutres", ["Color.Any"] = "Choisir une couleur...",
            ["Color.Orange"] = "Orange", ["Color.Yellow"] = "Jaune", ["Color.Fuchsia"] = "Fuchsia",
            ["Color.Turquoise"] = "Turquoise", ["Color.Violet"] = "Violet", ["Color.BrightGreen"] = "Vert vif",
            ["Color.PastelPink"] = "Rose pastel", ["Color.SkyBlue"] = "Bleu ciel", ["Color.Mint"] = "Vert menthe",
            ["Color.Lavender"] = "Lavande", ["Color.Peach"] = "Pêche", ["Color.SoftYellow"] = "Jaune doux",
            ["Color.Navy"] = "Bleu marine", ["Color.Burgundy"] = "Bordeaux", ["Color.Forest"] = "Vert forêt",
            ["Color.DarkPurple"] = "Violet foncé", ["Color.Brown"] = "Marron", ["Color.Slate"] = "Gris ardoise",
            ["Color.White"] = "Blanc", ["Color.LightGray"] = "Gris clair", ["Color.Gray"] = "Gris",
            ["Color.DarkGray"] = "Gris foncé", ["Color.Black"] = "Noir", ["Color.Beige"] = "Beige",
            ["Editor.Ready"] = "Faites glisser pour annoter ; cliquez pour ajouter du texte. Ctrl+Z : annuler - Ctrl+S : enregistrer - Ctrl+C : copier.",
            ["Dialog.AddText"] = "Ajouter du texte à la capture", ["Dialog.TextPrompt"] = "Texte à écrire sur l'image :",
            ["Dialog.OK"] = "Valider", ["Dialog.Cancel"] = "Annuler", ["Selection.Title"] = "Sélectionner une zone - Échap pour annuler"
        },
        ["de"] = new()
        {
            ["Main.Title"] = "CapturaLibre 2.5.1 - DNET-Ausgabe", ["Main.Tagline"] = "Nützliche, private Bildschirmfotos ohne Umstände.",
            ["Main.Heading"] = "Was möchten Sie aufnehmen?", ["Main.Language"] = "Sprache:",
            ["Capture.Region"] = "Bereich mit der Maus auswählen   [F6]", ["Capture.All"] = "Alle Bildschirme   [F7]",
            ["Capture.Primary"] = "Hauptbildschirm   [F8]", ["Capture.WindowSection"] = "Bestimmtes Fenster aufnehmen",
            ["Capture.Refresh"] = "Liste aktualisieren", ["Capture.Window"] = "Fenster aufnehmen",
            ["Capture.Delay"] = "Verzögerung vor Aufnahme:", ["Capture.Seconds"] = "Sekunden",
            ["Main.Restore"] = "Fenster wiederherstellen", ["Main.Minimize"] = "Andere Fenster minimieren",
            ["Main.MinimizeOnStart"] = "Andere Fenster beim Start minimieren (nicht schließen)",
            ["Main.OpenImage"] = "Bild öffnen...", ["Dnet.Enable"] = "Externen DNET-Auslöser aktivieren",
            ["Dnet.ChooseFolder"] = "Ordner auswählen...", ["Dnet.Capture5"] = "DNET-Aufnahme in 5 s",
            ["Dnet.AutoCrop"] = "Automatisch an den nützlichen DNET-Inhalt anpassen",
            ["Dnet.Folder"] = "Ordner: {0}", ["Dnet.Trigger"] = "Externer Auslöser ohne Fokuswechsel: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
            ["Dnet.HelpAccessible"] = "Hilfe zum externen DNET-Auslöser",
            ["Dnet.HelpTitle"] = "Hilfe zum DNET-Modus",
            ["Dnet.HelpBody"] = "Es gibt zwei Arbeitsweisen:\n\n• Normale Aufnahmen: Die Schaltflächen Bereich, Alle Bildschirme, Hauptbildschirm und Fenster aufnehmen öffnen immer den Editor. Dort können Sie zeichnen, schreiben, zuschneiden oder Daten verdecken.\n\n• Direkte DNET-Aufnahme: Die Schaltfläche 'DNET-Aufnahme in 5 s' und die Datei capturar_dnet.txt speichern das PNG direkt, ohne den Editor zu öffnen oder den Fokus zu wechseln.\n\nDieses Kontrollkästchen aktiviert oder deaktiviert nur den externen DNET-Auslöser. Die normalen Schaltflächen funktionieren weiterhin unverändert.",
            ["Main.Help"] = "F6: Bereich - F7: alle Bildschirme - F8: Hauptbildschirm. Bilder können bearbeitet, zugeschnitten, gedreht und verdeckt werden. Aufnahmen werden nicht ins Internet gesendet.",
            ["Main.Ready"] = "Aufnahmebereit. Alles wird auf Ihrem Computer verarbeitet.",
            ["Editor.Title"] = "Aufnahme bearbeiten - {0} x {1} - CapturaLibre",
            ["Tool.Arrow"] = "Pfeil", ["Tool.Rectangle"] = "Rechteck", ["Tool.Ellipse"] = "Ellipse",
            ["Tool.Pencil"] = "Stift", ["Tool.Text"] = "Text", ["Tool.Redact"] = "Daten verdecken",
            ["Tool.Crop"] = "Zuschneiden", ["Tool.Thickness"] = "Stärke:",
            ["Action.Undo"] = "Rückgängig", ["Action.Rotate"] = "90° drehen", ["Action.Flip"] = "Spiegeln",
            ["Action.SaveAs"] = "Speichern unter...", ["Action.QuickSave"] = "Schnellspeichern",
            ["Action.Copy"] = "Bild kopieren", ["Action.OpenFolder"] = "Ordner öffnen",
            ["Color.Label"] = "Farbe:", ["Color.More"] = "Weitere Farben...",
            ["Color.Vivid"] = "Leuchtende Farben", ["Color.Pastel"] = "Pastelltöne", ["Color.Dark"] = "Dunkle Farben",
            ["Color.Neutral"] = "Neutrale Farben", ["Color.Any"] = "Beliebige Farbe wählen...",
            ["Color.Orange"] = "Orange", ["Color.Yellow"] = "Gelb", ["Color.Fuchsia"] = "Fuchsia",
            ["Color.Turquoise"] = "Türkis", ["Color.Violet"] = "Violett", ["Color.BrightGreen"] = "Hellgrün",
            ["Color.PastelPink"] = "Pastellrosa", ["Color.SkyBlue"] = "Himmelblau", ["Color.Mint"] = "Mintgrün",
            ["Color.Lavender"] = "Lavendel", ["Color.Peach"] = "Pfirsich", ["Color.SoftYellow"] = "Hellgelb",
            ["Color.Navy"] = "Marineblau", ["Color.Burgundy"] = "Bordeaux", ["Color.Forest"] = "Waldgrün",
            ["Color.DarkPurple"] = "Dunkelviolett", ["Color.Brown"] = "Braun", ["Color.Slate"] = "Schiefergrau",
            ["Color.White"] = "Weiß", ["Color.LightGray"] = "Hellgrau", ["Color.Gray"] = "Grau",
            ["Color.DarkGray"] = "Dunkelgrau", ["Color.Black"] = "Schwarz", ["Color.Beige"] = "Beige",
            ["Editor.Ready"] = "Zum Markieren ziehen; zum Schreiben klicken. Ctrl+Z: rückgängig - Ctrl+S: speichern - Ctrl+C: kopieren.",
            ["Dialog.AddText"] = "Text zur Aufnahme hinzufügen", ["Dialog.TextPrompt"] = "Text für das Bild:",
            ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "Abbrechen", ["Selection.Title"] = "Bereich auswählen - Esc zum Abbrechen"
        },
        ["it"] = new()
        {
            ["Main.Title"] = "CapturaLibre 2.5.1 - edizione DNET", ["Main.Tagline"] = "Catture utili, private e senza complicazioni.",
            ["Main.Heading"] = "Cosa desideri catturare?", ["Main.Language"] = "Lingua:",
            ["Capture.Region"] = "Seleziona un'area con il mouse   [F6]", ["Capture.All"] = "Tutti gli schermi   [F7]",
            ["Capture.Primary"] = "Schermo principale   [F8]", ["Capture.WindowSection"] = "Cattura una finestra specifica",
            ["Capture.Refresh"] = "Aggiorna elenco", ["Capture.Window"] = "Cattura finestra",
            ["Capture.Delay"] = "Ritardo prima della cattura:", ["Capture.Seconds"] = "secondi",
            ["Main.Restore"] = "Ripristina finestre", ["Main.Minimize"] = "Riduci le altre finestre",
            ["Main.MinimizeOnStart"] = "Riduci le altre finestre all'avvio (senza chiuderle)",
            ["Main.OpenImage"] = "Apri immagine...", ["Dnet.Enable"] = "Attiva trigger DNET esterno",
            ["Dnet.ChooseFolder"] = "Scegli cartella...", ["Dnet.Capture5"] = "Cattura DNET tra 5 s",
            ["Dnet.AutoCrop"] = "Adatta automaticamente al contenuto DNET utile",
            ["Dnet.Folder"] = "Cartella: {0}", ["Dnet.Trigger"] = "Trigger esterno senza cambiare focus: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
            ["Dnet.HelpAccessible"] = "Guida per il trigger DNET esterno",
            ["Dnet.HelpTitle"] = "Guida della modalità DNET",
            ["Dnet.HelpBody"] = "Esistono due modi di lavorare:\n\n• Catture normali: i pulsanti Area, Tutti gli schermi, Schermo principale e Cattura finestra aprono sempre l'editor per disegnare, scrivere, ritagliare o nascondere dati.\n\n• Cattura DNET diretta: il pulsante 'Cattura DNET tra 5 s' e il file capturar_dnet.txt salvano direttamente il PNG, senza aprire l'editor e senza cambiare il focus.\n\nQuesta casella attiva o disattiva soltanto il trigger DNET esterno. Non modifica il funzionamento dei pulsanti normali.",
            ["Main.Help"] = "F6: area - F7: tutti gli schermi - F8: schermo principale. Puoi modificare, ritagliare, ruotare e nascondere dati. Le catture non vengono inviate a Internet.",
            ["Main.Ready"] = "Pronto per la cattura. Tutto viene elaborato sul computer.",
            ["Editor.Title"] = "Modifica cattura - {0} x {1} - CapturaLibre",
            ["Tool.Arrow"] = "Freccia", ["Tool.Rectangle"] = "Rettangolo", ["Tool.Ellipse"] = "Ellisse",
            ["Tool.Pencil"] = "Matita", ["Tool.Text"] = "Testo", ["Tool.Redact"] = "Nascondi dati",
            ["Tool.Crop"] = "Ritaglia", ["Tool.Thickness"] = "Spessore:",
            ["Action.Undo"] = "Annulla", ["Action.Rotate"] = "Ruota 90°", ["Action.Flip"] = "Ribalta",
            ["Action.SaveAs"] = "Salva con nome...", ["Action.QuickSave"] = "Salvataggio rapido",
            ["Action.Copy"] = "Copia immagine", ["Action.OpenFolder"] = "Apri cartella",
            ["Color.Label"] = "Colore:", ["Color.More"] = "Altri colori...",
            ["Color.Vivid"] = "Colori vivaci", ["Color.Pastel"] = "Toni pastello", ["Color.Dark"] = "Colori scuri",
            ["Color.Neutral"] = "Colori neutri", ["Color.Any"] = "Scegli un colore...",
            ["Color.Orange"] = "Arancione", ["Color.Yellow"] = "Giallo", ["Color.Fuchsia"] = "Fucsia",
            ["Color.Turquoise"] = "Turchese", ["Color.Violet"] = "Viola", ["Color.BrightGreen"] = "Verde brillante",
            ["Color.PastelPink"] = "Rosa pastello", ["Color.SkyBlue"] = "Azzurro", ["Color.Mint"] = "Verde menta",
            ["Color.Lavender"] = "Lavanda", ["Color.Peach"] = "Pesca", ["Color.SoftYellow"] = "Giallo tenue",
            ["Color.Navy"] = "Blu navy", ["Color.Burgundy"] = "Bordeaux", ["Color.Forest"] = "Verde foresta",
            ["Color.DarkPurple"] = "Viola scuro", ["Color.Brown"] = "Marrone", ["Color.Slate"] = "Grigio ardesia",
            ["Color.White"] = "Bianco", ["Color.LightGray"] = "Grigio chiaro", ["Color.Gray"] = "Grigio",
            ["Color.DarkGray"] = "Grigio scuro", ["Color.Black"] = "Nero", ["Color.Beige"] = "Beige",
            ["Editor.Ready"] = "Trascina per annotare; fai clic per aggiungere testo. Ctrl+Z: annulla - Ctrl+S: salva - Ctrl+C: copia.",
            ["Dialog.AddText"] = "Aggiungi testo alla cattura", ["Dialog.TextPrompt"] = "Testo da scrivere sull'immagine:",
            ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "Annulla", ["Selection.Title"] = "Seleziona un'area - Esc per annullare"
        },
        ["pt"] = new()
        {
            ["Main.Title"] = "CapturaLibre 2.5.1 - edição DNET", ["Main.Tagline"] = "Capturas úteis, privadas e sem complicações.",
            ["Main.Heading"] = "O que deseja capturar?", ["Main.Language"] = "Idioma:",
            ["Capture.Region"] = "Selecionar área com o rato   [F6]", ["Capture.All"] = "Todos os ecrãs   [F7]",
            ["Capture.Primary"] = "Ecrã principal   [F8]", ["Capture.WindowSection"] = "Capturar uma janela específica",
            ["Capture.Refresh"] = "Atualizar lista", ["Capture.Window"] = "Capturar janela",
            ["Capture.Delay"] = "Atraso antes da captura:", ["Capture.Seconds"] = "segundos",
            ["Main.Restore"] = "Restaurar janelas", ["Main.Minimize"] = "Minimizar outras janelas",
            ["Main.MinimizeOnStart"] = "Minimizar outras janelas ao iniciar (sem as fechar)",
            ["Main.OpenImage"] = "Abrir imagem...", ["Dnet.Enable"] = "Ativar acionador DNET externo",
            ["Dnet.ChooseFolder"] = "Escolher pasta...", ["Dnet.Capture5"] = "Captura DNET em 5 s",
            ["Dnet.AutoCrop"] = "Ajustar automaticamente ao conteúdo DNET útil",
            ["Dnet.Folder"] = "Pasta: {0}", ["Dnet.Trigger"] = "Acionador externo sem mudar o foco: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
            ["Dnet.HelpAccessible"] = "Ajuda sobre o acionador DNET externo",
            ["Dnet.HelpTitle"] = "Ajuda do modo DNET",
            ["Dnet.HelpBody"] = "Existem duas formas de trabalhar:\n\n• Capturas normais: os botões Área, Todos os ecrãs, Ecrã principal e Capturar janela abrem sempre o editor para desenhar, escrever, recortar ou ocultar dados.\n\n• Captura DNET direta: o botão 'Captura DNET em 5 s' e o ficheiro capturar_dnet.txt guardam o PNG diretamente, sem abrir o editor nem mudar o foco.\n\nEsta caixa apenas ativa ou desativa o acionador DNET externo. Não altera o funcionamento dos botões normais.",
            ["Main.Help"] = "F6: área - F7: todos os ecrãs - F8: ecrã principal. Pode editar, recortar, rodar e ocultar dados. As capturas não são enviadas para a Internet.",
            ["Main.Ready"] = "Pronto para capturar. Tudo é processado no seu computador.",
            ["Editor.Title"] = "Editar captura - {0} x {1} - CapturaLibre",
            ["Tool.Arrow"] = "Seta", ["Tool.Rectangle"] = "Retângulo", ["Tool.Ellipse"] = "Elipse",
            ["Tool.Pencil"] = "Lápis", ["Tool.Text"] = "Texto", ["Tool.Redact"] = "Ocultar dados",
            ["Tool.Crop"] = "Recortar", ["Tool.Thickness"] = "Espessura:",
            ["Action.Undo"] = "Desfazer", ["Action.Rotate"] = "Rodar 90°", ["Action.Flip"] = "Inverter",
            ["Action.SaveAs"] = "Guardar como...", ["Action.QuickSave"] = "Gravação rápida",
            ["Action.Copy"] = "Copiar imagem", ["Action.OpenFolder"] = "Abrir pasta",
            ["Color.Label"] = "Cor:", ["Color.More"] = "Mais cores...",
            ["Color.Vivid"] = "Cores vivas", ["Color.Pastel"] = "Tons pastel", ["Color.Dark"] = "Cores escuras",
            ["Color.Neutral"] = "Cores neutras", ["Color.Any"] = "Escolher qualquer cor...",
            ["Color.Orange"] = "Laranja", ["Color.Yellow"] = "Amarelo", ["Color.Fuchsia"] = "Fúcsia",
            ["Color.Turquoise"] = "Turquesa", ["Color.Violet"] = "Violeta", ["Color.BrightGreen"] = "Verde vivo",
            ["Color.PastelPink"] = "Rosa pastel", ["Color.SkyBlue"] = "Azul-céu", ["Color.Mint"] = "Verde menta",
            ["Color.Lavender"] = "Lavanda", ["Color.Peach"] = "Pêssego", ["Color.SoftYellow"] = "Amarelo suave",
            ["Color.Navy"] = "Azul-marinho", ["Color.Burgundy"] = "Bordô", ["Color.Forest"] = "Verde-floresta",
            ["Color.DarkPurple"] = "Roxo escuro", ["Color.Brown"] = "Castanho", ["Color.Slate"] = "Cinzento ardósia",
            ["Color.White"] = "Branco", ["Color.LightGray"] = "Cinzento claro", ["Color.Gray"] = "Cinzento",
            ["Color.DarkGray"] = "Cinzento escuro", ["Color.Black"] = "Preto", ["Color.Beige"] = "Bege",
            ["Editor.Ready"] = "Arraste para anotar; clique para adicionar texto. Ctrl+Z: desfazer - Ctrl+S: guardar - Ctrl+C: copiar.",
            ["Dialog.AddText"] = "Adicionar texto à captura", ["Dialog.TextPrompt"] = "Texto a escrever na imagem:",
            ["Dialog.OK"] = "Aceitar", ["Dialog.Cancel"] = "Cancelar", ["Selection.Title"] = "Selecionar área - Esc para cancelar"
        }
    };

    public static string CurrentLanguage { get; private set; } = "es";

    public static void SetLanguage(string? code) =>
        CurrentLanguage = Texts.ContainsKey(code ?? string.Empty) ? code! : "es";

    public static string Get(string key)
    {
        if (Texts[CurrentLanguage].TryGetValue(key, out string? value)) return value;
        return Texts["es"].TryGetValue(key, out value) ? value : key;
    }

    public static T Bind<T>(T control, string key) where T : Control
    {
        control.Name = "i18n:" + key;
        control.Text = Get(key);
        return control;
    }

    public static void Apply(Control root)
    {
        foreach (Control control in root.Controls)
        {
            if (control.Name.StartsWith("i18n:", StringComparison.Ordinal))
                control.Text = Get(control.Name[5..]);
            Apply(control);
        }
    }
}
