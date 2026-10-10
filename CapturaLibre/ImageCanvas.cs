using System.Drawing.Drawing2D;

namespace CapturaLibre;

internal enum AnnotationTool { Rectangle, Ellipse, Arrow, Pencil, Text, Redact, Crop }

internal sealed record AnnotationAction(
    AnnotationTool Tool,
    Point Start,
    Point End,
    IReadOnlyList<Point> Path);

internal sealed class ImageCanvas : Control
{
    public Bitmap? Image { get; set; }
    public AnnotationTool Tool { get; set; } = AnnotationTool.Arrow;
    public Color InkColor { get; set; } = Color.Red;
    public int InkWidth { get; set; } = 4;
    public event Action<AnnotationAction>? EditCompleted;

    private bool _dragging;
    private Point _start;
    private Point _end;
    private readonly List<Point> _path = new();

    public ImageCanvas()
    {
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(223, 228, 236);
        Cursor = Cursors.Cross;
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
    }

    private (RectangleF Dest, float Scale) GetViewport()
    {
        if (Image is null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
            return (RectangleF.Empty, 1);
        const int margin = 20;
        float availW = Math.Max(1, ClientSize.Width - 2 * margin);
        float availH = Math.Max(1, ClientSize.Height - 2 * margin);
        float scale = Math.Min(availW / Image.Width, availH / Image.Height);
        // Display at native size if there is more room; avoid enlarging pixelated images.
        scale = Math.Min(1f, scale);
        float width = Image.Width * scale;
        float height = Image.Height * scale;
        return (new RectangleF((ClientSize.Width - width) / 2f,
             (ClientSize.Height - height) / 2f, width, height), scale);
    }

    private Point? ScreenToImage(Point mouse)
    {
        if (Image is null) return null;
        var (dest, scale) = GetViewport();
        if (!dest.Contains(mouse)) return null;
        return new Point(Math.Clamp((int)((mouse.X - dest.Left) / scale), 0, Image.Width - 1),
                         Math.Clamp((int)((mouse.Y - dest.Top) / scale), 0, Image.Height - 1));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (Image is null) return;
        Graphics g = e.Graphics;
        var (dest, scale) = GetViewport();
        g.InterpolationMode = scale < 1 ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
        g.DrawImage(Image, dest);
        using var edge = new Pen(Color.FromArgb(146, 157, 173), 1);
        g.DrawRectangle(edge, dest.X, dest.Y, dest.Width, dest.Height);

        if (!_dragging) return;
        using var state = new GraphicsStateGuard(g);
        g.TranslateTransform(dest.X, dest.Y);
        g.ScaleTransform(scale, scale);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        AnnotationPainter.Paint(g, new AnnotationAction(Tool, _start, _end, _path),
            InkColor, InkWidth, preview: true);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left) return;
        Point? point = ScreenToImage(e.Location);
        if (point is null) return;
        _dragging = true;
        _start = _end = point.Value;
        _path.Clear();
        _path.Add(point.Value);
        Capture = true;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (!_dragging) return;
        // When mouse leaves canvas, hold the last valid image coordinate.
        Point? point = ScreenToImage(e.Location);
        if (point is null) return;
        _end = point.Value;
        if (_path.Count == 0 || _path[^1] != _end) _path.Add(_end);
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left || !_dragging) return;
        Point? end = ScreenToImage(e.Location);
        if (end is not null) _end = end.Value;
        _dragging = false;
        Capture = false;
        var action = new AnnotationAction(Tool, _start, _end, _path.ToArray());
        _path.Clear();
        Invalidate();
        if (action.Tool == AnnotationTool.Text ||
            (action.Tool == AnnotationTool.Pencil && action.Path.Count > 1) ||
            (action.Tool != AnnotationTool.Pencil && action.Start != action.End))
            EditCompleted?.Invoke(action);
    }

    private sealed class GraphicsStateGuard : IDisposable
    {
        private readonly Graphics _graphics;
        private readonly GraphicsState _state;
        public GraphicsStateGuard(Graphics graphics)
        {
            _graphics = graphics;
            _state = graphics.Save();
        }
        public void Dispose() => _graphics.Restore(_state);
    }
}

internal static class AnnotationPainter
{
    public static void Paint(Graphics g, AnnotationAction a, Color color, int width, bool preview)
    {
        var rect = Rectangle.FromLTRB(Math.Min(a.Start.X, a.End.X), Math.Min(a.Start.Y, a.End.Y),
            Math.Max(a.Start.X, a.End.X), Math.Max(a.Start.Y, a.End.Y));

        if (a.Tool == AnnotationTool.Crop)
        {
            using var border = new Pen(Color.FromArgb(0, 154, 235), Math.Max(2, width));
            border.DashStyle = DashStyle.Dash;
            g.DrawRectangle(border, rect);
            return;
        }

        if (a.Tool == AnnotationTool.Redact)
        {
            using var fill = new SolidBrush(Color.Black);
            g.FillRectangle(fill, rect);
            return;
        }

        if (a.Tool == AnnotationTool.Text)
        {
            if (preview)
            {
                using var marker = new Pen(color, 2);
                g.DrawLine(marker, a.Start.X - 7, a.Start.Y, a.Start.X + 7, a.Start.Y);
                g.DrawLine(marker, a.Start.X, a.Start.Y - 7, a.Start.X, a.Start.Y + 7);
            }
            return;
        }

        using var pen = new Pen(color, width)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };
        if (preview) pen.DashStyle = DashStyle.Dash;
        switch (a.Tool)
        {
            case AnnotationTool.Rectangle:
                g.DrawRectangle(pen, rect);
                break;
            case AnnotationTool.Ellipse:
                g.DrawEllipse(pen, rect);
                break;
            case AnnotationTool.Arrow:
                // Arrowheads are preserved in the final saved image.
                pen.DashStyle = DashStyle.Solid;
                using (var arrow = new AdjustableArrowCap(5, 7, true))
                {
                    pen.CustomEndCap = arrow;
                    g.DrawLine(pen, a.Start, a.End);
                }
                break;
            case AnnotationTool.Pencil:
                if (a.Path.Count > 1) g.DrawLines(pen, a.Path.ToArray());
                break;
        }
    }
}
