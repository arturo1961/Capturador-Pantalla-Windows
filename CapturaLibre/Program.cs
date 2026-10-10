namespace CapturaLibre;

internal static class Program
{
    // Usar el mismo icono en la ventana principal y en el editor.
    // Al estar incrustado en la aplicación funciona incluso en el EXE de un solo archivo.
    internal static Icon AppIcon { get; } = LoadAppIcon();

    private static Icon LoadAppIcon()
    {
        using Stream? resource = typeof(Program).Assembly
            .GetManifestResourceStream("CapturaLibre.AppIcon.ico");
        if (resource is null) return (Icon)SystemIcons.Application.Clone();
        using var icon = new Icon(resource);
        return (Icon)icon.Clone();
    }

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
