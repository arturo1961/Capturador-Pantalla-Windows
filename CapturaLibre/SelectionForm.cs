using System.Drawing.Drawing2D;

namespace CapturaLibre;

internal sealed class SelectionForm : Form
{
    private readonly Bitmap _desktop;
    private bool _dragging;
    private Point _first;
    private Point _last;
    private Point _cursor;

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
        string label = $"X {_last.X + Bounds.Left}  Y {_last.Y + Bounds.Top}   " +
            $"{actual.Width} × {actual.Height} px";
        SizeF labelSize = g.MeasureString(label, font);
        float labelX = Math.Clamp(actual.Left + 5, 0, Math.Max(0, ClientSize.Width - labelSize.Width - 15));
        float labelY = actual.Top > 35 ? actual.Top - 34 : actual.Top + 5;
        g.FillRectangle(backdrop, labelX, labelY, labelSize.Width + 12, labelSize.Height + 7);
        g.DrawString(label, font, Brushes.White, labelX + 6, labelY + 3);
        DrawMagnifier(g, _cursor);
    }

    private void OnSelectMouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) { Cancel(); return; }
        if (e.Button != MouseButtons.Left) return;
        _first = e.Location;
        _last = e.Location;
        _cursor = e.Location;
        _dragging = true;
        Capture = true;
    }

    private void OnSelectMouseMove(object? sender, MouseEventArgs e)
    {
        if (!_dragging) return;
        _last = e.Location;
        _cursor = e.Location;
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

    private void DrawMagnifier(Graphics g, Point cursor)
    {
        const int sourceSize = 24;
        const int zoomSize = 120;
        Rectangle source = new(cursor.X - sourceSize / 2, cursor.Y - sourceSize / 2,
            sourceSize, sourceSize);
        source = Rectangle.Intersect(source, new Rectangle(Point.Empty, _desktop.Size));
        if (source.Width < 2 || source.Height < 2) return;

        int x = cursor.X + 24;
        int y = cursor.Y + 24;
        if (x + zoomSize + 6 > ClientSize.Width) x = cursor.X - zoomSize - 24;
        if (y + zoomSize + 34 > ClientSize.Height) y = cursor.Y - zoomSize - 48;
        x = Math.Clamp(x, 4, Math.Max(4, ClientSize.Width - zoomSize - 6));
        y = Math.Clamp(y, 4, Math.Max(4, ClientSize.Height - zoomSize - 34));

        Rectangle destination = new(x, y, zoomSize, zoomSize);
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.DrawImage(_desktop, destination, source, GraphicsUnit.Pixel);
        using var frame = new Pen(Color.White, 3);
        g.DrawRectangle(frame, destination);
        using var cross = new Pen(Color.Red, 1);
        g.DrawLine(cross, x + zoomSize / 2, y, x + zoomSize / 2, y + zoomSize);
        g.DrawLine(cross, x, y + zoomSize / 2, x + zoomSize, y + zoomSize / 2);

        using var font = new Font("Segoe UI", 9, FontStyle.Bold);
        using var back = new SolidBrush(Color.FromArgb(220, 29, 41, 59));
        string position = $"X {cursor.X + Bounds.Left} · Y {cursor.Y + Bounds.Top}";
        g.FillRectangle(back, x, y + zoomSize, zoomSize, 27);
        g.DrawString(position, font, Brushes.White, x + 5, y + zoomSize + 5);
    }
}
