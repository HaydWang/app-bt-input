# Project Guidelines

## 项目概述

Windows 和手机之间的蓝牙配对应用：
- **Windows 客户端**: C# (WinUI3/WPF)
- **移动端应用**: Flutter
- **通信协议**: 蓝牙低功耗 (BLE) GATT 服务

## 技术栈

### Windows 端 (C#)
- **UI 框架**: WinUI3 或 WPF
- **蓝牙库**: `Windows.Devices.Bluetooth` (WinUI3) 或 `InTheHand.Net.Bluetooth` (WPF)
- **架构模式**: MVVM (使用 CommunityToolkit.Mvvm)
- **异步**: async/await 模式

### 移动端 (Flutter)
- **蓝牙包**: `flutter_blue_plus` (BLE)
- **状态管理**: Provider/Riverpod/Bloc (待确定)
- **平台**: Android 和 iOS

### 共享组件
- **协议定义**: 统一的 GATT 服务/特征 UUID 列表（在 `/doc` 或 `/shared-docs` 中定义）
- **数据模型**: C# 和 Dart 中对应的数据结构

## 项目结构

```
/windows-client          # C# Windows 客户端
  /Services             # BluetoothService, GattService 等
  /ViewModels           # MVVM ViewModels
  /Models               # 数据模型
  /Views                # UI 视图
/mobile-app             # Flutter 移动应用
  /lib/services         # 蓝牙服务抽象层
  /lib/models           # 数据模型（对应 C# Models）
  /lib/screens          # UI 界面
  /lib/widgets          # 可复用组件
/doc                    # 项目文档和蓝牙协议规范
```

## 蓝牙开发约定

### UUID 管理
- 所有自定义 GATT 服务和特征 UUID 必须在 `/doc` 中集中定义
- 使用常量类/文件管理 UUID，避免硬编码
- 示例：`BluetoothConstants.cs` (C#) 和 `bluetooth_constants.dart` (Flutter)

### 连接管理
- 实现心跳机制监控连接状态
- 添加自动重连逻辑（指数退避策略）
- 设置合理的连接超时时间（建议 10-15 秒）
- 断开连接时正确释放资源

### 数据传输
- BLE 默认 MTU 为 20-23 字节，需协商更大 MTU 或实现分包逻辑
- 使用 GATT 特征通知 (Notify) 而非轮询 (Read)
- 对敏感数据实施应用层加密 (AES)

### 错误处理
- 捕获蓝牙权限被拒绝的情况，引导用户授权
- 处理设备未开启蓝牙的场景
- 对扫描超时、配对失败、连接中断等异常提供用户友好的提示

### 安全性
- 使用蓝牙安全简单配对 (SSP) 或 PIN 码认证
- 实现设备白名单机制（基于设备地址或名称）
- 验证连接设备的服务 UUID 是否匹配

## 构建和测试

### Windows 客户端
```bash
# 待添加：构建、运行和测试命令
```

### Flutter 移动端
```bash
# 安装依赖
flutter pub get

# 运行 Android 版本
flutter run

# 运行 iOS 版本（需要 macOS）
flutter run -d ios

# 待添加：测试命令
```

### 测试策略
- **单元测试**: Mock 蓝牙服务，测试业务逻辑
- **物理设备测试**: 必须在真实设备上测试配对和数据传输
- **调试工具**: 使用 Nordic nRF Connect 调试 BLE GATT 服务

## 关键约定

### 代码风格
- **C#**: 遵循 .NET 标准命名约定（PascalCase）
- **Flutter/Dart**: 遵循 Dart 风格指南（lowerCamelCase）

### 异步编程
- **C#**: 使用 `async/await`，避免阻塞 UI 线程
- **Flutter**: 使用 `Future` 和 `Stream`，蓝牙事件使用 Stream 监听

### 日志记录
- 记录关键蓝牙事件：扫描、连接、断开、数据传输
- 包含设备信息和时间戳便于调试
- 生产环境移除敏感信息日志

## 文档位置

- **协议规范**: `/doc` 文件夹（由开发者提供）
- **API 文档**: 待定
- **更新日志**: 待定
