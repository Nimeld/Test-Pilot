using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Project_03_TestPilot_20260617.Services;

public class HotKeyService
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int WM_HOTKEY = 0x0312;
    private const int LAUNCH_ID = 9001;
    private const int KILL_ID = 9002;

    private readonly Window _window;
    private readonly ProcessManager _processManager;
    private HwndSource? _hwndSource;
    private Config _currentConfig = new();

    public HotKeyService(Window window, ProcessManager processManager)
    {
        _window = window;
        _processManager = processManager;
    }

    public void Initialize()
    {
        _hwndSource = PresentationSource.FromVisual(_window) as HwndSource;
        if (_hwndSource == null)
        {
            _window.SourceInitialized += (_, _) =>
            {
                _hwndSource = PresentationSource.FromVisual(_window) as HwndSource;
                _hwndSource?.AddHook(HwndHook);
            };
        }
        else
        {
            _hwndSource.AddHook(HwndHook);
        }
    }

    public void Register(Config config)
    {
        _currentConfig = config;
        if (_hwndSource == null) return;
        var h = _hwndSource.Handle;
        UnregisterHotKey(h, LAUNCH_ID);
        UnregisterHotKey(h, KILL_ID);

        if (!RegisterHotKey(h, LAUNCH_ID, 0, VkCode(config.LaunchHotKey)))
            MessageBox.Show($"热键 {config.LaunchHotKey} 注册失败，可能被占用", "TestPilot");
        if (!RegisterHotKey(h, KILL_ID, 0, VkCode(config.KillHotKey)))
            MessageBox.Show($"热键 {config.KillHotKey} 注册失败，可能被占用", "TestPilot");
    }

    public void Unregister()
    {
        if (_hwndSource == null) return;
        var h = _hwndSource.Handle;
        UnregisterHotKey(h, LAUNCH_ID);
        UnregisterHotKey(h, KILL_ID);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY)
        {
            var id = wParam.ToInt32();
            if (id == LAUNCH_ID) { _processManager.Launch(_currentConfig); handled = true; }
            else if (id == KILL_ID) { _processManager.Kill(_currentConfig); handled = true; }
        }
        return IntPtr.Zero;
    }

    private static uint VkCode(string key) => key.ToUpperInvariant() switch
    {
        "F11" => 0x7A,
        "F12" => 0x7B,
        _ => 0x7A,
    };
}
