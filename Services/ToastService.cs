using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Project_03_TestPilot_20260617.Services;

public static class ToastService
{
    public static void Show(string message, int durationMs = 1500)
    {
        var toast = new Window
        {
            WindowStyle = WindowStyle.None,
            AllowsTransparency = true,
            Background = null,
            Topmost = true,
            ShowInTaskbar = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.NoResize,
        };

        var border = new Border
        {
            Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 0, 0, 0)),
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(36, 18, 36, 18),
            Child = new TextBlock
            {
                Text = message,
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
            }
        };

        toast.Content = border;

        toast.Loaded += async (_, _) =>
        {
            await Task.Delay(durationMs);
            toast.Close();
        };

        toast.Show();
    }
}
