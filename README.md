# BT Input

将智能手机变成 Windows PC 的无线输入设备，通过蓝牙低功耗 (BLE) 实现实时文字输入。

## 项目概述

用户在手机上使用**任何系统输入法**（拼音、语音、手写、Gboard 等）输入文字，文字实时出现在 PC 光标位置。无需 WiFi，无需互联网，仅通过 BLE 直连。

**技术栈:**

- **手机端**: Flutter 3.x / Dart (BLE Peripheral)
- **PC 端**: C# WPF / .NET 8 (BLE Central)
- **通信协议**: JSON over BLE GATT

## 快速开始

### 初始化项目

```bash
cd bt-input
claude
```

### 开发工作流

#### 手机端开发

```bash
# 创建 Flutter 项目骨架
> /phone create the Flutter project skeleton with directory structure,
>   pubspec.yaml with flutter_blue_plus, and placeholder files

# 实现核心算法
> /phone implement DiffEngine with all 7 scenarios and unit tests

# 实现节流发送器
> /phone implement ThrottledDiffSender with 50ms throttle window
```

#### PC 端开发

```bash
# 创建 WPF 项目
> /pc create WPF project with system tray, hotkey Ctrl+Shift+B, and app skeleton

# 实现 BLE Central
> /pc implement BleManager with scan, connect, and auto-reconnect

# 实现文本注入
> /pc implement TextInjector with SendInput and clipboard injection
```

#### 验证与测试

```bash
# 验证协议实现
> /test-protocol all

# 端到端集成测试
> /e2e basic text input
```

#### 会话管理

```bash
# 会话结束前保存进度
> /handoff

# 下次开始时恢复上下文
> /catchup
```

## 项目结构

```
bt-input/
├── phone/                  # Flutter 手机端
│   ├── lib/
│   │   ├── core/          # DiffEngine, ThrottledDiffSender, Protocol
│   │   ├── services/      # BLE Service, ConnectionManager
│   │   ├── pages/         # UI 页面
│   │   ├── models/        # 数据模型
│   │   └── utils/         # 常量、日志
│   └── test/              # 单元测试
├── pc/                     # C# WPF PC 端
│   ├── src/
│   │   ├── Core/          # BleManager, TextInjector, ProtocolDecoder
│   │   ├── UI/            # FloatingBar, TrayManager
│   │   ├── Protocol/      # 消息定义
│   │   └── Helpers/       # P/Invoke, 热键管理
│   └── tests/             # 单元测试
├── docs/                   # 详细文档
│   ├── PRD.md             # 产品需求
│   ├── ARCHITECTURE.md    # 系统架构
│   ├── PROTOCOL.md        # BLE 协议规范
│   └── LOW_LEVEL_DESIGN.md # 底层实现细节
├── .claude/commands/       # Claude 开发命令
└── CLAUDE.md              # 项目总览和开发指南

```

## 构建和运行

### 手机端 (Flutter)

```bash
cd phone
flutter pub get
flutter run                # 运行在连接的设备/模拟器
flutter test               # 运行单元测试
flutter analyze            # 静态分析
dart format lib/ test/     # 格式化代码
flutter build apk          # 构建 Android release
flutter build ios          # 构建 iOS (需要 macOS)
```

### PC 端 (C# WPF .NET 8)

```bash
cd pc
dotnet restore
dotnet build
dotnet run                 # 运行应用
dotnet test                # 运行单元测试
dotnet format              # 格式化代码
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 核心特性

- ✅ **BLE 直连**: 完全离线，无需 WiFi 或互联网
- ✅ **系统 IME 支持**: 支持任何手机输入法（拼音、语音、手写等）
- ✅ **智能差分算法**: O(N) 前缀+后缀匹配，覆盖 95%+ 真实输入场景
- ✅ **50ms 节流**: 优化传输效率，批量发送变更
- ✅ **大文本支持**: 自动使用剪贴板注入 >10 字符的文本
- ✅ **自动清空**: 手机输入框 500 字符+2 秒空闲后自动清空
- ✅ **自动重连**: 断线后指数退避重连（1/2/4/8/16 秒）
- ✅ **浮动状态栏**: WPF 透明窗口，不抢占焦点
- ✅ **全局热键**: Ctrl+Shift+B 激活/停用

## 文档

- **[CLAUDE.md](CLAUDE.md)**: 项目总览、架构、协议速查、开发规范
- **[.github/copilot-instructions.md](.github/copilot-instructions.md)**: GitHub Copilot AI 代理指导
- **详细文档** (待创建):
  - `docs/PRD.md` - 产品需求和 UI/UX 规范
  - `docs/ARCHITECTURE.md` - 系统架构和组件交互
  - `docs/PROTOCOL.md` - BLE GATT 协议完整规范
  - `docs/LOW_LEVEL_DESIGN.md` - DiffEngine、TextInjector 等核心算法

## MVP 开发阶段

1. ✅ Flutter 项目骨架 + BLE 服务桩
2. ⬜ DiffEngine + 单元测试（全部 7 种场景）
3. ⬜ ThrottledDiffSender + 协议编码器
4. ⬜ 手机端 UI: ConnectionPage、InputPage、SettingsPage
5. ⬜ C# WPF 项目骨架 + 系统托盘 + 热键
6. ⬜ PC BLE Central（扫描、连接、订阅、自动重连）
7. ⬜ PC TextInjector（SendInput + 剪贴板注入）
8. ⬜ PC FloatingBar（WPF 透明窗口、光标跟踪）
9. ⬜ PC 协议解码器 + 消息分发
10. ⬜ 端到端集成测试

## 许可证

待定

## 贡献

待定
