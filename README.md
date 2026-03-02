# BT Input

BT Input 是一个把手机变成 Windows 蓝牙输入设备的工具。用户在手机上使用任意系统输入法输入文字，文本会通过 BLE 实时注入到 PC 当前光标位置。

## What is BT Input

- 手机端：Flutter 应用，负责采集输入和 BLE 发送
- PC 端：WPF 应用，负责 BLE 接收、协议解码和文本注入
- 传输方式：BLE GATT，离线直连，不依赖 Wi-Fi 或互联网

## System Requirements

### PC

- Windows 10/11（支持 BLE）
- Bluetooth 已开启
- 安装 VC 运行时（如目标机器缺少 .NET 运行所需组件）

### Phone

- Android 8.0+
- 蓝牙权限已授权
- 蓝牙已开启

## Installation

### 安装 PC 端

1. 下载发布包中的 `BtInput.exe`。
2. 双击运行，确认系统托盘出现 BT Input 图标。
3. 首次运行根据提示开启蓝牙与相关权限。

### 安装手机端

1. 安装发布的 Android APK。
2. 打开应用并授予蓝牙权限。
3. 在连接页选择目标 PC 设备。

## Usage Guide

1. 先在 PC 端启动 BT Input（托盘图标出现）。
2. 手机打开 BT Input，进入连接页并连接到目标电脑。
3. 进入输入页后，在文本框中输入内容。
4. 文本将实时出现在 PC 当前焦点输入框。
5. 使用 `Ctrl+Shift+B` 可在 PC 端切换激活/暂停输入。

## Screenshots

- 手机连接页：`docs/screenshots/phone-connection.png`（待补）
- 手机输入页：`docs/screenshots/phone-input.png`（待补）
- PC 托盘/浮动条：`docs/screenshots/pc-floating-bar.png`（待补）

## Troubleshooting FAQ

### 手机搜不到电脑怎么办？

- 确认 PC 端程序已启动且蓝牙开启。
- 关闭并重开手机蓝牙后下拉刷新设备列表。
- 电脑蓝牙适配器异常时，重启蓝牙服务或重启电脑。

### 连接后无法输入到目标应用？

- 确认目标应用窗口已聚焦。
- 确认 PC 端未被热键切换到暂停状态。
- 对部分高权限窗口，尝试以管理员身份运行 PC 端。

### 输入延迟或丢字怎么办？

- 减少与电脑间遮挡，缩短距离。
- 关闭高干扰蓝牙设备后重试。
- 断开重连，等待自动同步完成。

### 手机端提示权限问题怎么办？

- 进入系统设置，手动授予蓝牙与附近设备权限。
- iOS/Android 均需允许蓝牙访问后才能正常连接。
