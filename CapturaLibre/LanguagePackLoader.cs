using System.Reflection;
using System.Text.Json;

namespace CapturaLibre;

internal sealed class LanguagePack
{
    public string Code { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool RightToLeft { get; set; }
    public int SortOrder { get; set; } = 1000;
    public Dictionary<string, string> Texts { get; set; } = new();
}

internal static class LanguagePackLoader
{
    public static IReadOnlyList<LanguagePack> Load()
    {
        var packs = new Dictionary<string, LanguagePack>(StringComparer.OrdinalIgnoreCase);
        Assembly assembly = typeof(LanguagePackLoader).Assembly;

        foreach (string resource in assembly.GetManifestResourceNames()
                     .Where(name => name.Contains(".LanguagePacks.", StringComparison.Ordinal)
                         && name.EndsWith(".json", StringComparison.OrdinalIgnoreCase)))
        {
            try
            {
                using Stream? stream = assembly.GetManifestResourceStream(resource);
                if (stream is null) continue;
                LanguagePack? pack = JsonSerializer.Deserialize<LanguagePack>(stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                AddValid(packs, pack);
            }
            catch (Exception)
            {
                // Un paquete defectuoso nunca debe impedir que el capturador se abra.
            }
        }

        // Esta carpeta permite añadir o corregir idiomas sin recompilar el EXE.
        string externalFolder = Path.Combine(AppContext.BaseDirectory, "Idiomas");
        if (Directory.Exists(externalFolder))
        {
            foreach (string file in Directory.EnumerateFiles(externalFolder, "*.json"))
            {
                try
                {
                    LanguagePack? pack = JsonSerializer.Deserialize<LanguagePack>(File.ReadAllText(file),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    AddValid(packs, pack, mergeExisting: true);
                }
                catch (Exception)
                {
                    // Se ignora solamente el archivo que no sea válido.
                }
            }
        }

        return packs.Values.OrderBy(pack => pack.SortOrder)
            .ThenBy(pack => pack.DisplayName, StringComparer.CurrentCultureIgnoreCase).ToArray();
    }

    private static void AddValid(Dictionary<string, LanguagePack> packs, LanguagePack? pack,
        bool mergeExisting = false)
    {
        if (pack is null || string.IsNullOrWhiteSpace(pack.Code) ||
            string.IsNullOrWhiteSpace(pack.DisplayName) || pack.Texts.Count == 0)
            return;

        if (mergeExisting && packs.TryGetValue(pack.Code, out LanguagePack? existing))
        {
            foreach ((string key, string value) in pack.Texts)
                existing.Texts[key] = value;
            existing.DisplayName = pack.DisplayName;
            existing.RightToLeft = pack.RightToLeft;
            existing.SortOrder = pack.SortOrder;
            return;
        }

        packs[pack.Code] = pack;
    }
}
