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
    private const int MaxUndo = 6;

    public EditorForm(Bitmap image)
    {
        _image = image; // ownership transferred from caller
        Text = $"Editar captura — {_image.Width} × {_image.Height} — CapturaLibre";
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
        AddTool(tools, "Flecha", AnnotationTool.Arrow, selected: true);
        AddTool(tools, "Rectángulo", AnnotationTool.Rectangle);
        AddTool(tools, "Elipse", AnnotationTool.Ellipse);
        AddTool(tools, "Lápiz", AnnotationTool.Pencil);
        AddTool(tools, "Texto", AnnotationTool.Text);
        AddTool(tools, "Ocultar datos", AnnotationTool.Redact);
        AddTool(tools, "Recortar", AnnotationTool.Crop);
        // Mantener juntos la etiqueta y el selector al pasar a la siguiente fila.
        var thicknessGroup = new FlowLayoutPanel
        {
            AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false, Margin = new Padding(12, 3, 5, 3),
            BackColor = tools.BackColor
        };
        thicknessGroup.Controls.Add(new Label
        {
            Text = "Grosor:", AutoSize = true, ForeColor = Color.White,
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
        AddAction(actions, "Deshacer", Undo, 105);
        AddAction(actions, "Girar 90°", RotateRight, 118);
        AddAction(actions, "Voltear", FlipHorizontal, 110);
        AddAction(actions, "Guardar como…", SaveAs, 150);
        AddAction(actions, "Guardado rápido", QuickSave, 150);
        AddAction(actions, "Copiar imagen", CopyImage, 150);
        AddAction(actions, "Abrir carpeta", OpenFolder, 140);
        // La paleta no se separa de su etiqueta al reducir la ventana.
        var palette = new FlowLayoutPanel
        {
            AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false, Margin = new Padding(12, 2, 5, 3),
            BackColor = actions.BackColor
        };
        palette.Controls.Add(new Label
        {
            Text = "Color:", AutoSize = true,
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
            Text = "Más colores…", Width = 150, Height = 38,
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

        _status.Text = "Arrastra para anotar; haz clic para añadir texto. Ctrl+Z: deshacer · Ctrl+S: guardar · Ctrl+C: copiar.";
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

    private void AddTool(FlowLayoutPanel panel, string text, AnnotationTool tool, bool selected = false)
    {
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
                AnnotationTool.Crop => "Recortar: arrastra una zona para conservar solo ese fragmento.",
                _ => "Herramienta seleccionada: " + text + "."
            };
        };
        panel.Controls.Add(radio);
    }

    private static void AddAction(FlowLayoutPanel panel, string text, Action action, int width)
    {
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
        AddPalette("Colores vivos", new (string Name, Color Color)[]
        {
            ("Naranja", Color.DarkOrange),
            ("Amarillo", Color.Gold),
            ("Fucsia", Color.DeepPink),
            ("Turquesa", Color.DarkTurquoise),
            ("Violeta", Color.BlueViolet),
            ("Verde brillante", Color.Lime)
        });
        AddPalette("Tonos pastel", new (string Name, Color Color)[]
        {
            ("Rosa pastel", Color.FromArgb(255, 182, 193)),
            ("Azul cielo", Color.FromArgb(173, 216, 230)),
            ("Verde menta", Color.FromArgb(152, 251, 152)),
            ("Lavanda", Color.FromArgb(216, 191, 216)),
            ("Melocotón", Color.FromArgb(255, 218, 185)),
            ("Amarillo suave", Color.FromArgb(255, 250, 205))
        });
        AddPalette("Colores oscuros", new (string Name, Color Color)[]
        {
            ("Azul marino", Color.Navy),
            ("Burdeos", Color.Maroon),
            ("Verde bosque", Color.ForestGreen),
            ("Púrpura oscuro", Color.Indigo),
            ("Marrón", Color.SaddleBrown),
            ("Gris pizarra", Color.DarkSlateGray)
        });
        AddPalette("Colores neutros", new (string Name, Color Color)[]
        {
            ("Blanco", Color.White),
            ("Gris claro", Color.LightGray),
            ("Gris", Color.Gray),
            ("Gris oscuro", Color.DimGray),
            ("Negro", Color.Black),
            ("Beige", Color.Beige)
        });

        _colorPalettes.Items.Add(new ToolStripSeparator());
        var personalized = new ToolStripMenuItem("Elegir cualquier color…");
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
        string? text = null;
        if (action.Tool == AnnotationTool.Text)
        {
            text = AskForText();
            if (string.IsNullOrWhiteSpace(text)) return;
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
                    _status.Text = "El recorte debe ser mayor que 2 × 2 píxeles.";
                    return;
                }
                Bitmap cropped = _image.Clone(rect, PixelFormat.Format32bppArgb);
                _canvas.Image = cropped;
                _image.Dispose();
                _image = cropped;
                UpdateImageTitle();
            }
            else
            {
                using Graphics g = Graphics.FromImage(_image);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (action.Tool == AnnotationTool.Text)
                {
                    using var font = new Font("Segoe UI", Math.Max(16, _canvas.InkWidth * 5),
                        FontStyle.Bold, GraphicsUnit.Pixel);
                    using var brush = new SolidBrush(_ink);
                    g.DrawString(text!, font, brush, action.Start);
                }
                else
                    AnnotationPainter.Paint(g, action, _ink, _canvas.InkWidth, preview: false);
            }

            PushUndo(before);
            before = null;
            _canvas.Invalidate();
            _status.Text = $"Cambio aplicado. Deshacer disponible ({_history.Count}/{MaxUndo}).";
        }
        catch (Exception ex)
        {
            before?.Dispose();
            MessageBox.Show(this, "No se pudo aplicar el cambio:\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
            PushUndo(before);
            before = null;
            UpdateImageTitle();
            _canvas.Invalidate();
            _status.Text = successMessage;
        }
        catch (Exception ex)
        {
            before?.Dispose();
            MessageBox.Show(this, "No se pudo modificar la imagen:\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateImageTitle() =>
        Text = $"Editar captura — {_image.Width} × {_image.Height} — CapturaLibre";

    private string? AskForText()
    {
        using var input = new Form
        {
            Text = "Añadir texto a la captura",
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false, MaximizeBox = false,
            ShowInTaskbar = false,
            ClientSize = new Size(480, 170),
            Font = new Font("Segoe UI", 11)
        };
        var caption = new Label
        {
            Text = "Texto que deseas escribir sobre la imagen:",
            Left = 18, Top = 15, Width = 445, Height = 28
        };
        var box = new TextBox { Left = 18, Top = 52, Width = 445 };
        var ok = new Button
        {
            Text = "Aceptar", DialogResult = DialogResult.OK,
            Left = 259, Top = 112, Width = 98, Height = 38
        };
        var cancel = new Button
        {
            Text = "Cancelar", DialogResult = DialogResult.Cancel,
            Left = 365, Top = 112, Width = 98, Height = 38
        };
        input.Controls.AddRange([caption, box, ok, cancel]);
        input.AcceptButton = ok;
        input.CancelButton = cancel;
        input.Shown += (_, _) => box.Focus();
        return input.ShowDialog(this) == DialogResult.OK ? box.Text : null;
    }

    private void Undo()
    {
        if (_history.Count == 0)
        {
            _status.Text = "No hay cambios para deshacer.";
            return;
        }
        Bitmap previous = _history.Last!.Value;
        _history.RemoveLast();
        _image.Dispose();
        _image = previous;
        _canvas.Image = _image;
        UpdateImageTitle();
        _canvas.Invalidate();
        _status.Text = "Última modificación deshecha.";
    }

    private void SaveAs()
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Guardar captura",
            Filter = "Imagen PNG (*.png)|*.png|Imagen JPEG (*.jpg)|*.jpg",
            FileName = "CapturaLibre_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png",
            AddExtension = true,
            DefaultExt = "png",
            OverwritePrompt = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        SaveFile(dialog.FileName);
    }

    private void QuickSave()
    {
        string folder = GetSaveFolder();
        Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, "CapturaLibre_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".png");
        SaveFile(path);
    }

    private void SaveFile(string path)
    {
        try
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            _image.Save(path, ext is ".jpg" or ".jpeg" ? ImageFormat.Jpeg : ImageFormat.Png);
            _status.Text = "Imagen guardada en: " + path;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "No se pudo guardar la imagen:\n" + ex.Message,
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
            string folder = GetSaveFolder();
            Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = folder,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "No se pudo abrir la carpeta:\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CopyImage()
    {
        try
        {
            Clipboard.SetImage(_image);
            _status.Text = "Imagen copiada. Puedes pegarla en otro programa (Ctrl+V).";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "No se pudo copiar al portapapeles:\n" + ex.Message,
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
        _colorPalettes.Dispose();
        foreach (Image icon in _paletteIcons) icon.Dispose();
        _paletteIcons.Clear();
        base.OnFormClosed(e);
    }
}
