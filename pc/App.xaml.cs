using System.Windows;
using System.Text;
using BtInput.Core;
using BtInput.Helpers;
using BtInput.Protocol;
using BtInput.UI;

namespace BtInput;

public partial class App : System.Windows.Application
{
    private TrayManager? _trayManager;
    private HotkeyManager? _hotkeyManager;
    private BleManager? _bleManager;
    private ProtocolDecoder? _protocolDecoder;
    private TextInjector? _textInjector;
    private bool _isActivated;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _trayManager = new TrayManager();
        _trayManager.ExitRequested += (_, _) => Shutdown();
        _trayManager.ToggleRequested += (_, _) => ToggleActivation();
        _trayManager.UpdateState(TrayState.Disconnected);

        _hotkeyManager = new HotkeyManager();
        _hotkeyManager.HotkeyPressed += (_, _) => ToggleActivation();
        _hotkeyManager.Register(Constants.DefaultHotkeyModifiers, Constants.DefaultHotkeyVirtualKey);

        _protocolDecoder = new ProtocolDecoder();
        _textInjector = new TextInjector();
        _bleManager = new BleManager();
        _bleManager.TextDataReceived += OnTextDataReceived;
        _bleManager.ConnectionChanged += OnConnectionChanged;
    }

    private async void OnConnectionChanged(bool connected)
    {
        if (connected)
        {
            _trayManager?.UpdateState(_isActivated ? TrayState.Active : TrayState.Connected, deviceName: _bleManager?.ConnectedDeviceName, activated: _isActivated);
            if (_bleManager is not null)
            {
                await _bleManager.SendControlAsync(Encoding.UTF8.GetBytes("{\"t\":129}"));
            }
        }
        else
        {
            _trayManager?.UpdateState(TrayState.Disconnected);
        }
    }

    private async void OnTextDataReceived(byte[] bytes)
    {
        if (!_isActivated || _protocolDecoder is null || _textInjector is null || _bleManager is null)
        {
            return;
        }

        var message = _protocolDecoder.Decode(bytes);
        if (message is TextDeltaMessage textDeltaMessage)
        {
            _textInjector.HandleDelta(textDeltaMessage);
            _trayManager?.UpdateState(TrayState.Active, deviceName: _bleManager.ConnectedDeviceName, activated: true);
        }

        if (_protocolDecoder.SequenceGapDetected)
        {
            await _bleManager.SendControlAsync(Encoding.UTF8.GetBytes("{\"t\":131}"));
        }
    }

    private async void ToggleActivation()
    {
        _isActivated = !_isActivated;
        var trayState = _isActivated ? TrayState.Active : TrayState.Connected;
        _trayManager?.UpdateState(trayState, deviceName: "示例设备", activated: _isActivated);

        if (_bleManager is not null)
        {
            var controlPayload = _isActivated ? "{\"t\":129}" : "{\"t\":130}";
            await _bleManager.SendControlAsync(Encoding.UTF8.GetBytes(controlPayload));
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _hotkeyManager?.Dispose();
        _trayManager?.Dispose();
        if (_bleManager is not null)
        {
            _ = _bleManager.DisposeAsync();
        }
        base.OnExit(e);
    }
}

