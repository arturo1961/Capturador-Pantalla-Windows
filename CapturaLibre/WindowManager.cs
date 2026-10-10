using System.Runtime.InteropServices;
using System.Text;

namespace CapturaLibre;

internal sealed record WindowChoice(nint Handle, string Title)
{
    public override string ToString() => Title.Length > 95 ? Title[..92] + "…" : Title;
}

// No cierra aplicaciones ni toca sus documentos.
internal sealed class WindowManager
{
    private readonly HashSet<nint> _minimizedByUs = new();
    private const int SwMinimize = 6;
    private const int SwRestore = 9;
    private const uint GwOwner = 4;
    private const int DwmwaExtendedFrameBounds = 9;

    private delegate bool EnumWindowsProc(nint hwnd, nint lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc callback, nint lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(nint hwnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsIconic(nint hwnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(nint hwnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(nint hwnd, StringBuilder text, int maxCount);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(nint hwnd, out uint processId);

    [DllImport("user32.dll")]
    private static extern nint GetWindow(nint hwnd, uint command);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindow(nint hwnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(nint hwnd, int command);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(nint hwnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(nint hwnd, out NativeRect rect);

    [DllImport("dwmapi.dll")]
    private static extern int DwmGetWindowAttribute(nint hwnd, int attribute,
        out NativeRect rect, int size);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeRect
    {
        public int Left, Top, Right, Bottom;
        public Rectangle ToRectangle() => Rectangle.FromLTRB(Left, Top, Right, Bottom);
    }

    private static bool IsCandidate(nint hwnd)
    {
        if (!IsWindowVisible(hwnd) || GetWindowTextLength(hwnd) <= 0 ||
            GetWindow(hwnd, GwOwner) != 0) return false;
        GetWindowThreadProcessId(hwnd, out uint pid);
        return pid != 0 && pid != (uint)Environment.ProcessId;
    }

    public List<WindowChoice> ListWindows()
    {
        var windows = new List<WindowChoice>();
        EnumWindows((hwnd, _) =>
        {
            if (!IsCandidate(hwnd)) return true;
            int length = Math.Min(GetWindowTextLength(hwnd), 1024);
            var name = new StringBuilder(length + 1);
            GetWindowText(hwnd, name, name.Capacity);
            string title = name.ToString().Trim();
            if (title.Length > 0) windows.Add(new WindowChoice(hwnd, title));
            return true;
        }, 0);
        return windows;
    }

    public int MinimizeOthers()
    {
        int count = 0;
        EnumWindows((hwnd, _) =>
        {
            if (!IsCandidate(hwnd) || IsIconic(hwnd)) return true;
            ShowWindow(hwnd, SwMinimize);
            if (IsIconic(hwnd))
            {
                _minimizedByUs.Add(hwnd);
                count++;
            }
            return true;
        }, 0);
        return count;
    }

    public int RestoreOthers()
    {
        int count = 0;
        foreach (nint hwnd in _minimizedByUs)
        {
            if (IsWindow(hwnd) && IsIconic(hwnd))
            {
                ShowWindow(hwnd, SwRestore);
                count++;
            }
        }
        _minimizedByUs.Clear();
        return count;
    }

    public Rectangle PrepareWindowCapture(nint hwnd)
    {
        if (!IsWindow(hwnd) || !IsCandidate(hwnd))
            throw new InvalidOperationException("La ventana seleccionada ya no está disponible.");

        if (IsIconic(hwnd)) ShowWindow(hwnd, SwRestore);
        // Windows puede impedir cambiar el primer plano en algunos contextos.
        // El usuario puede restaurar la ventana manualmente en ese caso.
        SetForegroundWindow(hwnd);
        if (DwmGetWindowAttribute(hwnd, DwmwaExtendedFrameBounds,
                out NativeRect frame, Marshal.SizeOf<NativeRect>()) != 0 ||
            frame.Right <= frame.Left || frame.Bottom <= frame.Top)
        {
            if (!GetWindowRect(hwnd, out frame))
                throw new InvalidOperationException("No se puede localizar la ventana.");
        }

        Rectangle visible = Rectangle.Intersect(frame.ToRectangle(), ScreenshotService.DesktopBounds);
        if (visible.Width < 2 || visible.Height < 2)
            throw new InvalidOperationException("La ventana debe estar visible dentro de una pantalla.");
        return visible;
    }
}
