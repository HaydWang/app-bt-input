# CLAUDE.md — BT Input

## What is BT Input

A tool that turns a smartphone into a wireless input device for Windows PCs via Bluetooth Low Energy.
Users type on their phone using **any system IME** (Pinyin, voice, handwriting, Gboard, etc.), text appears in real-time at the PC's active cursor position. No WiFi, no internet — BLE direct connection only.

## Architecture

```
Phone (Flutter 3.x / Dart)              PC (C# WPF / .NET 8)
┌─────────────────────────┐             ┌─────────────────────────┐
│ TextField → onChanged   │             │ BLE Central (WinRT)     │
│ DiffEngine (prefix+     │── BLE ────▶│ ProtocolDecoder         │
│   suffix O(N) diff)     │  GATT      │ TextInjector            │
│ ThrottledDiffSender     │  Notify    │   (SendInput / Ctrl+V)  │
│ BLE Peripheral (GATT    │             │ FloatingBar (WPF)       │
│   Server)               │             │ SystemTray + HotKey     │
│ flutter_blue_plus       │             │   (Ctrl+Shift+B)        │
└─────────────────────────┘             └─────────────────────────┘
```

- **Phone**: `phone/` — BLE Peripheral (GATT Server), hosts input TextField, runs DiffEngine + ThrottledDiffSender
- **PC**: `pc/` — BLE Central, receives text deltas, injects via SendInput or Clipboard, shows floating bar

## Key Design Decisions

1. PC is **NOT a real IME/TSF**. It is a normal WPF app that simulates IME behavior (Approach C: tray app + floating bar + SendInput with `KEYEVENTF_UNICODE`)
2. Phone does **NOT implement its own input method**. It uses a `TextField` to invoke the system IME, then watches text changes via `onChanged`
3. Protocol: **JSON over BLE GATT** (MVP). Binary format reserved for v2
4. Diff algorithm: **prefix + suffix** matching, O(N), covers 95%+ of real input scenarios
5. Large text (>10 chars): PC uses **clipboard injection** (save clipboard → set text → Ctrl+V → restore clipboard)
6. Phone input box **auto-clears** at 500 chars + 2s idle to support unlimited-length input sessions
7. Reliability: **no per-packet ACK**. On sequence gap → PC sends SYNC_REQUEST → phone replies FULLSYNC

## BLE GATT Service

| Element | UUID | Direction | Property |
|---------|------|-----------|----------|
| Service | `0000FFF0-0000-1000-8000-00805F9B34FB` | — | — |
| Text Char | `0000FFF1-…` | Phone→PC | NOTIFY |
| Control Char | `0000FFF2-…` | PC→Phone | WRITE |
| Status Char | `0000FFF3-…` | Phone→PC | NOTIFY |

## Protocol Quick Reference

**Message types** (Phone→PC): TEXT_DELTA `0x01`, TEXT_FULLSYNC `0x02`, HEARTBEAT `0x03`, SEGMENT_COMPLETE `0x06`
**Message types** (PC→Phone): ACTIVATE `0x81`, DEACTIVATE `0x82`, SYNC_REQUEST `0x83`, CLEAR `0x84`

**TEXT_DELTA JSON**:

```json
{"t":1, "s":42, "o":"A", "p":0, "n":0, "d":"你好", "c":false}
```

- `o`: `A`=Append, `I`=Insert, `D`=Delete, `R`=Replace
- `c`: clipboard hint (`true` when `d.length > 10`)

**Throttle**: 50ms window. First change fires immediately; subsequent changes within window batch to latest state.

**Heartbeat**: every 5s. PC declares disconnect after 15s silence. Auto-reconnect with exponential backoff (1/2/4/8/16s, max 5 attempts).

## Build & Run

### Phone (Flutter)

```bash
cd phone
flutter pub get
flutter run                # run on connected device/emulator
flutter test               # run unit tests
flutter analyze            # static analysis
dart format lib/ test/      # format code
flutter build apk          # build Android release
flutter build ios          # build iOS (macOS only)
```

### PC (C# WPF .NET 8)

```bash
cd pc
dotnet restore
dotnet build
dotnet run                 # run the app
dotnet test                # run unit tests
dotnet format              # format code
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Code Conventions

### Dart (phone/)

- Follow `flutter_lints` (included via `analysis_options.yaml`)
- Format with `dart format`
- Dartdoc comments (`///`) on all public classes, methods, and properties
- File naming: `snake_case.dart`
- Class naming: `PascalCase`
- Use `final` wherever possible; prefer immutable data classes

### C# (pc/)

- Follow standard .NET naming conventions
- Format with `dotnet format`
- XML doc comments (`///`) on all public classes, methods, and properties
- File naming: `PascalCase.cs`
- Use `nullable reference types` (enabled project-wide)
- Async methods: suffix with `Async`, return `Task` or `Task<T>`
- P/Invoke declarations: group in a static class `NativeMethods`

## Project Documentation

For detailed specs, Claude should read these files as needed:

- **Product requirements & UI/UX**: `docs/PRD.md` — read when implementing user-facing features or making product decisions
- **System architecture**: `docs/ARCHITECTURE.md` — read when creating new modules, understanding component interactions, or making structural changes
- **BLE protocol spec**: `docs/PROTOCOL.md` — read when implementing any BLE communication, message encoding/decoding, or sync logic
- **Low-level implementation details**: `docs/LOW_LEVEL_DESIGN.md` — read when implementing DiffEngine, TextInjector, ThrottledDiffSender, or other core algorithms

## Phase Plan (MVP)

1. Flutter project skeleton + BLE service stubs
2. DiffEngine + unit tests (all 7 scenarios)
3. ThrottledDiffSender + Protocol encoder
4. Phone UI: ConnectionPage, InputPage, SettingsPage
5. C# WPF project skeleton + system tray + hotkey
6. PC BLE Central (scan, connect, subscribe, auto-reconnect)
7. PC TextInjector (SendInput + clipboard injection)
8. PC FloatingBar (WPF transparent window, caret tracking)
9. PC Protocol decoder + message dispatch
10. End-to-end integration testing

## Do NOT

- Do NOT create a Windows IME (TSF/IMM). We use SendInput
- Do NOT implement speech recognition or any input method on the phone. We rely on system IME via TextField
- Do NOT use WiFi, WebSocket, or internet. BLE-only, fully offline
- Do NOT add cloud services, analytics, or telemetry
- Do NOT modify files outside `phone/` and `pc/` directories without asking
- Do NOT skip running tests after making changes
- Do NOT auto-merge PRs or push to main
