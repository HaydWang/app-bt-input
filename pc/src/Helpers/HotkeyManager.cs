using System.Windows.Interop;

namespace BtInput.Helpers;

public sealed class HotkeyManager : IDisposable
{
    private const int HotkeyId = 1001;

    private HwndSource? _source;
    private bool _registered;

    public event EventHandler? HotkeyPressed;

    public void Register(uint modifiers, uint virtualKey)
    {
        if (_registered)
        {
            return;
        }

        var parameters = new HwndSourceParameters("BtInputHotkeyWindow")
        {
            Width = 0,
            Height = 0,
            WindowStyle = 0
        };

        _source = new HwndSource(parameters);
        _source.AddHook(WndProc);

        _registered = NativeMethods.RegisterHotKey(
            _source.Handle,
            HotkeyId,
            modifiers,
            virtualKey);

        if (!_registered)
        {
            throw new InvalidOperationException("Failed to register global hotkey.");
        }
    }

    public void Unregister()
    {
        if (_source is null)
        {
            return;
        }

        if (_registered)
        {
            NativeMethods.UnregisterHotKey(_source.Handle, HotkeyId);
            _registered = false;
        }

        _source.RemoveHook(WndProc);
        _source.Dispose();
        _source = null;
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == NativeMethods.WM_HOTKEY && wParam.ToInt32() == HotkeyId)
        {
            handled = true;
            HotkeyPressed?.Invoke(this, EventArgs.Empty);
        }

        return IntPtr.Zero;
    }

    public void Dispose()
    {
        Unregister();
    }
}
