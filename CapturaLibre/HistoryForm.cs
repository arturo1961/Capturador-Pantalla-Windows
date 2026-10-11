namespace CapturaLibre;

internal sealed class HistoryForm : Form
{
    private readonly AppSettings _settings;
    private readonly FlowLayoutPanel _gallery = new();
    private readonly Label _empty = new();

    public HistoryForm(AppSettings settings)
    {
        _settings = settings;
        Text = UiText.Get("History.Title");
        Icon = Program.AppIcon;
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(980, 690);
        MinimumSize = new Size(700, 480);
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(247, 249, 252);

        var header = new Label
        {
            Dock = DockStyle.Top, Height = 64,
            Text = UiText.Get("History.Header"),
            Font = new Font("Segoe UI", 15, FontStyle.Bold),
            ForeColor = Color.FromArgb(29, 41, 59),
            Padding = new Padding(18, 18, 0, 0)
        };
        _gallery.Dock = DockStyle.Fill;
        _gallery.AutoScroll = true;
        _gallery.Padding = new Padding(12);
        _gallery.WrapContents = true;
        _empty.Dock = DockStyle.Fill;
        _empty.TextAlign = ContentAlignment.MiddleCenter;
        _empty.Text = UiText.Get("History.Empty");
        _empty.Font = new Font("Segoe UI", 12);

        Controls.Add(_gallery);
        Controls.Add(_empty);
        Controls.Add(header);
        LoadGallery();
    }

    private void LoadGallery()
    {
        IReadOnlyList<string> files = CaptureHistory.List(_settings.HistoryLimit);
        _empty.Visible = files.Count == 0;
        _gallery.Visible = files.Count > 0;
        foreach (string path in files)
        {
            Bitmap thumbnail;
            try
            {
                using var source = new Bitmap(path);
                thumbnail = new Bitmap(source, new Size(210, 130));
            }
            catch (Exception) { continue; }

            var card = new Panel
            {
                Width = 235, Height = 185, Margin = new Padding(8),
                BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle,
                Tag = path, Cursor = Cursors.Hand
            };
            var picture = new PictureBox
            {
                Image = thumbnail, SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Top, Height = 140, Tag = path, Cursor = Cursors.Hand
            };
            var caption = new Label
            {
                Text = File.GetLastWriteTime(path).ToString("dd/MM/yyyy  HH:mm:ss"),
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
                Tag = path, Cursor = Cursors.Hand
            };
            EventHandler open = (_, _) => OpenCapture(path);
            card.DoubleClick += open;
            picture.DoubleClick += open;
            caption.DoubleClick += open;
            card.Controls.Add(caption);
            card.Controls.Add(picture);
            _gallery.Controls.Add(card);
        }
    }

    private void OpenCapture(string path)
    {
        try
        {
            using var source = new Bitmap(path);
            using var editor = new EditorForm(new Bitmap(source), _settings);
            editor.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, UiText.Get("History.OpenError") + "\n" + ex.Message,
                "CapturaLibre", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        foreach (PictureBox picture in _gallery.Controls.OfType<Panel>()
                     .SelectMany(panel => panel.Controls.OfType<PictureBox>()))
            picture.Image?.Dispose();
        base.OnFormClosed(e);
    }
}
