# TestPilot

> 测试启动/关闭工具 — 全局热键一键控制被测试软件

## 功能

- **F11**（可自定义）启动目标程序 — 已有进程先静默关闭再重新启动，启动后验证进程是否成功运行
- **F12**（可自定义）关闭目标程序进程
- 启动成功/失败/退出时屏幕中央弹出提示（1.5 秒自动消失）
- 系统托盘运行，关闭窗口最小化到托盘
- 自定义组合键：支持 Ctrl/Alt/Shift/Win + F1~F24

## 使用

1. 启动 TestPilot，设置目标 exe 路径
2. 点击热键输入框，按下需要的组合键（如 `Ctrl+F11`）
3. 点击保存
4. 按热键启动/关闭目标程序

## 配置

`appsettings.json`（自动生成，与 exe 同目录）：

```json
{
  "TargetAppPath": "C:\\Path\\To\\YourApp.exe",
  "TargetProcessName": "YourApp",
  "LaunchHotKey": "F11",
  "KillHotKey": "F12"
}
```

## 构建

```bash
dotnet build
dotnet run
```

要求：.NET 10 SDK
