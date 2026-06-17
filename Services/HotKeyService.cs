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

    private const uint MOD_ALT = 0x0001;
    private const uint MOD_CTRL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint MOD_WIN = 0x0008;

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

        var (modL, vkL) = ParseCombo(config.LaunchHotKey);
        var (modK, vkK) = ParseCombo(config.KillHotKey);

        if (!RegisterHotKey(h, LAUNCH_ID, modL, vkL))
            MessageBox.Show($"热键 {config.LaunchHotKey} 注册失败，可能被占用", "TestPilot");
        if (!RegisterHotKey(h, KILL_ID, modK, vkK))
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

    private static (uint mod, uint vk) ParseCombo(string combo)
    {
        uint mod = 0;
        uint vk = 0x7A;
        var parts = combo.Split('+');
        foreach (var part in parts)
        {
            var p = part.Trim().ToUpperInvariant();
            switch (p)
            {
                case "CTRL":  mod |= MOD_CTRL; break;
                case "ALT":   mod |= MOD_ALT; break;
                case "SHIFT": mod |= MOD_SHIFT; break;
                case "WIN":   mod |= MOD_WIN; break;
                default:
                    vk = p switch
                    {
                        "F1" => 0x70,  "F2" => 0x71,  "F3" => 0x72,  "F4" => 0x73,
                        "F5" => 0x74,  "F6" => 0x75,  "F7" => 0x76,  "F8" => 0x77,
                        "F9" => 0x78,  "F10" => 0x79, "F11" => 0x7A, "F12" => 0x7B,
                        "F13" => 0x7C, "F14" => 0x7D, "F15" => 0x7E, "F16" => 0x7F,
                        "F17" => 0x80, "F18" => 0x81, "F19" => 0x82, "F20" => 0x83,
                        "F21" => 0x84, "F22" => 0x85, "F23" => 0x86, "F24" => 0x87,
                        _ => 0x7A,
                    };
                    break;
            }
        }
        return (mod, vk);
    }
}
