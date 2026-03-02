using BtInput.Helpers;

namespace BtInput.Core;

public sealed class FocusTracker
{
    public (int X, int Y, int Width, int Height)? GetCaretScreenPosition()
    {
        var info = new NativeMethods.GUITHREADINFO
        {
            cbSize = System.Runtime.InteropServices.Marshal.SizeOf<NativeMethods.GUITHREADINFO>()
        };

        var ok = NativeMethods.GetGUIThreadInfo(0, ref info);
        if (!ok)
        {
            return null;
        }

        var rect = info.rcCaret;
        if (rect.Left == 0 && rect.Top == 0 && rect.Right == 0 && rect.Bottom == 0)
        {
            return null;
        }

        return (rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
    }
}
