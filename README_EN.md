<p align="center">
  <img src="app.ico" width="64"/>
  <br><a href="README.md">中文</a> | English
  <br>Launch / kill your target app with a single global hotkey
  <br>A handy tool for developers and testers
</p>

<p align="center">
  <a href="#"><img src="https://img.shields.io/badge/version-1.0.0-blue.svg?style=popout-square" alt="Version"></a>
  <a href="#"><img src="https://img.shields.io/badge/.NET-10.0-512BD4.svg?style=popout-square" alt=".NET"></a>
  <a href="#"><img src="https://img.shields.io/badge/platform-Windows-brightgreen.svg?style=popout-square" alt="Platform"></a>
</p>

---

## Introduction

**TestPilot** is a Windows desktop utility designed for developers and testers who frequently need to launch and kill their target application during testing.

Press a hotkey to instantly start the target app (auto-restart if already running) or kill its process. Every action provides clear on-screen feedback, so you can stay focused on testing without switching windows or reaching for the mouse.

## Features

| Feature | Description |
|---------|-------------|
| 🚀 **One-click launch** | Launch the target app; auto-kill existing process before restarting |
| 🛑 **One-click kill** | Kill all processes of the target app |
| 🔔 **Action feedback** | Centered toast notification on success / failure / exit (1.5s auto-dismiss) |
| 🔧 **Custom hotkeys** | Supports any combination of Ctrl / Alt / Shift / Win + F1~F24 |
| 📋 **Config file** | `appsettings.json` stores target path and hotkeys — edit to switch targets |
| 🎯 **System tray** | Close minimizes to tray; only truly exits via right-click "Exit" |

## Quick Start

### Prerequisites

- Windows 7 or later
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Build and Run

```bash
# Clone
git clone https://github.com/your-username/TestPilot.git
cd TestPilot

# Build
dotnet build

# Run
dotnet run
```

### First Use

1. Launch TestPilot — the settings window opens
2. Click "Browse" to select the target .exe
3. Click the hotkey field and press your desired combination (e.g. `Ctrl+F11`)
4. Click "Save"
5. Press the hotkey to launch / kill the target app

## Configuration

`appsettings.json` (auto-generated in the same directory as the executable):

```json
{
  "TargetAppPath": "C:\\Path\\To\\YourApp.exe",
  "TargetProcessName": "YourApp",
  "LaunchHotKey": "F11",
  "KillHotKey": "F12"
}
```

| Field | Description |
|-------|-------------|
| `TargetAppPath` | Full path to the target executable |
| `TargetProcessName` | Process name (auto-derived from path, can override) |
| `LaunchHotKey` | Launch hotkey |
| `KillHotKey` | Kill hotkey |

> Hotkey format: `Ctrl+F11`, `Alt+F12`, `Shift+F1`, `Ctrl+Shift+F1`, etc. Supports F1~F24.

## Icon

The app icon combines a hammer and gear — symbolizing "testing tool" and "engineering debugging".
