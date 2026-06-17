using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using Application = System.Windows.Application;

namespace Project_03_TestPilot_20260617.Services;

public class TrayService : IDisposable
{
    private NotifyIcon? _icon;
    private readonly Window _window;

    public TrayService(Window window) => _window = window;

    public void Initialize()
    {
        _icon = new NotifyIcon
        {
            Icon = LoadCustomIcon(),
            Text = "TestPilot",
            Visible = true
        };

        _icon.DoubleClick += (_, _) => ShowWindow();

        var menu = new ContextMenuStrip();
        menu.Items.Add("设置", null, (_, _) => ShowWindow());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("退出", null, (_, _) => ExitApp());
        _icon.ContextMenuStrip = menu;

        _window.Closing += (_, e) => { e.Cancel = true; _window.Hide(); };
    }

    private static Icon LoadCustomIcon()
    {
        try
        {
            var path = System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (System.IO.File.Exists(path))
                return new Icon(path);
        }
        catch { }
        return SystemIcons.Application;
    }

    private void ShowWindow()
    {
        _window.Show();
        _window.WindowState = WindowState.Normal;
        _window.Activate();
    }

    private void ExitApp()
    {
        if (_icon != null) _icon.Visible = false;
        Application.Current.Shutdown();
    }

    public void SetTip(string text)
    {
        if (_icon != null) _icon.Text = text.Length > 63 ? text[..63] : text;
    }

    public void Dispose()
    {
        _icon?.Dispose();
        _icon = null;
    }
}
