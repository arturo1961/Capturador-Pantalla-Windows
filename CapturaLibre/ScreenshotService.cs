using System.Drawing.Imaging;

namespace CapturaLibre;

internal static class ScreenshotService
{
    public static Rectangle DesktopBounds => SystemInformation.VirtualScreen;

    public static Bitmap Take(Rectangle bounds)
    {
        if (bounds.Width < 1 || bounds.Height < 1)
            throw new InvalidOperationException("No se ha encontrado ninguna pantalla.");

        var image = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
        try
        {
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size,
                    CopyPixelOperation.SourceCopy);
            }
            return image;
        }
        catch
        {
            image.Dispose();
            throw;
        }
    }

    // crop is relative to the image (not to the desktop's virtual origin).
    public static Bitmap Crop(Bitmap original, Rectangle crop)
    {
        crop = Rectangle.Intersect(crop, new Rectangle(Point.Empty, original.Size));
        if (crop.Width < 2 || crop.Height < 2)
            throw new ArgumentException("Selecciona una zona más grande.", nameof(crop));

        return original.Clone(crop, PixelFormat.Format32bppArgb);
    }

    public static Bitmap AutoCropForDnet(Bitmap original)
    {
        if (original.Width <= 900 && original.Height <= 650)
            return new Bitmap(original);

        Color background = FindDominantBackground(original);
        int startY = Math.Min(160, Math.Max(0, original.Height / 4));
        int maxX = 0;
        int maxY = 0;

        // Se ignora la franja superior durante el análisis porque las barras de
        // título y menús suelen ocupar todo el ancho aunque el contenido real no.
        for (int y = startY; y < original.Height; y += 3)
        {
            for (int x = 0; x < original.Width; x += 3)
            {
                Color pixel = original.GetPixel(x, y);
                if (ColorDistance(pixel, background) <= 48) continue;
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
            }
        }

        if (maxX == 0 || maxY == 0)
            return new Bitmap(original);

        int width = Math.Min(original.Width, Math.Max(900, maxX + 80));
        int height = Math.Min(original.Height, Math.Max(650, maxY + 80));
        if (width >= original.Width - 20 && height >= original.Height - 20)
            return new Bitmap(original);

        return Crop(original, new Rectangle(0, 0, width, height));
    }

    private static Color FindDominantBackground(Bitmap image)
    {
        var buckets = new Dictionary<int, (int Count, long R, long G, long B)>();
        int startY = Math.Min(160, Math.Max(0, image.Height / 4));
        for (int y = startY; y < image.Height; y += 12)
        {
            for (int x = 0; x < image.Width; x += 12)
            {
                Color color = image.GetPixel(x, y);
                int key = (color.R / 16 << 8) | (color.G / 16 << 4) | color.B / 16;
                buckets.TryGetValue(key, out var value);
                buckets[key] = (value.Count + 1, value.R + color.R,
                    value.G + color.G, value.B + color.B);
            }
        }

        var dominant = buckets.Values.OrderByDescending(value => value.Count).First();
        return Color.FromArgb((int)(dominant.R / dominant.Count),
            (int)(dominant.G / dominant.Count), (int)(dominant.B / dominant.Count));
    }

    private static int ColorDistance(Color first, Color second) =>
        Math.Abs(first.R - second.R) + Math.Abs(first.G - second.G) +
        Math.Abs(first.B - second.B);
}
