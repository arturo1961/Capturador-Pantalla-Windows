using System.Drawing.Imaging;

namespace CapturaLibre;

internal static class CaptureHistory
{
    public static string Folder => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CapturaLibre_DNET", "Historial");

    public static void Add(Bitmap image, int limit)
    {
        try
        {
            Directory.CreateDirectory(Folder);
            string path = Path.Combine(Folder, $"{DateTime.Now:yyyyMMdd_HHmmss_fff}.png");
            image.Save(path, ImageFormat.Png);
            int keep = Math.Clamp(limit, 5, 100);
            foreach (FileInfo old in new DirectoryInfo(Folder).EnumerateFiles("*.png")
                         .OrderByDescending(file => file.LastWriteTimeUtc).Skip(keep))
                old.Delete();
        }
        catch (Exception)
        {
            // El historial es una ayuda opcional y nunca debe impedir una captura.
        }
    }

    public static IReadOnlyList<string> List(int limit)
    {
        try
        {
            if (!Directory.Exists(Folder)) return Array.Empty<string>();
            return new DirectoryInfo(Folder).EnumerateFiles("*.png")
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .Take(Math.Clamp(limit, 5, 100)).Select(file => file.FullName).ToArray();
        }
        catch (Exception)
        {
            return Array.Empty<string>();
        }
    }
}
