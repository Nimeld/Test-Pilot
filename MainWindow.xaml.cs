using Microsoft.Win32;
using Project_03_TestPilot_20260617.Services;
using System.Windows;
using System.Windows.Input;

namespace Project_03_TestPilot_20260617;

public partial class MainWindow : Window
{
    private ConfigService? _configService;
    private HotKeyService? _hotKeyService;
    private Config? _originalConfig;
    private string _pendingLaunchKey = "F11";
    private string _pendingKillKey = "F12";

    public MainWindow()
    {
        InitializeComponent();
        IsVisibleChanged += OnVisibilityChanged;
    }

    internal void SetDependencies(ConfigService configService, HotKeyService hotKeyService)
    {
        _configService = configService;
        _hotKeyService = hotKeyService;
    }

    internal void LoadConfig()
    {
        var config = _configService?.Load();
        if (config != null)
        {
            _originalConfig = new Config
            {
                TargetAppPath = config.TargetAppPath,
                TargetProcessName = config.TargetProcessName,
                LaunchHotKey = config.LaunchHotKey,
                KillHotKey = config.KillHotKey
            };
            txtPath.Text = config.TargetAppPath;
            _pendingLaunchKey = config.LaunchHotKey;
            _pendingKillKey = config.KillHotKey;
            txtLaunchKey.Text = config.LaunchHotKey;
            txtKillKey.Text = config.KillHotKey;
        }
        txtStatus.Text = "就绪";
    }

    internal void UpdateStatus(string status)
    {
        Dispatcher.Invoke(() => txtStatus.Text = status);
    }

    private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue == false && _originalConfig != null)
            _hotKeyService?.Register(_originalConfig);
    }

    private void BrowseClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*",
            Title = "选择目标程序"
        };
        if (dialog.ShowDialog() == true) txtPath.Text = dialog.FileName;
    }

    private void SaveClick(object sender, RoutedEventArgs e)
    {
        var path = txtPath.Text.Trim();
        if (!string.IsNullOrEmpty(path) && !System.IO.File.Exists(path))
        {
            txtStatus.Text = "⚠️ 文件路径不存在";
            return;
        }

        var config = new Config
        {
            TargetAppPath = path,
            LaunchHotKey = _pendingLaunchKey,
            KillHotKey = _pendingKillKey
        };
        _configService?.Save(config);
        _originalConfig = new Config
        {
            TargetAppPath = config.TargetAppPath,
            TargetProcessName = config.TargetProcessName,
            LaunchHotKey = config.LaunchHotKey,
            KillHotKey = config.KillHotKey
        };
        _hotKeyService?.Register(config);
        txtStatus.Text = "✅ 配置已保存";
    }

    // --- Hotkey capture with combo support ---

    private void KeyBox_GotFocus(object sender, RoutedEventArgs e)
    {
        _hotKeyService?.Unregister();
        if (sender is System.Windows.Controls.TextBox tb)
            tb.Text = "按下热键...";
    }

    private static bool TryGetComboKey(KeyEventArgs e, out string combo)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        var keyName = key.ToString();
        if (!keyName.StartsWith("F") || keyName.Length <= 1 ||
            !int.TryParse(keyName[1..], out var n) || n < 1 || n > 24)
        {
            combo = "";
            return false;
        }

        var parts = new System.Collections.Generic.List<string>();
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Windows)) parts.Add("Win");
        parts.Add(keyName);
        combo = string.Join("+", parts);
        return true;
    }

    private void LaunchKey_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        if (TryGetComboKey(e, out var key))
        {
            _pendingLaunchKey = key;
            txtLaunchKey.Text = key;
        }
    }

    private void KillKey_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        if (TryGetComboKey(e, out var key))
        {
            _pendingKillKey = key;
            txtKillKey.Text = key;
        }
    }
}
