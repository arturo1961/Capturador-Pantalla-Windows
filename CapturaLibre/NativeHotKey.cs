using System.Runtime.InteropServices;

namespace CapturaLibre;

internal static class NativeHotKey
{
    private const uint ModNoRepeat = 0x4000;
    private const uint VkSnapshot = 0x2C;

    internal static bool RegisterPrintScreen(nint windowHandle, int id) =>
        RegisterHotKey(windowHandle, id, ModNoRepeat, VkSnapshot);

    internal static void Unregister(nint windowHandle, int id) =>
        UnregisterHotKey(windowHandle, id);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RegisterHotKey(nint hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnregisterHotKey(nint hWnd, int id);
}
