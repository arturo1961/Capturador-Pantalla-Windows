using System.Text.Json;

namespace CapturaLibre;

internal sealed class AppSettings
{
    private const int CurrentSettingsVersion = 241;

    // Cero identifica los ajustes creados por versiones anteriores, que no
    // incluían este campo en el archivo JSON.
    public int SettingsVersion { get; set; }
    public bool MinimizeOnStart { get; set; }
    public bool DnetMode { get; set; }
    public string DnetOutputFolder { get; set; } = string.Empty;

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
                if (settings.SettingsVersion < CurrentSettingsVersion)
                {
                    settings.DnetMode = false;
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
}
