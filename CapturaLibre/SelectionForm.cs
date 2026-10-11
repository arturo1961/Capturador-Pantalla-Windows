using System.Drawing.Drawing2D;

namespace CapturaLibre;

internal sealed class SelectionForm : Form
{
    private readonly Bitmap _desktop;
    private bool _dragging;
    private Point _first;
    private Point _last;

    public Rectangle SelectedArea { get; private set; }

    public SelectionForm(Bitmap desktop, Rectangle virtualBounds)
    {
        _desktop = desktop;
        Text = UiText.Get("Selection.Title");
        Icon = Program.AppIcon;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        Bounds = virtualBounds;
        ShowInTaskbar = false;
        TopMost = true;
        KeyPreview = true;
        Cursor = Cursors.Cross;
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Cancel(); };
        MouseDown += OnSelectMouseDown;
        MouseMove += OnSelectMouseMove;
        MouseUp += OnSelectMouseUp;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.DrawImageUnscaled(_desktop, 0, 0);
        using var shade = new SolidBrush(Color.FromArgb(105, Color.Black));
        g.FillRectangle(shade, ClientRectangle);

        if (!_dragging) return;
        Rectangle box = Normalize(_first, _last);
        if (box.Width < 1 || box.Height < 1) return;
        Rectangle actual = Rectangle.Intersect(box, new Rectangle(Point.Empty, _desktop.Size));
        if (actual.IsEmpty) return;

        g.DrawImage(_desktop, actual, actual, GraphicsUnit.Pixel);
        using var border = new Pen(Color.FromArgb(0, 150, 255), 2) { DashStyle = DashStyle.Dash };
        g.DrawRectangle(border, actual);
        using var font = new Font("Segoe UI", 11, FontStyle.Bold);
        using var backdrop = new SolidBrush(Color.FromArgb(205, 29, 41, 59));
        string label = $"{actual.Width} × {actual.Height}";
        SizeF labelSize = g.MeasureString(label, font);
        float labelX = Math.Clamp(actual.Left + 5, 0, Math.Max(0, ClientSize.Width - labelSize.Width - 15));
        float labelY = actual.Top > 35 ? actual.Top - 34 : actual.Top + 5;
        g.FillRectangle(backdrop, labelX, labelY, labelSize.Width + 12, labelSize.Height + 7);
        g.DrawString(label, font, Brushes.White, labelX + 6, labelY + 3);
    }

    private void OnSelectMouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) { Cancel(); return; }
        if (e.Button != MouseButtons.Left) return;
        _first = e.Location;
        _last = e.Location;
        _dragging = true;
        Capture = true;
    }

    private void OnSelectMouseMove(object? sender, MouseEventArgs e)
    {
        if (!_dragging) return;
        _last = e.Location;
        Invalidate();
    }

    private void OnSelectMouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || !_dragging) return;
        _last = e.Location;
        _dragging = false;
        Capture = false;
        SelectedArea = Rectangle.Intersect(Normalize(_first, _last),
            new Rectangle(Point.Empty, _desktop.Size));
        if (SelectedArea.Width < 3 || SelectedArea.Height < 3)
        {
            Invalidate();
            return;
        }
        DialogResult = DialogResult.OK;
        Close();
    }

    private void Cancel()
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private static Rectangle Normalize(Point a, Point b) => Rectangle.FromLTRB(
        Math.Min(a.X, b.X), Math.Min(a.Y, b.Y),
        Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
}
