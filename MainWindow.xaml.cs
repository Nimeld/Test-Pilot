using Microsoft.Win32;
using Project_03_TestPilot_20260617.Services;
using System.Windows;
using System.Windows.Input;

namespace Project_03_TestPilot_20260617;

public partial class MainWindow : Window
{
    private ConfigService? _configService;
    private HotKeyService? _hotKeyService;
    private string _pendingLaunchKey = "F11";
    private string _pendingKillKey = "F12";

    public MainWindow() => InitializeComponent();

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
        _hotKeyService?.Register(config);
        txtStatus.Text = "✅ 配置已保存";
    }

    private void KeyBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.TextBox tb)
            tb.Text = "按下热键...";
    }

    private static bool TryGetFunctionKey(KeyEventArgs e, out string keyName)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        keyName = key.ToString();
        if (keyName.StartsWith("F") && keyName.Length > 1 &&
            int.TryParse(keyName[1..], out var n) && n >= 1 && n <= 24)
            return true;
        keyName = "";
        return false;
    }

    private void LaunchKey_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        if (TryGetFunctionKey(e, out var key))
        {
            _pendingLaunchKey = key;
            txtLaunchKey.Text = key;
        }
    }

    private void KillKey_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        if (TryGetFunctionKey(e, out var key))
        {
            _pendingKillKey = key;
            txtKillKey.Text = key;
        }
    }
}
