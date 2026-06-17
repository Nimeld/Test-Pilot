using Microsoft.Win32;
using Project_03_TestPilot_20260617.Services;
using System.Windows;

namespace Project_03_TestPilot_20260617;

public partial class MainWindow : Window
{
    private ConfigService? _configService;
    private HotKeyService? _hotKeyService;

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

        var config = new Config { TargetAppPath = path };
        _configService?.Save(config);
        _hotKeyService?.Register(config);
        txtStatus.Text = "✅ 配置已保存";
    }
}
