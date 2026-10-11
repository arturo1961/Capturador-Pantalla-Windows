namespace CapturaLibre;

/// <summary>
/// Traducciones añadidas a partir de la versión posterior a 2.5.3.
/// Las claves que no necesitan una traducción propia usan el texto inglés.
/// </summary>
internal static class AdditionalTranslations
{
    public static Dictionary<string, string> Create(string language) => language switch
    {
        "nl" => Dutch(),
        "pl" => Polish(),
        "ro" => Romanian(),
        "ru" => Russian(),
        "zh-Hans" => SimplifiedChinese(),
        "ja" => Japanese(),
        "hi" => Hindi(),
        _ => new()
    };

    private static Dictionary<string, string> Dutch() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 - DNET-editie", ["Main.Tagline"] = "Handige, privé-schermafbeeldingen zonder gedoe.",
        ["Main.Heading"] = "Wat wilt u vastleggen?", ["Main.Language"] = "Taal:",
        ["Capture.Region"] = "Gebied selecteren met de muis   [F6]", ["Capture.All"] = "Alle schermen   [F7]",
        ["Capture.Primary"] = "Hoofdscherm   [F8]", ["Capture.WindowSection"] = "Een specifiek venster vastleggen",
        ["Capture.Refresh"] = "Lijst vernieuwen", ["Capture.Window"] = "Venster vastleggen",
        ["Capture.Delay"] = "Vertraging vóór opname:", ["Capture.Seconds"] = "seconden",
        ["Main.Restore"] = "Vensters herstellen", ["Main.Minimize"] = "Andere vensters minimaliseren",
        ["Main.MinimizeOnStart"] = "Andere vensters bij het starten minimaliseren (niet sluiten)",
        ["Main.OpenImage"] = "Afbeelding openen...", ["Dnet.Enable"] = "Externe DNET-trigger inschakelen",
        ["Dnet.ChooseFolder"] = "Map kiezen...", ["Dnet.Capture5"] = "DNET-opname over 5 s",
        ["Dnet.AutoCrop"] = "Automatisch aanpassen aan bruikbare DNET-inhoud",
        ["Dnet.Folder"] = "Opslagmap geselecteerd. Gebruik 'Map kiezen...' om deze te wijzigen.",
        ["Dnet.Trigger"] = "Externe trigger zonder focuswijziging: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "Hulp voor de externe DNET-trigger", ["Dnet.HelpTitle"] = "Hulp voor DNET-modus",
        ["Dnet.HelpBody"] = "Er zijn twee werkwijzen:\n\n• Normale opnamen: de knoppen Gebied, Alle schermen, Hoofdscherm en Venster vastleggen openen altijd de editor om te tekenen, schrijven, bij te snijden of gegevens te verbergen.\n\n• Directe DNET-opname: de knop 'DNET-opname over 5 s' en het bestand capturar_dnet.txt slaan de PNG rechtstreeks op, zonder de editor te openen of de focus te wijzigen.\n\nDit selectievakje schakelt alleen de externe DNET-trigger in of uit. De normale knoppen blijven ongewijzigd werken.",
        ["Main.Help"] = "F6: gebied - F7: alle schermen - F8: hoofdscherm. U kunt bewerken, bijsnijden, draaien en gegevens verbergen. Opnamen worden niet naar internet verzonden.",
        ["Main.Ready"] = "Klaar om vast te leggen. Alles wordt op uw computer verwerkt.",
        ["Editor.Title"] = "Opname bewerken - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "Pijl", ["Tool.Rectangle"] = "Rechthoek", ["Tool.Ellipse"] = "Ellips", ["Tool.Pencil"] = "Potlood",
        ["Tool.Text"] = "Tekst", ["Tool.Redact"] = "Gegevens verbergen", ["Tool.Crop"] = "Bijsnijden", ["Tool.Thickness"] = "Dikte:",
        ["Action.Undo"] = "Ongedaan maken", ["Action.Rotate"] = "90° draaien", ["Action.Flip"] = "Spiegelen",
        ["Action.SaveAs"] = "Opslaan als...", ["Action.QuickSave"] = "Snel opslaan", ["Action.Copy"] = "Afbeelding kopiëren",
        ["Action.OpenFolder"] = "Map openen", ["Color.Label"] = "Kleur:", ["Color.More"] = "Meer kleuren...",
        ["Editor.Ready"] = "Sleep om te annoteren; klik om tekst toe te voegen. Ctrl+Z: ongedaan maken - Ctrl+S: opslaan - Ctrl+C: kopiëren.",
        ["Dialog.AddText"] = "Tekst aan de opname toevoegen", ["Dialog.TextPrompt"] = "Tekst voor op de afbeelding:",
        ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "Annuleren", ["Selection.Title"] = "Gebied selecteren - Esc om te annuleren"
    };

    private static Dictionary<string, string> Polish() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 - edycja DNET", ["Main.Tagline"] = "Przydatne, prywatne zrzuty ekranu bez komplikacji.",
        ["Main.Heading"] = "Co chcesz przechwycić?", ["Main.Language"] = "Język:",
        ["Capture.Region"] = "Wybierz obszar myszą   [F6]", ["Capture.All"] = "Wszystkie ekrany   [F7]",
        ["Capture.Primary"] = "Ekran główny   [F8]", ["Capture.WindowSection"] = "Przechwyć wybrane okno",
        ["Capture.Refresh"] = "Odśwież listę", ["Capture.Window"] = "Przechwyć okno",
        ["Capture.Delay"] = "Opóźnienie przed przechwyceniem:", ["Capture.Seconds"] = "sekund",
        ["Main.Restore"] = "Przywróć okna", ["Main.Minimize"] = "Zminimalizuj inne okna",
        ["Main.MinimizeOnStart"] = "Zminimalizuj inne okna przy uruchomieniu (bez zamykania)",
        ["Main.OpenImage"] = "Otwórz obraz...", ["Dnet.Enable"] = "Włącz zewnętrzny wyzwalacz DNET",
        ["Dnet.ChooseFolder"] = "Wybierz folder...", ["Dnet.Capture5"] = "Zrzut DNET za 5 s",
        ["Dnet.AutoCrop"] = "Automatycznie dopasuj do użytecznej zawartości DNET",
        ["Dnet.Folder"] = "Wybrano folder zapisu. Użyj „Wybierz folder...”, aby go zmienić.",
        ["Dnet.Trigger"] = "Zewnętrzny wyzwalacz bez zmiany fokusu: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "Pomoc wyzwalacza zewnętrznego DNET", ["Dnet.HelpTitle"] = "Pomoc trybu DNET",
        ["Dnet.HelpBody"] = "Dostępne są dwa sposoby pracy:\n\n• Zwykłe zrzuty: przyciski Obszar, Wszystkie ekrany, Ekran główny i Przechwyć okno zawsze otwierają edytor, w którym można rysować, pisać, kadrować lub ukrywać dane.\n\n• Bezpośredni zrzut DNET: przycisk „Zrzut DNET za 5 s” i plik capturar_dnet.txt zapisują PNG bezpośrednio, bez otwierania edytora i zmiany fokusu.\n\nTo pole tylko włącza lub wyłącza zewnętrzny wyzwalacz DNET. Nie zmienia działania zwykłych przycisków.",
        ["Main.Help"] = "F6: obszar - F7: wszystkie ekrany - F8: ekran główny. Możesz edytować, kadrować, obracać i ukrywać dane. Zrzuty nie są wysyłane do Internetu.",
        ["Main.Ready"] = "Gotowe do przechwytywania. Wszystko jest przetwarzane na Twoim komputerze.",
        ["Editor.Title"] = "Edytuj zrzut - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "Strzałka", ["Tool.Rectangle"] = "Prostokąt", ["Tool.Ellipse"] = "Elipsa", ["Tool.Pencil"] = "Ołówek",
        ["Tool.Text"] = "Tekst", ["Tool.Redact"] = "Ukryj dane", ["Tool.Crop"] = "Przytnij", ["Tool.Thickness"] = "Grubość:",
        ["Action.Undo"] = "Cofnij", ["Action.Rotate"] = "Obróć o 90°", ["Action.Flip"] = "Odbij",
        ["Action.SaveAs"] = "Zapisz jako...", ["Action.QuickSave"] = "Szybki zapis", ["Action.Copy"] = "Kopiuj obraz",
        ["Action.OpenFolder"] = "Otwórz folder", ["Color.Label"] = "Kolor:", ["Color.More"] = "Więcej kolorów...",
        ["Editor.Ready"] = "Przeciągnij, aby opisać; kliknij, aby dodać tekst. Ctrl+Z: cofnij - Ctrl+S: zapisz - Ctrl+C: kopiuj.",
        ["Dialog.AddText"] = "Dodaj tekst do zrzutu", ["Dialog.TextPrompt"] = "Tekst do umieszczenia na obrazie:",
        ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "Anuluj", ["Selection.Title"] = "Wybierz obszar - Esc, aby anulować"
    };

    private static Dictionary<string, string> Romanian() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 - ediția DNET", ["Main.Tagline"] = "Capturi de ecran utile, private și fără complicații.",
        ["Main.Heading"] = "Ce doriți să capturați?", ["Main.Language"] = "Limbă:",
        ["Capture.Region"] = "Selectați o zonă cu mouse-ul   [F6]", ["Capture.All"] = "Toate ecranele   [F7]",
        ["Capture.Primary"] = "Ecranul principal   [F8]", ["Capture.WindowSection"] = "Capturați o anumită fereastră",
        ["Capture.Refresh"] = "Actualizați lista", ["Capture.Window"] = "Capturați fereastra",
        ["Capture.Delay"] = "Întârziere înainte de captură:", ["Capture.Seconds"] = "secunde",
        ["Main.Restore"] = "Restabiliți ferestrele", ["Main.Minimize"] = "Minimizați celelalte ferestre",
        ["Main.MinimizeOnStart"] = "Minimizați celelalte ferestre la pornire (fără a le închide)",
        ["Main.OpenImage"] = "Deschideți imaginea...", ["Dnet.Enable"] = "Activați declanșatorul DNET extern",
        ["Dnet.ChooseFolder"] = "Alegeți folderul...", ["Dnet.Capture5"] = "Captură DNET în 5 s",
        ["Dnet.AutoCrop"] = "Ajustare automată la conținutul DNET util",
        ["Dnet.Folder"] = "Folder de salvare selectat. Folosiți „Alegeți folderul...” pentru a-l schimba.",
        ["Dnet.Trigger"] = "Declanșator extern fără schimbarea focalizării: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "Ajutor pentru declanșatorul DNET extern", ["Dnet.HelpTitle"] = "Ajutor pentru modul DNET",
        ["Dnet.HelpBody"] = "Există două moduri de lucru:\n\n• Capturi normale: butoanele Zonă, Toate ecranele, Ecran principal și Capturați fereastra deschid întotdeauna editorul pentru desenare, scriere, decupare sau ascunderea datelor.\n\n• Captură DNET directă: butonul „Captură DNET în 5 s” și fișierul capturar_dnet.txt salvează direct fișierul PNG, fără a deschide editorul sau a schimba focalizarea.\n\nAceastă casetă doar activează sau dezactivează declanșatorul DNET extern. Nu modifică funcționarea butoanelor normale.",
        ["Main.Help"] = "F6: zonă - F7: toate ecranele - F8: ecran principal. Puteți edita, decupa, roti și ascunde date. Capturile nu sunt trimise pe internet.",
        ["Main.Ready"] = "Gata pentru captură. Totul este procesat pe computerul dvs.",
        ["Editor.Title"] = "Editare captură - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "Săgeată", ["Tool.Rectangle"] = "Dreptunghi", ["Tool.Ellipse"] = "Elipsă", ["Tool.Pencil"] = "Creion",
        ["Tool.Text"] = "Text", ["Tool.Redact"] = "Ascundeți datele", ["Tool.Crop"] = "Decupare", ["Tool.Thickness"] = "Grosime:",
        ["Action.Undo"] = "Anulare", ["Action.Rotate"] = "Rotire 90°", ["Action.Flip"] = "Oglindire",
        ["Action.SaveAs"] = "Salvare ca...", ["Action.QuickSave"] = "Salvare rapidă", ["Action.Copy"] = "Copiere imagine",
        ["Action.OpenFolder"] = "Deschidere folder", ["Color.Label"] = "Culoare:", ["Color.More"] = "Mai multe culori...",
        ["Editor.Ready"] = "Glisați pentru adnotare; faceți clic pentru text. Ctrl+Z: anulare - Ctrl+S: salvare - Ctrl+C: copiere.",
        ["Dialog.AddText"] = "Adăugați text la captură", ["Dialog.TextPrompt"] = "Text de scris pe imagine:",
        ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "Anulare", ["Selection.Title"] = "Selectați zona - Esc pentru anulare"
    };

    private static Dictionary<string, string> Russian() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 — версия DNET", ["Main.Tagline"] = "Полезные и конфиденциальные снимки экрана без лишних сложностей.",
        ["Main.Heading"] = "Что вы хотите захватить?", ["Main.Language"] = "Язык:",
        ["Capture.Region"] = "Выбрать область мышью   [F6]", ["Capture.All"] = "Все экраны   [F7]",
        ["Capture.Primary"] = "Основной экран   [F8]", ["Capture.WindowSection"] = "Захватить определённое окно",
        ["Capture.Refresh"] = "Обновить список", ["Capture.Window"] = "Захватить окно",
        ["Capture.Delay"] = "Задержка перед захватом:", ["Capture.Seconds"] = "секунд",
        ["Main.Restore"] = "Восстановить окна", ["Main.Minimize"] = "Свернуть другие окна",
        ["Main.MinimizeOnStart"] = "Свернуть другие окна при запуске (не закрывать)",
        ["Main.OpenImage"] = "Открыть изображение...", ["Dnet.Enable"] = "Включить внешний триггер DNET",
        ["Dnet.ChooseFolder"] = "Выбрать папку...", ["Dnet.Capture5"] = "Снимок DNET через 5 с",
        ["Dnet.AutoCrop"] = "Автоматически подогнать под полезное содержимое DNET",
        ["Dnet.Folder"] = "Папка сохранения выбрана. Используйте «Выбрать папку...», чтобы изменить её.",
        ["Dnet.Trigger"] = "Внешний триггер без смены фокуса: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "Справка по внешнему триггеру DNET", ["Dnet.HelpTitle"] = "Справка режима DNET",
        ["Dnet.HelpBody"] = "Есть два способа работы:\n\n• Обычные снимки: кнопки «Область», «Все экраны», «Основной экран» и «Захватить окно» всегда открывают редактор, где можно рисовать, писать, обрезать или скрывать данные.\n\n• Прямой снимок DNET: кнопка «Снимок DNET через 5 с» и файл capturar_dnet.txt сохраняют PNG напрямую, не открывая редактор и не меняя фокус.\n\nЭтот флажок только включает или выключает внешний триггер DNET. Он не меняет работу обычных кнопок.",
        ["Main.Help"] = "F6: область - F7: все экраны - F8: основной экран. Можно редактировать, обрезать, поворачивать и скрывать данные. Снимки не отправляются в интернет.",
        ["Main.Ready"] = "Готово к захвату. Всё обрабатывается на вашем компьютере.",
        ["Editor.Title"] = "Редактирование снимка - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "Стрелка", ["Tool.Rectangle"] = "Прямоугольник", ["Tool.Ellipse"] = "Эллипс", ["Tool.Pencil"] = "Карандаш",
        ["Tool.Text"] = "Текст", ["Tool.Redact"] = "Скрыть данные", ["Tool.Crop"] = "Обрезать", ["Tool.Thickness"] = "Толщина:",
        ["Action.Undo"] = "Отменить", ["Action.Rotate"] = "Повернуть на 90°", ["Action.Flip"] = "Отразить",
        ["Action.SaveAs"] = "Сохранить как...", ["Action.QuickSave"] = "Быстрое сохранение", ["Action.Copy"] = "Копировать изображение",
        ["Action.OpenFolder"] = "Открыть папку", ["Color.Label"] = "Цвет:", ["Color.More"] = "Другие цвета...",
        ["Editor.Ready"] = "Перетащите для пометки; щёлкните для текста. Ctrl+Z: отменить - Ctrl+S: сохранить - Ctrl+C: копировать.",
        ["Dialog.AddText"] = "Добавить текст на снимок", ["Dialog.TextPrompt"] = "Текст для изображения:",
        ["Dialog.OK"] = "ОК", ["Dialog.Cancel"] = "Отмена", ["Selection.Title"] = "Выберите область - Esc для отмены"
    };

    private static Dictionary<string, string> SimplifiedChinese() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 - DNET 版", ["Main.Tagline"] = "简单、实用且保护隐私的屏幕截图。",
        ["Main.Heading"] = "您想截取什么？", ["Main.Language"] = "语言：",
        ["Capture.Region"] = "用鼠标选择区域   [F6]", ["Capture.All"] = "所有屏幕   [F7]",
        ["Capture.Primary"] = "主屏幕   [F8]", ["Capture.WindowSection"] = "截取指定窗口",
        ["Capture.Refresh"] = "刷新列表", ["Capture.Window"] = "截取窗口",
        ["Capture.Delay"] = "截图前延迟：", ["Capture.Seconds"] = "秒",
        ["Main.Restore"] = "恢复窗口", ["Main.Minimize"] = "最小化其他窗口",
        ["Main.MinimizeOnStart"] = "启动时最小化其他窗口（不关闭）",
        ["Main.OpenImage"] = "打开图片...", ["Dnet.Enable"] = "启用外部 DNET 触发器",
        ["Dnet.ChooseFolder"] = "选择文件夹...", ["Dnet.Capture5"] = "5 秒后截取 DNET",
        ["Dnet.AutoCrop"] = "自动适应有效的 DNET 内容",
        ["Dnet.Folder"] = "已选择保存文件夹。使用“选择文件夹...”进行更改。",
        ["Dnet.Trigger"] = "无需切换焦点的外部触发器：%LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "外部 DNET 触发器帮助", ["Dnet.HelpTitle"] = "DNET 模式帮助",
        ["Dnet.HelpBody"] = "有两种工作方式：\n\n• 普通截图：“区域”“所有屏幕”“主屏幕”和“截取窗口”按钮始终打开编辑器，可绘图、添加文字、裁剪或隐藏数据。\n\n• 直接 DNET 截图：“5 秒后截取 DNET”按钮和 capturar_dnet.txt 文件会直接保存 PNG，不打开编辑器，也不切换焦点。\n\n此复选框仅用于启用或禁用外部 DNET 触发器，不会改变普通按钮的工作方式。",
        ["Main.Help"] = "F6：区域 - F7：所有屏幕 - F8：主屏幕。您可以编辑、裁剪、旋转和隐藏数据。截图不会发送到互联网。",
        ["Main.Ready"] = "已准备好截图。所有处理都在您的计算机上完成。",
        ["Editor.Title"] = "编辑截图 - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "箭头", ["Tool.Rectangle"] = "矩形", ["Tool.Ellipse"] = "椭圆", ["Tool.Pencil"] = "铅笔",
        ["Tool.Text"] = "文字", ["Tool.Redact"] = "隐藏数据", ["Tool.Crop"] = "裁剪", ["Tool.Thickness"] = "粗细：",
        ["Action.Undo"] = "撤销", ["Action.Rotate"] = "旋转 90°", ["Action.Flip"] = "翻转",
        ["Action.SaveAs"] = "另存为...", ["Action.QuickSave"] = "快速保存", ["Action.Copy"] = "复制图片",
        ["Action.OpenFolder"] = "打开文件夹", ["Color.Label"] = "颜色：", ["Color.More"] = "更多颜色...",
        ["Editor.Ready"] = "拖动以标注；单击以添加文字。Ctrl+Z：撤销 - Ctrl+S：保存 - Ctrl+C：复制。",
        ["Dialog.AddText"] = "向截图添加文字", ["Dialog.TextPrompt"] = "要写在图片上的文字：",
        ["Dialog.OK"] = "确定", ["Dialog.Cancel"] = "取消", ["Selection.Title"] = "选择区域 - 按 Esc 取消"
    };

    private static Dictionary<string, string> Japanese() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 - DNET版", ["Main.Tagline"] = "簡単で便利、プライバシーを守るスクリーンショット。",
        ["Main.Heading"] = "何をキャプチャしますか？", ["Main.Language"] = "言語：",
        ["Capture.Region"] = "マウスで範囲を選択   [F6]", ["Capture.All"] = "すべての画面   [F7]",
        ["Capture.Primary"] = "メイン画面   [F8]", ["Capture.WindowSection"] = "特定のウィンドウをキャプチャ",
        ["Capture.Refresh"] = "一覧を更新", ["Capture.Window"] = "ウィンドウをキャプチャ",
        ["Capture.Delay"] = "キャプチャまでの待ち時間：", ["Capture.Seconds"] = "秒",
        ["Main.Restore"] = "ウィンドウを復元", ["Main.Minimize"] = "他のウィンドウを最小化",
        ["Main.MinimizeOnStart"] = "起動時に他のウィンドウを最小化（閉じない）",
        ["Main.OpenImage"] = "画像を開く...", ["Dnet.Enable"] = "外部DNETトリガーを有効化",
        ["Dnet.ChooseFolder"] = "フォルダーを選択...", ["Dnet.Capture5"] = "5秒後にDNETをキャプチャ",
        ["Dnet.AutoCrop"] = "有効なDNETコンテンツに自動調整",
        ["Dnet.Folder"] = "保存フォルダーを選択しました。「フォルダーを選択...」で変更できます。",
        ["Dnet.Trigger"] = "フォーカスを変更しない外部トリガー：%LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "外部DNETトリガーのヘルプ", ["Dnet.HelpTitle"] = "DNETモードのヘルプ",
        ["Dnet.HelpBody"] = "2つの使い方があります。\n\n• 通常のキャプチャ：「範囲」「すべての画面」「メイン画面」「ウィンドウをキャプチャ」ボタンは常にエディターを開き、描画、文字入力、切り抜き、データの非表示ができます。\n\n• DNET直接キャプチャ：「5秒後にDNETをキャプチャ」ボタンと capturar_dnet.txt ファイルは、エディターを開かずフォーカスも変更せずにPNGを直接保存します。\n\nこのチェックボックスは外部DNETトリガーの有効／無効だけを切り替えます。通常のボタンの動作は変わりません。",
        ["Main.Help"] = "F6：範囲 - F7：すべての画面 - F8：メイン画面。編集、切り抜き、回転、データの非表示ができます。画像はインターネットへ送信されません。",
        ["Main.Ready"] = "キャプチャの準備ができました。すべての処理はこのコンピューター上で行われます。",
        ["Editor.Title"] = "キャプチャを編集 - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "矢印", ["Tool.Rectangle"] = "四角形", ["Tool.Ellipse"] = "楕円", ["Tool.Pencil"] = "鉛筆",
        ["Tool.Text"] = "文字", ["Tool.Redact"] = "データを隠す", ["Tool.Crop"] = "切り抜き", ["Tool.Thickness"] = "太さ：",
        ["Action.Undo"] = "元に戻す", ["Action.Rotate"] = "90°回転", ["Action.Flip"] = "反転",
        ["Action.SaveAs"] = "名前を付けて保存...", ["Action.QuickSave"] = "クイック保存", ["Action.Copy"] = "画像をコピー",
        ["Action.OpenFolder"] = "フォルダーを開く", ["Color.Label"] = "色：", ["Color.More"] = "その他の色...",
        ["Editor.Ready"] = "ドラッグして注釈、クリックして文字を追加。Ctrl+Z：元に戻す - Ctrl+S：保存 - Ctrl+C：コピー。",
        ["Dialog.AddText"] = "キャプチャに文字を追加", ["Dialog.TextPrompt"] = "画像に書き込む文字：",
        ["Dialog.OK"] = "OK", ["Dialog.Cancel"] = "キャンセル", ["Selection.Title"] = "範囲を選択 - Escでキャンセル"
    };

    private static Dictionary<string, string> Hindi() => new()
    {
        ["Main.Title"] = "CapturaLibre 2.5.3 - DNET संस्करण", ["Main.Tagline"] = "आसान, उपयोगी और निजी स्क्रीनशॉट।",
        ["Main.Heading"] = "आप क्या कैप्चर करना चाहते हैं?", ["Main.Language"] = "भाषा:",
        ["Capture.Region"] = "माउस से क्षेत्र चुनें   [F6]", ["Capture.All"] = "सभी स्क्रीन   [F7]",
        ["Capture.Primary"] = "मुख्य स्क्रीन   [F8]", ["Capture.WindowSection"] = "किसी विशेष विंडो को कैप्चर करें",
        ["Capture.Refresh"] = "सूची रीफ़्रेश करें", ["Capture.Window"] = "विंडो कैप्चर करें",
        ["Capture.Delay"] = "कैप्चर से पहले विलंब:", ["Capture.Seconds"] = "सेकंड",
        ["Main.Restore"] = "विंडो पुनर्स्थापित करें", ["Main.Minimize"] = "अन्य विंडो छोटी करें",
        ["Main.MinimizeOnStart"] = "शुरू होने पर अन्य विंडो छोटी करें (बंद न करें)",
        ["Main.OpenImage"] = "चित्र खोलें...", ["Dnet.Enable"] = "बाहरी DNET ट्रिगर चालू करें",
        ["Dnet.ChooseFolder"] = "फ़ोल्डर चुनें...", ["Dnet.Capture5"] = "5 सेकंड में DNET कैप्चर",
        ["Dnet.AutoCrop"] = "उपयोगी DNET सामग्री के अनुसार अपने-आप समायोजित करें",
        ["Dnet.Folder"] = "सहेजने का फ़ोल्डर चुना गया है। बदलने के लिए ‘फ़ोल्डर चुनें...’ का उपयोग करें।",
        ["Dnet.Trigger"] = "फ़ोकस बदले बिना बाहरी ट्रिगर: %LOCALAPPDATA%\\CapturaLibre_DNET\\capturar_dnet.txt",
        ["Dnet.HelpAccessible"] = "बाहरी DNET ट्रिगर की सहायता", ["Dnet.HelpTitle"] = "DNET मोड सहायता",
        ["Dnet.HelpBody"] = "काम करने के दो तरीके हैं:\n\n• सामान्य कैप्चर: क्षेत्र, सभी स्क्रीन, मुख्य स्क्रीन और विंडो कैप्चर बटन हमेशा संपादक खोलते हैं, जहाँ आप चित्र बना सकते हैं, लिख सकते हैं, काट सकते हैं या डेटा छिपा सकते हैं।\n\n• सीधा DNET कैप्चर: ‘5 सेकंड में DNET कैप्चर’ बटन और capturar_dnet.txt फ़ाइल संपादक खोले या फ़ोकस बदले बिना PNG को सीधे सहेजते हैं।\n\nयह चेकबॉक्स केवल बाहरी DNET ट्रिगर को चालू या बंद करता है। यह सामान्य बटनों के काम को नहीं बदलता।",
        ["Main.Help"] = "F6: क्षेत्र - F7: सभी स्क्रीन - F8: मुख्य स्क्रीन। आप संपादित, क्रॉप, घुमा और डेटा छिपा सकते हैं। स्क्रीनशॉट इंटरनेट पर नहीं भेजे जाते।",
        ["Main.Ready"] = "कैप्चर के लिए तैयार। सब कुछ आपके कंप्यूटर पर संसाधित होता है।",
        ["Editor.Title"] = "कैप्चर संपादित करें - {0} x {1} - CapturaLibre",
        ["Tool.Arrow"] = "तीर", ["Tool.Rectangle"] = "आयत", ["Tool.Ellipse"] = "दीर्घवृत्त", ["Tool.Pencil"] = "पेंसिल",
        ["Tool.Text"] = "पाठ", ["Tool.Redact"] = "डेटा छिपाएँ", ["Tool.Crop"] = "क्रॉप", ["Tool.Thickness"] = "मोटाई:",
        ["Action.Undo"] = "पूर्ववत", ["Action.Rotate"] = "90° घुमाएँ", ["Action.Flip"] = "पलटें",
        ["Action.SaveAs"] = "इस रूप में सहेजें...", ["Action.QuickSave"] = "त्वरित सहेजें", ["Action.Copy"] = "चित्र कॉपी करें",
        ["Action.OpenFolder"] = "फ़ोल्डर खोलें", ["Color.Label"] = "रंग:", ["Color.More"] = "और रंग...",
        ["Editor.Ready"] = "टिप्पणी के लिए खींचें; पाठ जोड़ने के लिए क्लिक करें। Ctrl+Z: पूर्ववत - Ctrl+S: सहेजें - Ctrl+C: कॉपी।",
        ["Dialog.AddText"] = "कैप्चर में पाठ जोड़ें", ["Dialog.TextPrompt"] = "चित्र पर लिखने का पाठ:",
        ["Dialog.OK"] = "ठीक", ["Dialog.Cancel"] = "रद्द करें", ["Selection.Title"] = "क्षेत्र चुनें - रद्द करने के लिए Esc"
    };
}
