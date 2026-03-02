import 'package:flutter/material.dart';

import '../models/device_info.dart';

class ConnectionPage extends StatefulWidget {
  const ConnectionPage({super.key});

  @override
  State<ConnectionPage> createState() => _ConnectionPageState();
}

class _ConnectionPageState extends State<ConnectionPage> with SingleTickerProviderStateMixin {
  late final AnimationController _pulseController;
  bool _isConnecting = false;

  final List<DeviceInfo> _devices = <DeviceInfo>[
    DeviceInfo(name: 'Hai 的 ThinkPad', address: '00:11:22:33:44:55', signalStrength: -52),
    DeviceInfo(name: '办公室台式机', address: '11:22:33:44:55:66', signalStrength: -64),
  ];

  @override
  void initState() {
    super.initState();
    _pulseController = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 1200),
    )..repeat(reverse: true);
  }

  @override
  void dispose() {
    _pulseController.dispose();
    super.dispose();
  }

  Future<void> _connectToDevice(DeviceInfo device) async {
    if (_isConnecting) {
      return;
    }
    setState(() {
      _isConnecting = true;
    });

    await Future<void>.delayed(const Duration(milliseconds: 600));
    if (!mounted) {
      return;
    }
    Navigator.of(context).pushReplacementNamed('/input', arguments: device.name);
  }

  Future<void> _refreshDevices() async {
    await Future<void>.delayed(const Duration(milliseconds: 700));
    if (!mounted) {
      return;
    }

    setState(() {
      _devices.sort((a, b) => b.signalStrength.compareTo(a.signalStrength));
    });
  }

  void _openUsageHelp() {
    Navigator.of(context).push(
      MaterialPageRoute<void>(builder: (_) => const UsageHelpPage()),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('BT Input'),
        actions: [
          IconButton(
            tooltip: '使用说明',
            onPressed: _openUsageHelp,
            icon: const Icon(Icons.help_outline),
          ),
        ],
      ),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            const SizedBox(height: 20),
            AnimatedBuilder(
              animation: _pulseController,
              builder: (context, child) {
                final scale = 0.9 + (_pulseController.value * 0.2);
                return Transform.scale(scale: scale, child: child);
              },
              child: const Icon(Icons.bluetooth, size: 72, color: Colors.blue),
            ),
            const SizedBox(height: 12),
            Text(_isConnecting ? '连接中...' : '正在搜索附近的 BT Input...'),
            const SizedBox(height: 8),
            const Text(
              '确保电脑端 BT Input 已启动',
              style: TextStyle(color: Colors.black54),
            ),
            const SizedBox(height: 20),
            Expanded(
              child: RefreshIndicator(
                onRefresh: _refreshDevices,
                child: ListView.separated(
                  physics: const AlwaysScrollableScrollPhysics(),
                  itemCount: _devices.length,
                  separatorBuilder: (_, __) => const SizedBox(height: 10),
                  itemBuilder: (context, index) {
                    final device = _devices[index];
                    return Card(
                      child: ListTile(
                        leading: const Icon(Icons.computer),
                        title: Text(device.name),
                        subtitle: Text('信号: ${device.signalStrength} dBm'),
                        trailing: const Icon(Icons.chevron_right),
                        onTap: () => _connectToDevice(device),
                      ),
                    );
                  },
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class UsageHelpPage extends StatelessWidget {
  const UsageHelpPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('使用说明')),
      body: const Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('1. 先在电脑启动 BT Input。'),
            SizedBox(height: 8),
            Text('2. 保持手机与电脑蓝牙开启。'),
            SizedBox(height: 8),
            Text('3. 在连接页点击电脑设备完成连接。'),
            SizedBox(height: 8),
            Text('4. 进入输入页后，输入内容会实时发送到电脑。'),
          ],
        ),
      ),
    );
  }
}
