namespace BtInput.Helpers;

public static class Constants
{
    public const string BleServiceUuid = "0000FFF0-0000-1000-8000-00805F9B34FB";
    public const string TextCharacteristicUuid = "0000FFF1-0000-1000-8000-00805F9B34FB";
    public const string ControlCharacteristicUuid = "0000FFF2-0000-1000-8000-00805F9B34FB";
    public const string StatusCharacteristicUuid = "0000FFF3-0000-1000-8000-00805F9B34FB";

    public const uint DefaultHotkeyModifiers = NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT;
    public const uint DefaultHotkeyVirtualKey = 0x42; // B

    public static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(15);
    public static readonly TimeSpan ReconnectBaseDelay = TimeSpan.FromSeconds(1);
}
