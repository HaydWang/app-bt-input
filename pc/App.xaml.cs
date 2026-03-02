using System.Windows;
using BtInput.Helpers;
using BtInput.UI;

namespace BtInput;

public partial class App : System.Windows.Application
{
	private TrayManager? _trayManager;
	private HotkeyManager? _hotkeyManager;
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
	}

	private void ToggleActivation()
	{
		_isActivated = !_isActivated;
		var trayState = _isActivated ? TrayState.Active : TrayState.Connected;
		_trayManager?.UpdateState(trayState, deviceName: "示例设备", activated: _isActivated);
	}

	protected override void OnExit(ExitEventArgs e)
	{
		_hotkeyManager?.Dispose();
		_trayManager?.Dispose();
		base.OnExit(e);
	}
}

