using Project_03_TestPilot_20260617.Services;
using System.Windows;

namespace Project_03_TestPilot_20260617;

public partial class App : Application
{
    private TrayService? _trayService;
    private HotKeyService? _hotKeyService;
    private ProcessManager? _processManager;
    private ConfigService _configService = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var config = _configService.Load();
        var mainWindow = new MainWindow();

        _processManager = new ProcessManager();
        _hotKeyService = new HotKeyService(mainWindow, _processManager);
        _hotKeyService.Initialize();
        _hotKeyService.Register(config);

        _trayService = new TrayService(mainWindow);
        _trayService.Initialize();

        _processManager.StatusChanged += status =>
        {
            _trayService.SetTip(status);
            mainWindow.UpdateStatus(status);
        };

        mainWindow.SetDependencies(_configService, _hotKeyService);
        mainWindow.LoadConfig();

        mainWindow.Hide();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _hotKeyService?.Unregister();
        _trayService?.Dispose();
        base.OnExit(e);
    }
}
