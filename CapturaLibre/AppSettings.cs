using System.Text.Json;

namespace CapturaLibre;

internal sealed class AppSettings
{
    public bool MinimizeOnStart { get; set; }
    public bool DnetMode { get; set; } = true;
    public string DnetOutputFolder { get; set; } = string.Empty;

    private static string SettingsPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CapturaLibre_DNET", "ajustes.json");

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
                return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath)) ?? new();
        }
        catch (Exception)
        {
            // Si los ajustes están dañados, el programa sigue funcionando con valores por defecto.
        }
        return new AppSettings();
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
}
