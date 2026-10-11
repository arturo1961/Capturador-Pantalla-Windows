namespace CapturaLibre;

internal enum CaptureMode { Region, AllScreens, PrimaryScreen, Window }

internal sealed class MainForm : Form
{
    private readonly WindowManager _windows = new();
    private readonly AppSettings _settings = AppSettings.Load();
    private readonly ComboBox _delay = new();
    private readonly ComboBox _windowList = new();
    private readonly Label _status = new();
    private readonly ComboBox _language = new();
    private readonly ComboBox _preset = new();
    private readonly List<Button> _captureButtons = new();
    private readonly System.Windows.Forms.Timer _dnetTriggerTimer = new() { Interval = 350 };
    private readonly Label _dnetFolderLabel = new();
    private bool _busy;

    private static readonly Color Navy = Color.FromArgb(29, 41, 59);
    private static readonly Color Blue = Color.FromArgb(30, 113, 205);
    private static readonly Color Light = Color.FromArgb(247, 249, 252);

    public MainForm()
    {
        UiText.SetLanguage(_settings.Language);
        ApplyLanguageDirection();
        Text = VersionedWindowTitle();
        Icon = Program.AppIcon;
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1050, 830);
        MinimumSize = new Size(850, 730);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 11f);
        BackColor = Light;
        KeyPreview = true;

        var header = new Panel { Dock = DockStyle.Top, Height = 125, BackColor = Navy };
        header.Controls.Add(UiText.Bind(new Label
        {
            Text = "CapturaLibre", ForeColor = Color.White,
            Font = new Font("Segoe UI", 29, FontStyle.Bold),
            Dock = DockStyle.Top, Height = 77,
            Padding = new Padding(28, 12, 0, 0)
        }, "Brand"));
        header.Controls.Add(UiText.Bind(new Label
        {
            ForeColor = Color.FromArgb(221, 230, 242),
            Dock = DockStyle.Bottom, Height = 46,
            Padding = new Padding(32, 0, 0, 13)
        }, "Main.Tagline"));

        var languagePanel = new FlowLayoutPanel
        {
            Size = new Size(390, 58),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            FlowDirection = FlowDirection.LeftToRight, WrapContents = false,
            Padding = new Padding(0, 15, 14, 0), BackColor = Navy
        };
        languagePanel.Controls.Add(UiText.Bind(new Label
        {
            AutoSize = true, ForeColor = Color.White,
            Padding = new Padding(0, 7, 4, 0)
        }, "Main.Language"));
        _language.DropDownStyle = ComboBoxStyle.DropDownList;
        _language.Width = 250;
        _language.DropDownWidth = 280;
        _language.Items.AddRange(UiText.Languages);
        _language.SelectedIndex = Array.FindIndex(UiText.Languages,
            option => option.Code == UiText.CurrentLanguage);
        _language.SelectedIndexChanged += (_, _) => ChangeLanguage();
        languagePanel.Controls.Add(_language);
        header.Controls.Add(languagePanel);
        void PositionLanguagePanel() => languagePanel.Location =
            new Point(Math.Max(0, header.ClientSize.Width - languagePanel.Width - 12), 7);
        header.Resize += (_, _) => PositionLanguagePanel();
        PositionLanguagePanel();
        languagePanel.BringToFront();

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 10,
            Padding = new Padding(28, 10, 28, 12),
            BackColor = Light, AutoScroll = true
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        foreach (float pixels in new float[] { 52, 67, 66, 103, 60, 66, 58, 168 })
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, pixels));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

        var heading = UiText.Bind(new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Navy,
            TextAlign = ContentAlignment.MiddleLeft
        }, "Main.Heading");
        Place(content, heading, 0, 0, 2);

        Button region = NewButton("Capture.Region", Blue);
        region.Click += async (_, _) => await CaptureAsync(CaptureMode.Region);
        Place(content, region, 0, 1, 2);
        _captureButtons.Add(region);

        Button all = NewButton("Capture.All", Navy);
        all.Click += async (_, _) => await CaptureAsync(CaptureMode.AllScreens);
        Place(content, all, 0, 2, 1);
        _captureButtons.Add(all);

        Button primary = NewButton("Capture.Primary", Navy);
        primary.Click += async (_, _) => await CaptureAsync(CaptureMode.PrimaryScreen);
        Place(content, primary, 1, 2, 1);
        _captureButtons.Add(primary);

        // Windows más actuales aparecen con el botón Actualizar.
        var windowRow = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2,
            Padding = new Padding(4, 2, 4, 6)
        };
        windowRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        windowRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 142));
        windowRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 209));
        windowRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        windowRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var windowTitle = UiText.Bind(new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = Navy, Font = new Font("Segoe UI", 10f, FontStyle.Bold)
        }, "Capture.WindowSection");
        Place(windowRow, windowTitle, 0, 0, 3);
        _windowList.DropDownStyle = ComboBoxStyle.DropDownList;
        _windowList.DrawMode = DrawMode.OwnerDrawFixed;
        _windowList.ItemHeight = 52;
        _windowList.DropDownHeight = 420;
        _windowList.DrawItem += DrawWindowItem;
        _windowList.Dock = DockStyle.Fill;
        _windowList.Margin = new Padding(0, 6, 5, 8);
        windowRow.Controls.Add(_windowList, 0, 1);

        Button refresh = NewButton("Capture.Refresh", Navy);
        refresh.Font = new Font("Segoe UI", 10);
        refresh.Click += (_, _) => RefreshWindows();
        windowRow.Controls.Add(refresh, 1, 1);

        Button captureWindow = NewButton("Capture.Window", Blue);
        captureWindow.Font = new Font("Segoe UI", 10);
        captureWindow.Click += async (_, _) => await CaptureAsync(CaptureMode.Window);
        windowRow.Controls.Add(captureWindow, 2, 1);
        _captureButtons.Add(captureWindow);
        Place(content, windowRow, 0, 3, 2);

        var delayRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(3, 10, 0, 0)
        };
        delayRow.Controls.Add(UiText.Bind(new Label
        {
            AutoSize = true, Padding = new Padding(0, 7, 0, 0)
        }, "Capture.Delay"));
        _delay.DropDownStyle = ComboBoxStyle.DropDownList;
        _delay.Width = 95;
        _delay.Items.AddRange(["0", "3", "5", "10"]);
        _delay.SelectedIndex = 0;
        delayRow.Controls.Add(_delay);
        delayRow.Controls.Add(UiText.Bind(new Label
        {
            AutoSize = true, Padding = new Padding(6, 7, 0, 0)
        }, "Capture.Seconds"));
        delayRow.Controls.Add(UiText.Bind(new Label
        {
            AutoSize = true, Padding = new Padding(24, 7, 0, 0)
        }, "Main.Preset"));
        _preset.DropDownStyle = ComboBoxStyle.DropDownList;
        _preset.Width = 185;
        RefreshPresetItems();
        _preset.SelectedIndexChanged += (_, _) =>
        {
            _settings.CapturePreset = _preset.SelectedIndex switch
            {
                1 => "Documentación",
                2 => "Privacidad",
                3 => "Rápida",
                _ => "Manual"
            };
            _settings.Save();
        };
        delayRow.Controls.Add(_preset);
        Place(content, delayRow, 0, 4, 2);

        Button restore = NewButton("Main.Restore", Navy);
        restore.Click += (_, _) =>
        {
            int count = _windows.RestoreOthers();
            _status.Text = string.Format(UiText.Get("Status.WindowsRestored"), count);
        };
        Place(content, restore, 0, 5, 1);

        Button minimize = NewButton("Main.Minimize", Navy);
        minimize.Click += (_, _) => MinimizeOtherWindows();
        Place(content, minimize, 1, 5, 1);

        var settingsRow = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            Padding = new Padding(3, 6, 0, 3)
        };
        settingsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49));
        settingsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));
        settingsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));
        settingsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));
        var minimizeAtStart = UiText.Bind(new CheckBox
        {
            Checked = _settings.MinimizeOnStart,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f),
            TextAlign = ContentAlignment.MiddleLeft
        }, "Main.MinimizeOnStart");
        minimizeAtStart.CheckedChanged += (_, _) =>
        {
            _settings.MinimizeOnStart = minimizeAtStart.Checked;
            _settings.Save();
        };
        settingsRow.Controls.Add(minimizeAtStart);
        var openImage = NewButton("Main.OpenImage", Navy);
        openImage.Font = new Font("Segoe UI", 9.5f);
        openImage.Margin = new Padding(5, 3, 5, 5);
        openImage.Click += (_, _) => OpenImage();
        settingsRow.Controls.Add(openImage, 1, 0);
        var history = NewButton("Main.History", Navy);
        history.Font = new Font("Segoe UI", 9.5f);
        history.Margin = new Padding(5, 3, 5, 5);
        history.Click += (_, _) => OpenHistory();
        settingsRow.Controls.Add(history, 2, 0);
        var preferences = NewButton("Main.Preferences", Navy);
        preferences.Font = new Font("Segoe UI", 9.5f);
        preferences.Margin = new Padding(5, 3, 5, 5);
        preferences.Click += (_, _) => ShowPreferences();
        settingsRow.Controls.Add(preferences, 3, 0);
        Place(content, settingsRow, 0, 6, 2);

        var dnetRow = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 4,
            Padding = new Padding(3, 5, 0, 5)
        };
        dnetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43));
        dnetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        dnetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        dnetRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        dnetRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        dnetRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        dnetRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        var modoDnet = UiText.Bind(new CheckBox
        {
            Checked = _settings.DnetMode,
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5f),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 8, 4, 0)
        }, "Dnet.Enable");
        modoDnet.CheckedChanged += (_, _) =>
        {
            _settings.DnetMode = modoDnet.Checked;
            _settings.Save();
            ActualizarEstadoModoDnet();
        };
        var ayudaDnet = NewButton("?", Navy, localize: false);
        ayudaDnet.AccessibleName = UiText.Get("Dnet.HelpAccessible");
        ayudaDnet.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        ayudaDnet.Size = new Size(36, 30);
        ayudaDnet.Margin = new Padding(2, 4, 0, 2);
        ayudaDnet.Click += (_, _) => ShowDnetHelp();
        var modoDnetPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        modoDnetPanel.Controls.Add(modoDnet);
        modoDnetPanel.Controls.Add(ayudaDnet);
        dnetRow.Controls.Add(modoDnetPanel, 0, 0);
        var elegirCarpetaDnet = NewButton("Dnet.ChooseFolder", Navy);
        elegirCarpetaDnet.Font = new Font("Segoe UI", 9f);
        elegirCarpetaDnet.Margin = new Padding(5, 4, 5, 5);
        elegirCarpetaDnet.Click += (_, _) => ElegirCarpetaDnet();
        dnetRow.Controls.Add(elegirCarpetaDnet, 1, 0);
        var capturaDnet = NewButton("Dnet.Capture5", Blue);
        capturaDnet.Font = new Font("Segoe UI", 9f);
        capturaDnet.Margin = new Padding(5, 4, 5, 5);
        capturaDnet.Click += async (_, _) =>
        {
            _settings.DnetMode = true;
            modoDnet.Checked = true;
            _delay.SelectedItem = "5";
            await CaptureAsync(CaptureMode.PrimaryScreen, guardarDirectamenteDnet: true);
        };
        dnetRow.Controls.Add(capturaDnet, 2, 0);
        _captureButtons.Add(capturaDnet);
        var recorteAutomaticoDnet = UiText.Bind(new CheckBox
        {
            Checked = _settings.DnetAutoCrop,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 0, 0, 0)
        }, "Dnet.AutoCrop");
        recorteAutomaticoDnet.CheckedChanged += (_, _) =>
        {
            _settings.DnetAutoCrop = recorteAutomaticoDnet.Checked;
            _settings.Save();
        };
        dnetRow.Controls.Add(recorteAutomaticoDnet, 0, 1);
        dnetRow.SetColumnSpan(recorteAutomaticoDnet, 3);
        _dnetFolderLabel.Dock = DockStyle.Fill;
        _dnetFolderLabel.AutoEllipsis = true;
        _dnetFolderLabel.TextAlign = ContentAlignment.MiddleLeft;
        _dnetFolderLabel.ForeColor = Color.FromArgb(75, 88, 105);
        dnetRow.Controls.Add(_dnetFolderLabel, 0, 2);
        dnetRow.SetColumnSpan(_dnetFolderLabel, 3);
        var instruccionesDnet = UiText.Bind(new Label
        {
            Dock = DockStyle.Fill,
            AutoEllipsis = true,
            ForeColor = Color.FromArgb(75, 88, 105),
            TextAlign = ContentAlignment.MiddleLeft
        }, "Dnet.Trigger");
        dnetRow.Controls.Add(instruccionesDnet, 0, 3);
        dnetRow.SetColumnSpan(instruccionesDnet, 3);
        Place(content, dnetRow, 0, 7, 2);

        var help = UiText.Bind(new Label
        {
            Dock = DockStyle.Fill, ForeColor = Color.FromArgb(75, 88, 105),
            Padding = new Padding(3, 12, 2, 0)
        }, "Main.Help");
        Place(content, help, 0, 8, 2);

        _status.Text = UiText.Get("Main.Ready");
        _status.ForeColor = Color.FromArgb(75, 88, 105);
        _status.Dock = DockStyle.Fill;
        _status.AutoEllipsis = true;
        _status.TextAlign = ContentAlignment.MiddleLeft;
        Place(content, _status, 0, 9, 2);

        Controls.Add(content);
        Controls.Add(header);

        KeyDown += OnKeyboardShortcut;
        _dnetTriggerTimer.Tick += ComprobarDisparoDnet;
        _dnetTriggerTimer.Start();
        ActualizarEstadoModoDnet();
        Shown += (_, _) => BeginInvoke(new Action(() =>
        {
            RefreshWindows();
            if (_settings.MinimizeOnStart) MinimizeOtherWindows();
        }));
        FormClosing += (_, _) =>
        {
            _dnetTriggerTimer.Stop();
            _windows.RestoreOthers();
            DisposeWindowChoices();
        };
    }

    private static Button NewButton(string textKey, Color bg, bool localize = true) => new AutoFitButton
    {
        Name = localize ? "i18n:" + textKey : string.Empty,
        Text = localize ? UiText.Get(textKey) : textKey,
        Dock = DockStyle.Fill,
        BackColor = bg,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 11),
        Cursor = Cursors.Hand,
        Margin = new Padding(5, 3, 5, 8),
        UseVisualStyleBackColor = false
    };

    private static string VersionedWindowTitle()
    {
        Version? assemblyVersion = typeof(MainForm).Assembly.GetName().Version;
        string version = assemblyVersion is null
            ? "2.6.0"
            : $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{Math.Max(assemblyVersion.Build, 0)}";

        // Conserva la traducción del título, pero la versión siempre procede
        // del ejecutable. Así, un paquete de idioma antiguo no puede mostrar
        // por error una versión distinta a la instalada.
        return System.Text.RegularExpressions.Regex.Replace(
            UiText.Get("Main.Title"), @"\d+\.\d+\.\d+", version);
    }

    private void ChangeLanguage()
    {
        if (_language.SelectedItem is not LanguageOption option) return;
        UiText.SetLanguage(option.Code);
        ApplyLanguageDirection();
        _settings.Language = option.Code;
        _settings.Save();
        Text = VersionedWindowTitle();
        UiText.Apply(this);
        RefreshPresetItems();
        _status.Text = UiText.Get("Main.Ready");
        ActualizarEstadoModoDnet();
    }

    private void ApplyLanguageDirection()
    {
        // La estructura visual se mantiene siempre igual en todos los idiomas.
        // El algoritmo bidireccional de Windows dibuja correctamente los textos
        // árabes, hebreos, persas y urdu sin tener que reflejar todo el formulario.
        // Invertir el formulario desplazaba la cabecera, cambiaba las columnas de
        // sitio y podía cortar la marca CapturaLibre.
        RightToLeft = RightToLeft.No;
        RightToLeftLayout = false;
    }

    private void RefreshPresetItems()
    {
        int selected = _settings.CapturePreset switch
        {
            "Documentación" => 1,
            "Privacidad" => 2,
            "Rápida" => 3,
            _ => 0
        };
        _preset.BeginUpdate();
        _preset.Items.Clear();
        _preset.Items.AddRange([
            UiText.Get("Preset.Manual"),
            UiText.Get("Preset.Documentation"),
            UiText.Get("Preset.Privacy"),
            UiText.Get("Preset.Quick")
        ]);
        _preset.SelectedIndex = selected;
        _preset.EndUpdate();
    }

    private void OpenHistory()
    {
        using var history = new HistoryForm(_settings);
        history.ShowDialog(this);
    }

    private void ShowPreferences()
    {
        using var dialog = new Form
        {
            Text = UiText.Get("Preferences.Title"), Icon = Program.AppIcon,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            ClientSize = new Size(720, 390), MinimizeBox = false, MaximizeBox = false,
            ShowInTaskbar = false, Font = new Font("Segoe UI", 10.5f),
            BackColor = Color.White
        };
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 6,
            Padding = new Padding(22), BackColor = Color.White
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        var pattern = new TextBox { Dock = DockStyle.Fill, Text = _settings.CaptureNamePattern };
        var folder = new TextBox { Dock = DockStyle.Fill, Text = _settings.QuickSaveFolder };
        var choose = new Button { Text = UiText.Get("Dnet.ChooseFolder"), Dock = DockStyle.Fill };
        choose.Click += (_, _) =>
        {
            using var browser = new FolderBrowserDialog
            {
                Description = UiText.Get("Preferences.FolderHelp"),
                SelectedPath = Directory.Exists(folder.Text) ? folder.Text : string.Empty,
                UseDescriptionForTitle = true
            };
            if (browser.ShowDialog(dialog) == DialogResult.OK) folder.Text = browser.SelectedPath;
        };
        var historyLimit = new NumericUpDown
        {
            Minimum = 5, Maximum = 100, Value = Math.Clamp(_settings.HistoryLimit, 5, 100),
            Width = 90, Dock = DockStyle.Left
        };
        var thumbnails = new CheckBox
        {
            Text = UiText.Get("Preferences.Thumbnails"), Checked = _settings.WindowThumbnails,
            AutoSize = true, Dock = DockStyle.Fill
        };
        var help = new Label
        {
            Text = UiText.Get("Preferences.PatternHelp"), Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(75, 88, 105), AutoSize = false
        };
        var accept = new Button
        {
            Text = UiText.Get("Dialog.OK"), DialogResult = DialogResult.OK,
            Dock = DockStyle.Fill, BackColor = Blue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat
        };
        var cancel = new Button
        {
            Text = UiText.Get("Dialog.Cancel"), DialogResult = DialogResult.Cancel,
            Dock = DockStyle.Fill, BackColor = Navy, ForeColor = Color.White, FlatStyle = FlatStyle.Flat
        };
        grid.Controls.Add(new Label { Text = UiText.Get("Preferences.Pattern"), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
        grid.Controls.Add(pattern, 1, 0); grid.SetColumnSpan(pattern, 2);
        grid.Controls.Add(new Label { Text = UiText.Get("Preferences.Folder"), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
        grid.Controls.Add(folder, 1, 1); grid.Controls.Add(choose, 2, 1);
        grid.Controls.Add(new Label { Text = UiText.Get("Preferences.HistoryLimit"), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
        grid.Controls.Add(historyLimit, 1, 2);
        grid.Controls.Add(thumbnails, 0, 3); grid.SetColumnSpan(thumbnails, 3);
        grid.Controls.Add(help, 0, 4); grid.SetColumnSpan(help, 3);
        grid.Controls.Add(cancel, 1, 5); grid.Controls.Add(accept, 2, 5);
        dialog.Controls.Add(grid);
        dialog.AcceptButton = accept;
        dialog.CancelButton = cancel;
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        _settings.CaptureNamePattern = pattern.Text.Trim();
        _settings.QuickSaveFolder = folder.Text.Trim();
        _settings.HistoryLimit = (int)historyLimit.Value;
        _settings.WindowThumbnails = thumbnails.Checked;
        _settings.Save();
        RefreshWindows();
    }

    private void ShowDnetHelp()
    {
        using var dialog = new Form
        {
            Text = UiText.Get("Dnet.HelpTitle"),
            Icon = Program.AppIcon,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            ClientSize = new Size(720, 500),
            MinimizeBox = false,
            MaximizeBox = false,
            ShowInTaskbar = false,
            Font = new Font("Segoe UI", 10.5f),
            BackColor = Color.White
        };

        var body = new Label
        {
            Text = UiText.Get("Dnet.HelpBody"),
            Dock = DockStyle.Fill,
            Padding = new Padding(24, 22, 24, 12),
            ForeColor = Navy
        };
        var footer = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 64,
            BackColor = Light,
            Padding = new Padding(0, 12, 18, 12)
        };
        var accept = new Button
        {
            Text = UiText.Get("Dialog.OK"),
            DialogResult = DialogResult.OK,
            Dock = DockStyle.Right,
            Width = 125,
            BackColor = Blue,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false
        };
        accept.FlatAppearance.BorderSize = 0;
        footer.Controls.Add(accept);
        dialog.Controls.Add(body);
        dialog.Controls.Add(footer);
        dialog.AcceptButton = accept;
        dialog.CancelButton = accept;
        dialog.ShowDialog(this);
    }

    private static void Place(TableLayoutPanel grid, Control control, int column, int row, int span)
    {
        grid.Controls.Add(control, column, row);
        if (span != 1) grid.SetColumnSpan(control, span);
        if (control is Button b) b.FlatAppearance.BorderSize = 0;
    }

    private void RefreshWindows()
    {
        nint selected = (_windowList.SelectedItem as WindowChoice)?.Handle ?? 0;
        _windowList.BeginUpdate();
        try
        {
            DisposeWindowChoices();
            _windowList.Items.Clear();
            foreach (var window in _windows.ListWindows(_settings.WindowThumbnails))
                _windowList.Items.Add(window);
            if (_windowList.Items.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < _windowList.Items.Count; i++)
                {
                    if ((_windowList.Items[i] as WindowChoice)?.Handle == selected) index = i;
                }
                _windowList.SelectedIndex = index;
            }
        }
        finally { _windowList.EndUpdate(); }
    }

    private void DisposeWindowChoices()
    {
        foreach (WindowChoice choice in _windowList.Items.OfType<WindowChoice>()) choice.Dispose();
    }

    private void DrawWindowItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || e.Index >= _windowList.Items.Count) return;
        if (_windowList.Items[e.Index] is not WindowChoice choice) return;
        Rectangle bounds = e.Bounds;
        int textLeft = bounds.Left + 8;
        if (choice.Thumbnail is not null)
        {
            var preview = new Rectangle(bounds.Left + 4, bounds.Top + 3, 78, Math.Max(10, bounds.Height - 6));
            e.Graphics.DrawImage(choice.Thumbnail, preview);
            textLeft = preview.Right + 10;
        }
        Color textColor = (e.State & DrawItemState.Selected) != 0
            ? SystemColors.HighlightText
            : SystemColors.ControlText;
        TextRenderer.DrawText(e.Graphics, choice.ToString(), _windowList.Font,
            new Rectangle(textLeft, bounds.Top, Math.Max(1, bounds.Right - textLeft - 4), bounds.Height),
            textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                       TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        e.DrawFocusRectangle();
    }

    private void MinimizeOtherWindows()
    {
        try
        {
            int count = _windows.MinimizeOthers();
            _status.Text = string.Format(UiText.Get("Status.WindowsMinimized"), count);
        }
        catch (Exception ex)
        {
            _status.Text = UiText.Get("Error.MinimizeWindows") + " " + ex.Message;
        }
    }

    private void OnKeyboardShortcut(object? sender, KeyEventArgs e)
    {
        if (_busy || e.Alt || e.Control || e.Shift) return;
        CaptureMode? mode = e.KeyCode switch
        {
            Keys.F6 => CaptureMode.Region,
            Keys.F7 => CaptureMode.AllScreens,
            Keys.F8 => CaptureMode.PrimaryScreen,
            _ => null
        };
        if (mode is null) return;
        e.Handled = true;
        e.SuppressKeyPress = true;
        _ = CaptureAsync(mode.Value);
    }

    private void OpenImage()
    {
        if (_busy) return;
        using var file = new OpenFileDialog
        {
            Title = UiText.Get("Dialog.OpenImageTitle"),
            Filter = UiText.Get("Dialog.ImageFilter")
        };
        if (file.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var original = new Bitmap(file.FileName);
            using var editor = new EditorForm(new Bitmap(original), _settings);
            editor.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, UiText.Get("Error.OpenImage") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task CaptureAsync(CaptureMode mode, bool guardarDirectamenteDnet = false)
    {
        if (_busy) return;
        WindowChoice? selected = null;
        if (mode == CaptureMode.Window)
        {
            selected = _windowList.SelectedItem as WindowChoice;
            if (selected is null)
            {
                MessageBox.Show(this, UiText.Get("Prompt.SelectWindow"),
                    "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        _busy = true;
        foreach (var button in _captureButtons) button.Enabled = false;
        Bitmap? shot = null;
        string? captureError = null;
        try
        {
            int seconds = int.Parse((string)_delay.SelectedItem!);
            Hide();
            if (seconds > 0)
            {
                _status.Text = string.Format(UiText.Get("Status.Countdown"), seconds);
                await Task.Delay(TimeSpan.FromSeconds(seconds));
            }
            await Task.Delay(250); // Quitar nuestra ventana de los píxeles capturados.

            Rectangle desktop = mode == CaptureMode.PrimaryScreen
                ? guardarDirectamenteDnet && _settings.DnetAutoCrop
                    ? Screen.PrimaryScreen?.WorkingArea ?? ScreenshotService.DesktopBounds
                    : Screen.PrimaryScreen?.Bounds ?? ScreenshotService.DesktopBounds
                : ScreenshotService.DesktopBounds;
            if (mode == CaptureMode.Window)
            {
                Rectangle bounds = _windows.PrepareWindowCapture(selected!.Handle);
                await Task.Delay(350); // Tiempo para restaurar/redibujar la ventana.
                shot = ScreenshotService.Take(bounds);
            }
            else if (mode == CaptureMode.Region)
            {
                using Bitmap full = ScreenshotService.Take(desktop);
                using var selector = new SelectionForm(full, desktop);
                if (selector.ShowDialog() == DialogResult.OK)
                    shot = ScreenshotService.Crop(full, selector.SelectedArea);
            }
            else
            {
                shot = ScreenshotService.Take(desktop);
            }
        }
        catch (Exception ex)
        {
            captureError = ex.Message;
        }
        finally
        {
            Show();
            Activate();
            foreach (var button in _captureButtons) button.Enabled = true;
            _status.Text = UiText.Get("Main.Ready");
            _busy = false;
        }

        if (captureError is not null)
        {
            MessageBox.Show(this, UiText.Get("Error.Capture") + "\n" + captureError,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (shot is null) return;
        CaptureHistory.Add(shot, _settings.HistoryLimit);
        // Los botones normales siempre abren el editor. El guardado directo se usa
        // solamente desde el botón DNET dedicado; el disparador externo tiene su
        // propio flujo y tampoco cambia el foco de la ventana documentada.
        if (guardarDirectamenteDnet)
        {
            try
            {
                if (_settings.DnetAutoCrop)
                {
                    Bitmap recortada = ScreenshotService.AutoCropForDnet(shot);
                    shot.Dispose();
                    shot = recortada;
                }
                string ruta = GuardarCapturaDnet(shot, null);
                _status.Text = string.Format(UiText.Get("Status.DnetSaved"), ruta);
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, UiText.Get("Error.DnetSave") + "\n" + ex.Message,
                    "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                shot.Dispose();
            }
            return;
        }
        if (_settings.CapturePreset == "Rápida")
        {
            try
            {
                string folder = GetQuickSaveFolder();
                Directory.CreateDirectory(folder);
                string path = Path.Combine(folder, _settings.NextCaptureFileName(selected?.Title));
                shot.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                _status.Text = string.Format(UiText.Get("Status.QuickSaved"), path);
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, UiText.Get("Error.QuickSave") + "\n" + ex.Message,
                    "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { shot.Dispose(); }
            return;
        }
        try
        {
            using var editor = new EditorForm(shot, _settings);
            shot = null; // A partir de aquí el editor se encarga del Bitmap.
            editor.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, UiText.Get("Error.OpenEditor") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { shot?.Dispose(); }
    }

    private string GetQuickSaveFolder()
    {
        if (!string.IsNullOrWhiteSpace(_settings.QuickSaveFolder))
            return _settings.QuickSaveFolder;
        string pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        if (string.IsNullOrWhiteSpace(pictures))
            pictures = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        return Path.Combine(pictures, "CapturaLibre");
    }

    private static string DnetTriggerPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CapturaLibre_DNET", "capturar_dnet.txt");

    private string CarpetaDnet
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_settings.DnetOutputFolder))
                return _settings.DnetOutputFolder;
            string imagenes = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (string.IsNullOrWhiteSpace(imagenes))
                imagenes = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            return Path.Combine(imagenes, "CapturaLibre", "DNET");
        }
    }

    private void ActualizarEstadoModoDnet()
    {
        _dnetFolderLabel.Text = UiText.Get("Dnet.Folder");
        _dnetFolderLabel.ForeColor = _settings.DnetMode ? Navy : Color.DimGray;
    }

    private void ElegirCarpetaDnet()
    {
        using var dialogo = new FolderBrowserDialog
        {
            Description = "Carpeta para las capturas directas del manual DNET",
            InitialDirectory = CarpetaDnet,
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true
        };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;
        _settings.DnetOutputFolder = dialogo.SelectedPath;
        _settings.Save();
        ActualizarEstadoModoDnet();
    }

    private async void ComprobarDisparoDnet(object? sender, EventArgs e)
    {
        if (!_settings.DnetMode || _busy || !File.Exists(DnetTriggerPath)) return;
        _dnetTriggerTimer.Stop();
        string nombre = string.Empty;
        try
        {
            nombre = (await File.ReadAllTextAsync(DnetTriggerPath)).Trim();
            File.Delete(DnetTriggerPath);
            await CapturarDnetDesdeDisparador(nombre);
        }
        catch (Exception ex)
        {
            _status.Text = UiText.Get("Error.DnetTrigger") + " " + ex.Message;
        }
        finally
        {
            if (!IsDisposed) _dnetTriggerTimer.Start();
        }
    }

    private async Task CapturarDnetDesdeDisparador(string nombreSolicitado)
    {
        _busy = true;
        foreach (Button button in _captureButtons) button.Enabled = false;
        bool restaurarVentana = Visible && WindowState != FormWindowState.Minimized;
        Bitmap? captura = null;
        try
        {
            if (restaurarVentana) Hide();
            await Task.Delay(250);
            Rectangle pantalla = _settings.DnetAutoCrop
                ? Screen.PrimaryScreen?.WorkingArea ?? ScreenshotService.DesktopBounds
                : Screen.PrimaryScreen?.Bounds ?? ScreenshotService.DesktopBounds;
            captura = ScreenshotService.Take(pantalla);
            if (_settings.DnetAutoCrop)
            {
                Bitmap recortada = ScreenshotService.AutoCropForDnet(captura);
                captura.Dispose();
                captura = recortada;
            }
            string ruta = GuardarCapturaDnet(captura, nombreSolicitado);
            _status.Text = string.Format(UiText.Get("Status.DnetSaved"), ruta);
            System.Media.SystemSounds.Asterisk.Play();
        }
        finally
        {
            captura?.Dispose();
            if (restaurarVentana)
            {
                Show();
                Activate();
            }
            foreach (Button button in _captureButtons) button.Enabled = true;
            _busy = false;
        }
    }

    private string GuardarCapturaDnet(Bitmap captura, string? nombreSolicitado)
    {
        string carpeta = CarpetaDnet;
        Directory.CreateDirectory(carpeta);
        string nombre = PrepararNombreDnet(nombreSolicitado);
        string ruta = Path.Combine(carpeta, nombre);
        captura.Save(ruta, System.Drawing.Imaging.ImageFormat.Png);
        return ruta;
    }

    private string PrepararNombreDnet(string? nombreSolicitado)
    {
        if (!string.IsNullOrWhiteSpace(nombreSolicitado))
        {
            string limpio = Path.GetFileNameWithoutExtension(nombreSolicitado.Trim());
            foreach (char invalido in Path.GetInvalidFileNameChars())
                limpio = limpio.Replace(invalido, '_');
            if (limpio.Length > 0) return limpio + ".png";
        }

        int siguiente = 1;
        if (Directory.Exists(CarpetaDnet))
        {
            foreach (string archivo in Directory.EnumerateFiles(CarpetaDnet, "*.png"))
            {
                string nombre = Path.GetFileNameWithoutExtension(archivo);
                if (nombre.Length >= 3 && int.TryParse(nombre[..3], out int numero))
                    siguiente = Math.Max(siguiente, numero + 1);
            }
        }
        return $"{siguiente:000}_captura_dnet_{DateTime.Now:yyyyMMdd_HHmmss}.png";
    }
}
