using System.Diagnostics;
using System.IO;

namespace Project_03_TestPilot_20260617.Services;

public enum LaunchResult
{
    Success,
    Failed,
    ConfigInvalid,
}

public class ProcessManager
{
    public event Action<string>? StatusChanged;

    public async Task<LaunchResult> LaunchAsync(Config config)
    {
        if (string.IsNullOrEmpty(config.TargetAppPath) || !File.Exists(config.TargetAppPath))
        {
            StatusChanged?.Invoke("配置无效或文件不存在");
            return LaunchResult.ConfigInvalid;
        }

        var processName = Path.GetFileNameWithoutExtension(config.TargetAppPath);

        foreach (var p in Process.GetProcessesByName(processName))
        {
            try { p.Kill(entireProcessTree: true); p.WaitForExit(3000); } catch { }
        }

        try
        {
            Process.Start(new ProcessStartInfo { FileName = config.TargetAppPath, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke($"启动失败: {ex.Message}");
            return LaunchResult.Failed;
        }

        await Task.Delay(800);

        if (Process.GetProcessesByName(processName).Length > 0)
        {
            StatusChanged?.Invoke($"已启动: {Path.GetFileName(config.TargetAppPath)}");
            return LaunchResult.Success;
        }
        else
        {
            StatusChanged?.Invoke("启动失败: 未检测到目标进程");
            return LaunchResult.Failed;
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
