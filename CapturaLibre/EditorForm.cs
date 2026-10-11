using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CapturaLibre;

internal sealed class EditorForm : Form
{
    private Bitmap _image;
    private readonly LinkedList<Bitmap> _history = new();
    private readonly ImageCanvas _canvas = new();
    private readonly Label _status = new();
    private readonly NumericUpDown _thickness = new();
    private Color _ink = Color.Red;
    private readonly Panel _selectedColorPreview = new();
    private readonly ContextMenuStrip _colorPalettes = new();
    private readonly List<Image> _paletteIcons = new();
    private EditableAnnotation? _lastEditable;
    private int _nextStepNumber = 1;
    private readonly AppSettings _settings;
    private const int MaxUndo = 6;

    public EditorForm(Bitmap image, AppSettings? settings = null)
    {
        _settings = settings ?? AppSettings.Load();
        _image = image; // ownership transferred from caller
        Text = string.Format(UiText.Get("Editor.Title"), _image.Width, _image.Height);
        Icon = Program.AppIcon;
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1230, 800);
        MinimumSize = new Size(840, 570);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10);
        KeyPreview = true;
        BackColor = Color.FromArgb(247, 249, 252);

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var tools = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, AutoScroll = false,
            WrapContents = true, Padding = new Padding(12, 10, 12, 8),
            BackColor = Color.FromArgb(29, 41, 59)
        };
        AnnotationTool defaultTool = _settings.CapturePreset switch
        {
            "Documentación" => AnnotationTool.Step,
            "Privacidad" => AnnotationTool.Redact,
            _ => AnnotationTool.Arrow
        };
        AddTool(tools, "Tool.Arrow", AnnotationTool.Arrow, selected: defaultTool == AnnotationTool.Arrow);
        AddTool(tools, "Tool.Rectangle", AnnotationTool.Rectangle);
        AddTool(tools, "Tool.Ellipse", AnnotationTool.Ellipse);
        AddTool(tools, "Tool.Pencil", AnnotationTool.Pencil);
        AddTool(tools, "Tool.Text", AnnotationTool.Text);
        AddTool(tools, "Tool.Step", AnnotationTool.Step, selected: defaultTool == AnnotationTool.Step);
        AddTool(tools, "Tool.Pixelate", AnnotationTool.Pixelate);
        AddTool(tools, "Tool.Blur", AnnotationTool.Blur);
        AddTool(tools, "Tool.Redact", AnnotationTool.Redact, selected: defaultTool == AnnotationTool.Redact);
        AddTool(tools, "Tool.Crop", AnnotationTool.Crop);
        AddTool(tools, "Tool.Move", AnnotationTool.Move);
        // Mantener juntos la etiqueta y el selector al pasar a la siguiente fila.
        var thicknessGroup = new FlowLayoutPanel
        {
            AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false, Margin = new Padding(12, 3, 5, 3),
            BackColor = tools.BackColor
        };
        thicknessGroup.Controls.Add(new Label
        {
            Text = UiText.Get("Tool.Thickness"), AutoSize = true, ForeColor = Color.White,
            Padding = new Padding(0, 10, 0, 0), Margin = new Padding(0, 0, 5, 0)
        });
        _thickness.Minimum = 1;
        _thickness.Maximum = 20;
        _thickness.Value = 4;
        _thickness.Width = 64;
        _thickness.Margin = new Padding(4, 4, 0, 0);
        _thickness.ValueChanged += (_, _) => _canvas.InkWidth = (int)_thickness.Value;
        thicknessGroup.Controls.Add(_thickness);
        tools.Controls.Add(thicknessGroup);

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, AutoScroll = false,
            WrapContents = true, Padding = new Padding(12, 10, 12, 8),
            BackColor = Color.White
        };
        AddAction(actions, "Action.Undo", Undo, 105);
        AddAction(actions, "Action.Rotate", RotateRight, 118);
        AddAction(actions, "Action.Flip", FlipHorizontal, 110);
        AddAction(actions, "Action.SaveAs", SaveAs, 150);
        AddAction(actions, "Action.QuickSave", QuickSave, 150);
        AddAction(actions, "Action.Copy", CopyImage, 150);
        AddAction(actions, "Action.OpenFolder", OpenFolder, 140);
        // La paleta no se separa de su etiqueta al reducir la ventana.
        var palette = new FlowLayoutPanel
        {
            AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false, Margin = new Padding(12, 2, 5, 3),
            BackColor = actions.BackColor
        };
        palette.Controls.Add(new Label
        {
            Text = UiText.Get("Color.Label"), AutoSize = true,
            Padding = new Padding(0, 10, 3, 0), Margin = new Padding(0, 0, 3, 0)
        });
        AddColor(palette, Color.Red);
        AddColor(palette, Color.Blue);
        AddColor(palette, Color.Black);
        AddColor(palette, Color.LimeGreen);

        // Un único botón nuevo; las herramientas anteriores no cambian.
        // Se conserva todo el bloque de colores unido al ajustar el ancho.
        var moreColors = new Button
        {
            Text = UiText.Get("Color.More"), Width = 150, Height = 38,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(30, 113, 205),
            ForeColor = Color.White,
            Margin = new Padding(10, 2, 4, 2),
            Cursor = Cursors.Hand,
            AccessibleName = "Mostrar más paletas de colores"
        };
        moreColors.FlatAppearance.BorderSize = 0;
        moreColors.Click += (_, _) => _colorPalettes.Show(moreColors, new Point(0, moreColors.Height));
        palette.Controls.Add(moreColors);

        _selectedColorPreview.Width = 34;
        _selectedColorPreview.Height = 34;
        _selectedColorPreview.Margin = new Padding(6, 4, 3, 0);
        _selectedColorPreview.BorderStyle = BorderStyle.FixedSingle;
        _selectedColorPreview.BackColor = _ink;
        _selectedColorPreview.AccessibleName = "Color seleccionado";
        palette.Controls.Add(_selectedColorPreview);
        InitializeColorPalettes();
        actions.Controls.Add(palette);

        _canvas.Image = _image;
        _canvas.InkColor = _ink;
        _canvas.InkWidth = (int)_thickness.Value;
        _canvas.EditCompleted += OnEdit;

        _status.Text = UiText.Get("Editor.Ready");
        _status.TextAlign = ContentAlignment.MiddleLeft;
        _status.Dock = DockStyle.Fill;
        _status.Padding = new Padding(14, 0, 0, 0);
        _status.AutoEllipsis = true;

        root.Controls.Add(tools, 0, 0);
        root.Controls.Add(actions, 0, 1);
        root.Controls.Add(_canvas, 0, 2);
        root.Controls.Add(_status, 0, 3);
        Controls.Add(root);

        // Ajustar la altura de ambas barras al ancho real: sin recortes ni
        // barras de desplazamiento horizontal, también al cambiar el zoom DPI.
        void UpdateToolbarLayout()
        {
            int width = root.ClientSize.Width;
            if (width <= 0) return;
            int topHeight = MeasureToolbarHeight(tools, width);
            int actionHeight = MeasureToolbarHeight(actions, width);
            if (root.RowStyles[0].Height != topHeight)
                root.RowStyles[0].Height = topHeight;
            if (root.RowStyles[1].Height != actionHeight)
                root.RowStyles[1].Height = actionHeight;
        }
        root.SizeChanged += (_, _) => UpdateToolbarLayout();
        tools.SizeChanged += (_, _) => UpdateToolbarLayout();
        actions.SizeChanged += (_, _) => UpdateToolbarLayout();
        Shown += (_, _) => UpdateToolbarLayout();

        KeyDown += HandleKeys;
    }

    // Medimos los controles existentes con sus márgenes y dejamos un pequeño
    // margen de seguridad para que WinForms pueda distribuirlos en varias filas.
    private static int MeasureToolbarHeight(FlowLayoutPanel panel, int parentWidth)
    {
        int usableWidth = Math.Max(1, parentWidth - panel.Margin.Horizontal
            - panel.Padding.Horizontal - 18);
        int lineWidth = 0;
        int lineHeight = 0;
        int totalHeight = panel.Padding.Vertical + 8;

        foreach (Control item in panel.Controls)
        {
            if (!item.Visible) continue;
            int width = item.Width + item.Margin.Horizontal;
            int height = item.Height + item.Margin.Vertical;
            if (lineWidth > 0 && lineWidth + width > usableWidth)
            {
                totalHeight += lineHeight;
                lineWidth = 0;
                lineHeight = 0;
            }
            lineWidth += width;
            lineHeight = Math.Max(lineHeight, height);
        }
        return Math.Max(64, totalHeight + lineHeight);
    }

    private void AddTool(FlowLayoutPanel panel, string textKey, AnnotationTool tool, bool selected = false)
    {
        string text = UiText.Get(textKey);
        var radio = new RadioButton
        {
            Text = text, Tag = tool,
            Appearance = Appearance.Button,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = false,
            Font = Font,
            Width = Math.Max(105, TextRenderer.MeasureText(text, Font).Width + 35),
            Height = 42, Margin = new Padding(4, 3, 4, 3),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(222, 230, 241),
            ForeColor = Color.FromArgb(29, 41, 59),
            Checked = selected
        };
        radio.CheckedChanged += (_, _) =>
        {
            if (!radio.Checked) return;
            _canvas.Tool = tool;
            _status.Text = tool switch
            {
                AnnotationTool.Redact => "Ocultar datos: dibuja un rectángulo. Sus píxeles se sustituirán por negro.",
                AnnotationTool.Pixelate => "Pixelar: dibuja sobre la información que quieras hacer ilegible.",
                AnnotationTool.Blur => "Desenfocar: dibuja sobre la información que quieras difuminar.",
                AnnotationTool.Crop => "Recortar: arrastra una zona para conservar solo ese fragmento.",
                AnnotationTool.Step => $"Paso numerado: el siguiente marcador será {_nextStepNumber}.",
                AnnotationTool.Move => "Mover último: arrastra para desplazar la última flecha, forma, texto o paso.",
                _ => "Herramienta seleccionada: " + text + "."
            };
        };
        panel.Controls.Add(radio);
    }

    private static void AddAction(FlowLayoutPanel panel, string textKey, Action action, int width)
    {
        string text = UiText.Get(textKey);
        var button = new Button
        {
            Text = text, Font = panel.Font,
            Width = Math.Max(width, TextRenderer.MeasureText(text, panel.Font).Width + 34),
            Height = 42,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(30, 113, 205),
            ForeColor = Color.White,
            Margin = new Padding(4, 3, 4, 3),
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += (_, _) => action();
        panel.Controls.Add(button);
    }

    private void AddColor(FlowLayoutPanel panel, Color color)
    {
        var swatch = new Button
        {
            BackColor = color, Width = 38, Height = 37,
            Margin = new Padding(3, 2, 3, 0),
            AccessibleName = "Elegir color " + color.Name,
            Text = string.Empty, FlatStyle = FlatStyle.Flat
        };
        swatch.Click += (_, _) => SelectInkColor(color);
        panel.Controls.Add(swatch);
    }

    private void SelectInkColor(Color color)
    {
        _ink = color;
        _canvas.InkColor = color;
        _selectedColorPreview.BackColor = color;
        _status.Text = $"Color seleccionado: {color.Name} (#{color.R:X2}{color.G:X2}{color.B:X2}).";
    }

    // Familias de colores para acceder rápidamente, sin llenar el editor de botones.
    private void InitializeColorPalettes()
    {
        AddPalette(UiText.Get("Color.Vivid"), new (string Name, Color Color)[]
        {
            (UiText.Get("Color.Orange"), Color.DarkOrange),
            (UiText.Get("Color.Yellow"), Color.Gold),
            (UiText.Get("Color.Fuchsia"), Color.DeepPink),
            (UiText.Get("Color.Turquoise"), Color.DarkTurquoise),
            (UiText.Get("Color.Violet"), Color.BlueViolet),
            (UiText.Get("Color.BrightGreen"), Color.Lime)
        });
        AddPalette(UiText.Get("Color.Pastel"), new (string Name, Color Color)[]
        {
            (UiText.Get("Color.PastelPink"), Color.FromArgb(255, 182, 193)),
            (UiText.Get("Color.SkyBlue"), Color.FromArgb(173, 216, 230)),
            (UiText.Get("Color.Mint"), Color.FromArgb(152, 251, 152)),
            (UiText.Get("Color.Lavender"), Color.FromArgb(216, 191, 216)),
            (UiText.Get("Color.Peach"), Color.FromArgb(255, 218, 185)),
            (UiText.Get("Color.SoftYellow"), Color.FromArgb(255, 250, 205))
        });
        AddPalette(UiText.Get("Color.Dark"), new (string Name, Color Color)[]
        {
            (UiText.Get("Color.Navy"), Color.Navy),
            (UiText.Get("Color.Burgundy"), Color.Maroon),
            (UiText.Get("Color.Forest"), Color.ForestGreen),
            (UiText.Get("Color.DarkPurple"), Color.Indigo),
            (UiText.Get("Color.Brown"), Color.SaddleBrown),
            (UiText.Get("Color.Slate"), Color.DarkSlateGray)
        });
        AddPalette(UiText.Get("Color.Neutral"), new (string Name, Color Color)[]
        {
            (UiText.Get("Color.White"), Color.White),
            (UiText.Get("Color.LightGray"), Color.LightGray),
            (UiText.Get("Color.Gray"), Color.Gray),
            (UiText.Get("Color.DarkGray"), Color.DimGray),
            (UiText.Get("Color.Black"), Color.Black),
            (UiText.Get("Color.Beige"), Color.Beige)
        });

        _colorPalettes.Items.Add(new ToolStripSeparator());
        var personalized = new ToolStripMenuItem(UiText.Get("Color.Any"));
        personalized.Click += (_, _) => OpenCustomColorDialog();
        _colorPalettes.Items.Add(personalized);
    }

    private void AddPalette(string title, (string Name, Color Color)[] colors)
    {
        var category = new ToolStripMenuItem(title);
        foreach (var (name, color) in colors)
        {
            // Un pequeño cuadrado junto al nombre para previsualizar el tono.
            var icon = new Bitmap(18, 18);
            using (var graphics = Graphics.FromImage(icon))
            {
                graphics.Clear(Color.Transparent);
                using var brush = new SolidBrush(color);
                graphics.FillRectangle(brush, 1, 1, 16, 16);
                graphics.DrawRectangle(Pens.DimGray, 1, 1, 16, 16);
            }
            _paletteIcons.Add(icon);
            var option = new ToolStripMenuItem(name) { Image = icon };
            option.Click += (_, _) => SelectInkColor(color);
            category.DropDownItems.Add(option);
        }
        _colorPalettes.Items.Add(category);
    }

    private void OpenCustomColorDialog()
    {
        using var dialog = new ColorDialog
        {
            Color = _ink,
            FullOpen = true,
            AnyColor = true,
            SolidColorOnly = false
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            SelectInkColor(dialog.Color);
    }

    private void OnEdit(AnnotationAction action)
    {
        TextAnnotationOptions? textOptions = null;
        if (action.Tool == AnnotationTool.Text)
        {
            textOptions = AskForText();
            if (textOptions is null || string.IsNullOrWhiteSpace(textOptions.Text)) return;
        }

        if (action.Tool == AnnotationTool.Move)
        {
            MoveLastAnnotation(action);
            return;
        }

        Bitmap? before = null;
        try
        {
            before = (Bitmap)_image.Clone();
            if (action.Tool == AnnotationTool.Crop)
            {
                Rectangle rect = Rectangle.FromLTRB(
                    Math.Min(action.Start.X, action.End.X),
                    Math.Min(action.Start.Y, action.End.Y),
                    Math.Max(action.Start.X, action.End.X),
                    Math.Max(action.Start.Y, action.End.Y));
                rect = Rectangle.Intersect(rect, new Rectangle(Point.Empty, _image.Size));
                if (rect.Width < 3 || rect.Height < 3)
                {
                    before.Dispose();
                    _status.Text = UiText.Get("Editor.CropTooSmall");
                    return;
                }
                Bitmap cropped = _image.Clone(rect, PixelFormat.Format32bppArgb);
                _canvas.Image = cropped;
                _image.Dispose();
                _image = cropped;
                _lastEditable?.Dispose();
                _lastEditable = null;
                UpdateImageTitle();
            }
            else
            {
                int? stepNumber = action.Tool == AnnotationTool.Step ? _nextStepNumber++ : null;
                ApplyAnnotation(_image, action, _ink, _canvas.InkWidth, textOptions, stepNumber);

                if (IsEditable(action.Tool))
                {
                    _lastEditable?.Dispose();
                    _lastEditable = new EditableAnnotation((Bitmap)before.Clone(), action,
                        _ink, _canvas.InkWidth, textOptions, stepNumber);
                }
                else
                {
                    _lastEditable?.Dispose();
                    _lastEditable = null;
                }
            }

            PushUndo(before);
            before = null;
            _canvas.Invalidate();
            _status.Text = string.Format(UiText.Get("Editor.ChangeApplied"), _history.Count, MaxUndo);
        }
        catch (Exception ex)
        {
            before?.Dispose();
            MessageBox.Show(this, UiText.Get("Error.ApplyChange") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static bool IsEditable(AnnotationTool tool) => tool is
        AnnotationTool.Arrow or AnnotationTool.Rectangle or AnnotationTool.Ellipse or
        AnnotationTool.Pencil or AnnotationTool.Text or AnnotationTool.Step;

    private void MoveLastAnnotation(AnnotationAction move)
    {
        if (_lastEditable is null)
        {
            _status.Text = UiText.Get("Editor.NoMovable");
            return;
        }

        int dx = move.End.X - move.Start.X;
        int dy = move.End.Y - move.Start.Y;
        AnnotationAction translated = Translate(_lastEditable.Action, dx, dy);
        Bitmap before = (Bitmap)_image.Clone();
        Bitmap replacement = (Bitmap)_lastEditable.BaseImage.Clone();
        try
        {
            ApplyAnnotation(replacement, translated, _lastEditable.Color,
                _lastEditable.Width, _lastEditable.TextOptions, _lastEditable.StepNumber);
            PushUndo(before);
            _image.Dispose();
            _image = replacement;
            _canvas.Image = _image;
            _lastEditable.Action = translated;
            _canvas.Invalidate();
            _status.Text = UiText.Get("Editor.Moved");
        }
        catch
        {
            before.Dispose();
            replacement.Dispose();
            throw;
        }
    }

    private static AnnotationAction Translate(AnnotationAction action, int dx, int dy) => new(
        action.Tool,
        new Point(action.Start.X + dx, action.Start.Y + dy),
        new Point(action.End.X + dx, action.End.Y + dy),
        action.Path.Select(point => new Point(point.X + dx, point.Y + dy)).ToArray());

    private static Rectangle ActionRectangle(AnnotationAction action, Size imageSize)
    {
        Rectangle rect = Rectangle.FromLTRB(
            Math.Min(action.Start.X, action.End.X), Math.Min(action.Start.Y, action.End.Y),
            Math.Max(action.Start.X, action.End.X), Math.Max(action.Start.Y, action.End.Y));
        return Rectangle.Intersect(rect, new Rectangle(Point.Empty, imageSize));
    }

    private static void ApplyAnnotation(Bitmap target, AnnotationAction action, Color color,
        int width, TextAnnotationOptions? textOptions, int? stepNumber)
    {
        if (action.Tool is AnnotationTool.Pixelate or AnnotationTool.Blur)
        {
            ApplyRegionEffect(target, ActionRectangle(action, target.Size),
                action.Tool == AnnotationTool.Pixelate);
            return;
        }

        using Graphics g = Graphics.FromImage(target);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (action.Tool == AnnotationTool.Text && textOptions is not null)
        {
            FontStyle style = textOptions.Bold ? FontStyle.Bold : FontStyle.Regular;
            using var font = new Font(textOptions.FontName, textOptions.Size, style,
                GraphicsUnit.Pixel);
            SizeF measured = g.MeasureString(textOptions.Text, font);
            if (textOptions.DrawBackground)
            {
                using var background = new SolidBrush(textOptions.BackgroundColor);
                g.FillRectangle(background, action.Start.X - 5, action.Start.Y - 3,
                    measured.Width + 10, measured.Height + 6);
            }
            using var brush = new SolidBrush(color);
            g.DrawString(textOptions.Text, font, brush, action.Start);
            return;
        }

        AnnotationPainter.Paint(g, action, color, width, preview: false);
        if (action.Tool == AnnotationTool.Step && stepNumber.HasValue)
        {
            int radius = Math.Max(16, width * 5);
            using var font = new Font("Segoe UI", radius, FontStyle.Bold, GraphicsUnit.Pixel);
            string number = stepNumber.Value.ToString();
            SizeF measured = g.MeasureString(number, font);
            g.DrawString(number, font, Brushes.White,
                action.Start.X - measured.Width / 2,
                action.Start.Y - measured.Height / 2 - 1);
        }
    }

    private static void ApplyRegionEffect(Bitmap image, Rectangle rect, bool pixelate)
    {
        if (rect.Width < 3 || rect.Height < 3) return;
        int divisor = pixelate ? 12 : 24;
        int smallWidth = Math.Max(1, rect.Width / divisor);
        int smallHeight = Math.Max(1, rect.Height / divisor);
        using Bitmap original = image.Clone(rect, PixelFormat.Format32bppArgb);
        using Bitmap reduced = new(smallWidth, smallHeight, PixelFormat.Format32bppArgb);
        using (Graphics small = Graphics.FromImage(reduced))
        {
            small.InterpolationMode = pixelate ? InterpolationMode.NearestNeighbor
                : InterpolationMode.HighQualityBilinear;
            small.DrawImage(original, new Rectangle(0, 0, smallWidth, smallHeight));
        }
        using Graphics output = Graphics.FromImage(image);
        output.InterpolationMode = pixelate ? InterpolationMode.NearestNeighbor
            : InterpolationMode.HighQualityBicubic;
        output.PixelOffsetMode = pixelate ? PixelOffsetMode.Half : PixelOffsetMode.HighQuality;
        output.DrawImage(reduced, rect);
    }

    private void PushUndo(Bitmap previous)
    {
        _history.AddLast(previous);
        while (_history.Count > MaxUndo)
        {
            _history.First!.Value.Dispose();
            _history.RemoveFirst();
        }
    }

    private void RotateRight() => ApplyTransform(RotateFlipType.Rotate90FlipNone, "Imagen girada 90°.");
    private void FlipHorizontal() => ApplyTransform(RotateFlipType.RotateNoneFlipX, "Imagen volteada.");

    private void ApplyTransform(RotateFlipType type, string successMessage)
    {
        Bitmap? before = null;
        try
        {
            before = (Bitmap)_image.Clone();
            _image.RotateFlip(type);
            _lastEditable?.Dispose();
            _lastEditable = null;
            PushUndo(before);
            before = null;
            UpdateImageTitle();
            _canvas.Invalidate();
            _status.Text = successMessage;
        }
        catch (Exception ex)
        {
            before?.Dispose();
            MessageBox.Show(this, UiText.Get("Error.ModifyImage") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateImageTitle() =>
        Text = string.Format(UiText.Get("Editor.Title"), _image.Width, _image.Height);

    private TextAnnotationOptions? AskForText()
    {
        using var input = new Form
        {
            Text = UiText.Get("Dialog.AddText"),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false, MaximizeBox = false,
            ShowInTaskbar = false,
            ClientSize = new Size(540, 285),
            Font = new Font("Segoe UI", 11)
        };
        var caption = new Label
        {
            Text = UiText.Get("Dialog.TextPrompt"),
            Left = 18, Top = 15, Width = 445, Height = 28
        };
        var box = new TextBox { Left = 18, Top = 52, Width = 500 };
        var sizeLabel = new Label { Text = UiText.Get("Dialog.TextSize"), Left = 18, Top = 96, Width = 90 };
        var size = new NumericUpDown
        {
            Left = 110, Top = 92, Width = 75, Minimum = 10, Maximum = 96,
            Value = Math.Max(16, _canvas.InkWidth * 5)
        };
        var bold = new CheckBox { Text = UiText.Get("Dialog.TextBold"), Left = 205, Top = 94, Width = 130, Checked = true };
        var background = new CheckBox
        {
            Text = UiText.Get("Dialog.TextBackground"), Left = 18, Top = 135,
            Width = 260, Checked = false
        };
        var backgroundColor = new Button
        {
            Text = UiText.Get("Dialog.TextBackgroundColor"), Left = 300, Top = 130,
            Width = 180, Height = 34, BackColor = Color.White
        };
        Color chosenBackground = Color.White;
        backgroundColor.Click += (_, _) =>
        {
            using var picker = new ColorDialog { Color = chosenBackground, FullOpen = true };
            if (picker.ShowDialog(input) != DialogResult.OK) return;
            chosenBackground = picker.Color;
            backgroundColor.BackColor = chosenBackground;
        };
        var ok = new Button
        {
            Text = UiText.Get("Dialog.OK"), DialogResult = DialogResult.OK,
            Left = 310, Top = 220, Width = 98, Height = 38
        };
        var cancel = new Button
        {
            Text = UiText.Get("Dialog.Cancel"), DialogResult = DialogResult.Cancel,
            Left = 420, Top = 220, Width = 98, Height = 38
        };
        input.Controls.AddRange([caption, box, sizeLabel, size, bold, background,
            backgroundColor, ok, cancel]);
        input.AcceptButton = ok;
        input.CancelButton = cancel;
        input.Shown += (_, _) => box.Focus();
        return input.ShowDialog(this) == DialogResult.OK
            ? new TextAnnotationOptions(box.Text, "Segoe UI", (float)size.Value,
                bold.Checked, background.Checked, chosenBackground)
            : null;
    }

    private void Undo()
    {
        if (_history.Count == 0)
        {
            _status.Text = UiText.Get("Editor.NoUndo");
            return;
        }
        Bitmap previous = _history.Last!.Value;
        _history.RemoveLast();
        _image.Dispose();
        _image = previous;
        _canvas.Image = _image;
        _lastEditable?.Dispose();
        _lastEditable = null;
        UpdateImageTitle();
        _canvas.Invalidate();
        _status.Text = UiText.Get("Editor.Undone");
    }

    private void SaveAs()
    {
        using var dialog = new SaveFileDialog
        {
            Title = UiText.Get("Dialog.SaveCaptureTitle"),
            Filter = UiText.Get("Dialog.SaveFilter"),
            FileName = _settings.NextCaptureFileName(),
            AddExtension = true,
            DefaultExt = "png",
            OverwritePrompt = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        SaveFile(dialog.FileName);
    }

    private void QuickSave()
    {
        string folder = string.IsNullOrWhiteSpace(_settings.QuickSaveFolder)
            ? GetSaveFolder()
            : _settings.QuickSaveFolder;
        Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, _settings.NextCaptureFileName());
        SaveFile(path);
    }

    private void SaveFile(string path)
    {
        try
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            _image.Save(path, ext is ".jpg" or ".jpeg" ? ImageFormat.Jpeg : ImageFormat.Png);
            CaptureHistory.Add(_image, _settings.HistoryLimit);
            _status.Text = string.Format(UiText.Get("Status.ImageSaved"), path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, UiText.Get("Error.SaveImage") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string GetSaveFolder()
    {
        string pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        if (string.IsNullOrWhiteSpace(pictures))
            pictures = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        return Path.Combine(pictures, "CapturaLibre");
    }

    private void OpenFolder()
    {
        try
        {
            string folder = string.IsNullOrWhiteSpace(_settings.QuickSaveFolder)
                ? GetSaveFolder()
                : _settings.QuickSaveFolder;
            Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = folder,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, UiText.Get("Error.OpenFolder") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CopyImage()
    {
        try
        {
            Clipboard.SetImage(_image);
            _status.Text = UiText.Get("Status.ImageCopied");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, UiText.Get("Error.CopyImage") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void HandleKeys(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Z) { Undo(); e.SuppressKeyPress = true; }
        else if (e.Control && e.KeyCode == Keys.S) { SaveAs(); e.SuppressKeyPress = true; }
        else if (e.Control && e.KeyCode == Keys.C) { CopyImage(); e.SuppressKeyPress = true; }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _canvas.Image = null;
        _image.Dispose();
        foreach (Bitmap b in _history) b.Dispose();
        _history.Clear();
        _lastEditable?.Dispose();
        _lastEditable = null;
        _colorPalettes.Dispose();
        foreach (Image icon in _paletteIcons) icon.Dispose();
        _paletteIcons.Clear();
        base.OnFormClosed(e);
    }

    private sealed class EditableAnnotation : IDisposable
    {
        public Bitmap BaseImage { get; }
        public AnnotationAction Action { get; set; }
        public Color Color { get; }
        public int Width { get; }
        public TextAnnotationOptions? TextOptions { get; }
        public int? StepNumber { get; }

        public EditableAnnotation(Bitmap baseImage, AnnotationAction action, Color color,
            int width, TextAnnotationOptions? textOptions, int? stepNumber)
        {
            BaseImage = baseImage;
            Action = action;
            Color = color;
            Width = width;
            TextOptions = textOptions;
            StepNumber = stepNumber;
        }

        public void Dispose() => BaseImage.Dispose();
    }

    private sealed record TextAnnotationOptions(string Text, string FontName, float Size,
        bool Bold, bool DrawBackground, Color BackgroundColor);
}
