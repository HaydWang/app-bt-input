using BtInput.Core;
using BtInput.Protocol;

namespace BtInput.Tests;

public class TextInjectorTests
{
    [Fact]
    public void InjectText_ShortText_UsesUnicodeInput()
    {
        var fakeInput = new FakeInputSender();
        var injector = new TextInjector(fakeInput, new FakeClipboardService());

        injector.InjectText("你好");

        Assert.Equal(1, fakeInput.UnicodeCalls);
        Assert.Equal("你好", fakeInput.LastUnicodeText);
    }

    [Fact]
    public void InjectText_LongText_UsesClipboardShortcut()
    {
        var fakeInput = new FakeInputSender();
        var injector = new TextInjector(fakeInput, new FakeClipboardService());

        injector.InjectText("今天天气真不错我们去公园散步吧");

        Assert.Equal(1, fakeInput.ShortcutCalls);
        Assert.Equal((ushort)0x56, fakeInput.LastShortcutKey);
    }

    [Fact]
    public void HandleDelta_Delete_UsesBackspace()
    {
        var fakeInput = new FakeInputSender();
        var injector = new TextInjector(fakeInput, new FakeClipboardService());

        injector.HandleDelta(new TextDeltaMessage
        {
            Op = DeltaOp.Delete,
            DeleteCount = 3
        });

        Assert.Equal(3, fakeInput.VirtualKeyCalls);
    }

    private sealed class FakeInputSender : IInputSender
    {
        public int UnicodeCalls { get; private set; }
        public int VirtualKeyCalls { get; private set; }
        public int ShortcutCalls { get; private set; }
        public string LastUnicodeText { get; private set; } = string.Empty;
        public ushort LastShortcutKey { get; private set; }

        public void SendUnicodeText(string text)
        {
            UnicodeCalls++;
            LastUnicodeText = text;
        }

        public void SendVirtualKey(ushort virtualKey)
        {
            VirtualKeyCalls++;
        }

        public void SendShortcut(ushort modifier, ushort key)
        {
            ShortcutCalls++;
            LastShortcutKey = key;
        }
    }

    private sealed class FakeClipboardService : IClipboardService
    {
        private string _text = string.Empty;

        public bool HasText() => !string.IsNullOrEmpty(_text);

        public System.Windows.IDataObject? GetDataObject() => null;

        public void SetText(string text)
        {
            _text = text;
        }

        public void Restore(System.Windows.IDataObject dataObject)
        {
        }
    }
}
