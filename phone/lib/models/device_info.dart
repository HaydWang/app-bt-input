class DeviceInfo {
  const DeviceInfo({
    required this.name,
    required this.address,
    required this.rssi,
  });

  // TODO: Add BLE metadata fields if needed by scan/connect UX.
  final String name;
  final String address;
  final int rssi;
}
