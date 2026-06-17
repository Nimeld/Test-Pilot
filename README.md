<p align="center">
  <img src="app.ico" width="64"/>
  <br>中文 | <a href="README_EN.md">English</a>
  <br>全局热键一键启动 / 关闭被测试软件
  <br>测试开发人员的好帮手
</p>

<p align="center">
  <a href="#"><img src="https://img.shields.io/badge/版本-1.0.0-blue.svg?style=popout-square" alt="版本"></a>
  <a href="#"><img src="https://img.shields.io/badge/.NET-10.0-512BD4.svg?style=popout-square" alt=".NET"></a>
  <a href="#"><img src="https://img.shields.io/badge/平台-Windows-brightgreen.svg?style=popout-square" alt="平台"></a>
</p>

---

## 简介

**TestPilot** 是一款 Windows 桌面小工具，专为需要频繁启动 / 关闭被测试软件的开发者和测试人员设计。

只需按下快捷键，即可一键启动目标程序（已运行则自动重启），一键关闭进程。所有操作都有屏幕居中提示反馈，让你专注在测试本身，不用反复切换窗口手动操作。

## 功能

| 功能 | 说明 |
|------|------|
| 🚀 **一键启动** | 按下热键启动目标程序，已有进程自动先关闭再重启 |
| 🛑 **一键关闭** | 按下热键关闭目标程序所有进程 |
| 🔔 **执行反馈** | 启动成功 / 失败 / 退出时，屏幕中央弹出提示（1.5 秒自动消失） |
| 🔧 **自定义热键** | 支持 Ctrl / Alt / Shift / Win + F1~F24 任意组合 |
| 📋 **配置文件** | `appsettings.json` 记录目标路径和快捷键，修改配置即可切换目标 |
| 🎯 **托盘运行** | 关闭窗口最小化到系统托盘，右键退出才真正退出 |

## 快速开始

### 环境要求

- Windows 7 及以上
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### 构建与运行

```bash
# 克隆
git clone https://github.com/Nimeld/Test-Pilot.git
cd TestPilot

# 构建
dotnet build

# 运行
dotnet run
```

### 首次使用

1. 启动 TestPilot，打开设置窗口
2. 点击「浏览」选择目标 exe 文件
3. 点击热键输入框，按下你想要的组合键（如 `Ctrl+F11`）
4. 点击「保存」
5. 按下热键即可启动 / 关闭目标程序

## 配置

`appsettings.json`（自动生成，位于 exe 同目录）：

```json
{
  "TargetAppPath": "C:\\Path\\To\\YourApp.exe",
  "TargetProcessName": "YourApp",
  "LaunchHotKey": "F11",
  "KillHotKey": "F12"
}
```

| 字段 | 说明 |
|------|------|
| `TargetAppPath` | 目标 exe 完整路径 |
| `TargetProcessName` | 进程名称（自动从路径提取，可覆盖） |
| `LaunchHotKey` | 启动快捷键 |
| `KillHotKey` | 关闭快捷键 |

> 热键格式：`Ctrl+F11`、`Alt+F12`、`Shift+F1`、`Ctrl+Shift+F1` 等，支持 F1~F24


