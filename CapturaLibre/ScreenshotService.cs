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
}
