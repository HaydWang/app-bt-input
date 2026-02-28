---
description: Work on the C# WPF PC app (pc/ directory)
argument-hint: <describe the task>
---

You are working on the **pc/** C# WPF (.NET 8) project for BT Input.

## Context Loading

1. Read `CLAUDE.md` for project overview and conventions
2. If the task involves BLE communication, also read `docs/PROTOCOL.md`
3. If the task involves TextInjector, FloatingBar, or FocusTracker, also read `docs/LOW_LEVEL_DESIGN.md`
4. If the task involves UI behavior or user experience, also read `docs/PRD.md` section 3.3 (PC UI)

## Key Source Files

```
pc/src/
├── App.xaml / App.xaml.cs            # WPF entry point
├── Core/
│   ├── BleManager.cs                 # BLE Central (WinRT APIs)
│   ├── TextInjector.cs               # SendInput + Clipboard injection
│   ├── DiffEngine.cs                 # (optional) PC-side mirror text tracking
│   ├── FocusTracker.cs               # GetGUIThreadInfo, caret position
│   └── ProtocolDecoder.cs            # JSON message deserialization
├── UI/
│   ├── FloatingBar.xaml/.cs          # Floating status bar (WS_EX_NOACTIVATE)
│   ├── TrayManager.cs                # System tray NotifyIcon
│   ├── SettingsWindow.xaml/.cs       # Settings/pairing dialog
│   └── FirstRunWindow.xaml/.cs       # First-time pairing guide
├── Protocol/
│   └── Messages.cs                   # Message type definitions & enums
└── Helpers/
    ├── NativeMethods.cs              # All P/Invoke declarations
    ├── HotkeyManager.cs              # Global hotkey (Ctrl+Shift+B)
    └── Constants.cs                  # UUIDs, timeouts, thresholds
```

## Workflow

1. Implement the requested changes
2. Run `cd pc && dotnet build` — fix all compilation errors
3. Run `cd pc && dotnet test` — fix all test failures
4. Run `cd pc && dotnet format` — ensure formatting
5. Summarize what was done

## Key APIs

- **BLE**: `Windows.Devices.Bluetooth.GenericAttributeProfile` (WinRT, accessed natively from .NET 8)
- **Keyboard simulation**: `user32.dll → SendInput` with `KEYEVENTF_UNICODE`
- **Caret tracking**: `user32.dll → GetGUIThreadInfo` → `GUITHREADINFO.rcCaret`
- **No-focus window**: `SetWindowLong` with `WS_EX_NOACTIVATE | WS_EX_TOPMOST | WS_EX_TOOLWINDOW`
- **Global hotkey**: `user32.dll → RegisterHotKey / UnregisterHotKey`
- **Clipboard**: `System.Windows.Clipboard` (save → set → Ctrl+V → restore)

## Rules

- NEVER create a real Windows IME (TSF). We use SendInput approach
- All P/Invoke signatures go in `NativeMethods.cs`
- All public APIs must have XML doc comments (`/// <summary>`)
- Use `async/await` for BLE operations; suffix async methods with `Async`
- FloatingBar must use `WS_EX_NOACTIVATE` — it must NEVER steal focus from other apps
- Enable nullable reference types project-wide

## Task

$ARGUMENTS
