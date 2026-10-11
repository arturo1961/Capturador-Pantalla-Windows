using System.Text.Json;

namespace CapturaLibre;

internal sealed class AppSettings
{
    private const int CurrentSettingsVersion = 260;

    // Cero identifica los ajustes creados por versiones anteriores, que no
    // incluían este campo en el archivo JSON.
    public int SettingsVersion { get; set; }
    public bool MinimizeOnStart { get; set; }
    public bool DnetMode { get; set; }
    public bool DnetAutoCrop { get; set; } = true;
    public string DnetOutputFolder { get; set; } = string.Empty;
    public string Language { get; set; } = "es";
    public string CapturePreset { get; set; } = "Manual";
    public string CaptureNamePattern { get; set; } = "CapturaLibre_{fecha}_{contador}";
    public int CaptureCounter { get; set; } = 1;
    public string QuickSaveFolder { get; set; } = string.Empty;
    public int HistoryLimit { get; set; } = 20;
    public bool WindowThumbnails { get; set; } = true;

    private static string SettingsPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CapturaLibre_DNET", "ajustes.json");

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                AppSettings settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath)) ?? new();
                // La versión 2.4.0 activaba DNET por defecto. Al actualizar,
                // se desmarca una sola vez para que las capturas normales abran
                // el editor. A partir de aquí se conserva la elección del usuario.
                if (settings.SettingsVersion < 241)
                {
                    settings.DnetMode = false;
                }
                if (settings.SettingsVersion < CurrentSettingsVersion)
                {
                    settings.SettingsVersion = CurrentSettingsVersion;
                    settings.Save();
                }
                return settings;
            }
        }
        catch (Exception)
        {
            // Si los ajustes están dañados, el programa sigue funcionando con valores por defecto.
        }
        return new AppSettings { SettingsVersion = CurrentSettingsVersion };
    }

    public void Save()
    {
        try
        {
            string? directory = Path.GetDirectoryName(SettingsPath);
            if (directory is not null) Directory.CreateDirectory(directory);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
        catch (Exception)
        {
            // El guardado de preferencias no debe bloquear la captura de pantalla.
        }
    }

    public string NextCaptureFileName(string? windowTitle = null)
    {
        int counter = Math.Max(1, CaptureCounter++);
        string pattern = string.IsNullOrWhiteSpace(CaptureNamePattern)
            ? "CapturaLibre_{fecha}_{contador}"
            : CaptureNamePattern.Trim();
        string name = pattern
            .Replace("{fecha}", DateTime.Now.ToString("yyyyMMdd"), StringComparison.OrdinalIgnoreCase)
            .Replace("{hora}", DateTime.Now.ToString("HHmmss"), StringComparison.OrdinalIgnoreCase)
            .Replace("{contador}", counter.ToString("000"), StringComparison.OrdinalIgnoreCase)
            .Replace("{ventana}", windowTitle ?? "captura", StringComparison.OrdinalIgnoreCase);
        foreach (char invalid in Path.GetInvalidFileNameChars()) name = name.Replace(invalid, '_');
        name = name.Trim(' ', '.');
        if (name.Length == 0) name = $"CapturaLibre_{DateTime.Now:yyyyMMdd_HHmmss}";
        Save();
        return name + ".png";
    }
}
