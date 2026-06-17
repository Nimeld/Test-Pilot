using System.Diagnostics;
using System.IO;

namespace Project_03_TestPilot_20260617.Services;

public class ProcessManager
{
    public event Action<string>? StatusChanged;

    public void Launch(Config config)
    {
        if (string.IsNullOrEmpty(config.TargetAppPath))
        {
            StatusChanged?.Invoke("未设置目标程序路径");
            return;
        }

        if (!File.Exists(config.TargetAppPath))
        {
            StatusChanged?.Invoke("文件不存在");
            return;
        }

        var processName = Path.GetFileNameWithoutExtension(config.TargetAppPath);
        if (Process.GetProcessesByName(processName).Length > 0)
        {
            StatusChanged?.Invoke($"程序已在运行 ({processName})");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo { FileName = config.TargetAppPath, UseShellExecute = true });
            StatusChanged?.Invoke($"已启动: {Path.GetFileName(config.TargetAppPath)}");
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke($"启动失败: {ex.Message}");
        }
    }

    public void Kill(Config config)
    {
        var name = config.TargetProcessName;
        if (string.IsNullOrEmpty(name))
        {
            StatusChanged?.Invoke("未设置进程名称");
            return;
        }

        var procs = Process.GetProcessesByName(name);
        if (procs.Length == 0)
        {
            StatusChanged?.Invoke($"未找到进程: {name}");
            return;
        }

        foreach (var p in procs)
        {
            try { p.Kill(entireProcessTree: true); p.WaitForExit(3000); }
            catch (Exception ex) { StatusChanged?.Invoke($"关闭失败: {ex.Message}"); }
        }
        StatusChanged?.Invoke($"已关闭 {procs.Length} 个进程 ({name})");
    }
}
