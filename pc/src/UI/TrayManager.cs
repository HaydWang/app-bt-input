using System.Drawing;
using System.Windows.Forms;

namespace BtInput.UI;

public enum TrayState
{
    Disconnected,
    Connecting,
    Connected,
    Active
}

public sealed class TrayManager : IDisposable
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ToolStripMenuItem _statusItem;
    private readonly ToolStripMenuItem _toggleItem;

    public event EventHandler? ToggleRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? AboutRequested;
    public event EventHandler? ExitRequested;

    public TrayManager()
    {
        _statusItem = new ToolStripMenuItem("未连接") { Enabled = false };
        _toggleItem = new ToolStripMenuItem("启用 BT Input");
        _toggleItem.Click += (_, _) => ToggleRequested?.Invoke(this, EventArgs.Empty);

        var settingsItem = new ToolStripMenuItem("快捷键设置...");
        settingsItem.Click += (_, _) => SettingsRequested?.Invoke(this, EventArgs.Empty);

        var aboutItem = new ToolStripMenuItem("关于");
        aboutItem.Click += (_, _) => AboutRequested?.Invoke(this, EventArgs.Empty);

        var exitItem = new ToolStripMenuItem("退出");
        exitItem.Click += (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty);

        var menu = new ContextMenuStrip();
        menu.Items.Add(_statusItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(_toggleItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(settingsItem);
        menu.Items.Add(aboutItem);
        menu.Items.Add(exitItem);

        _notifyIcon = new NotifyIcon
        {
            Visible = true,
            Icon = SystemIcons.Application,
            Text = "BT Input - 未连接",
            ContextMenuStrip = menu
        };

        _notifyIcon.DoubleClick += (_, _) => SettingsRequested?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateState(TrayState state, string? deviceName = null, bool activated = false)
    {
        var name = string.IsNullOrWhiteSpace(deviceName) ? "未连接" : deviceName;
        _statusItem.Text = state switch
        {
            TrayState.Disconnected => "未连接",
            TrayState.Connecting => "连接中...",
            TrayState.Connected => $"已连接: {name}",
            TrayState.Active => $"输入中: {name}",
            _ => "未连接"
        };

        _toggleItem.Checked = activated;
        _notifyIcon.Text = $"BT Input - {_statusItem.Text}";

        _notifyIcon.Icon = state switch
        {
            TrayState.Disconnected => SystemIcons.Application,
            TrayState.Connecting => SystemIcons.Warning,
            TrayState.Connected => SystemIcons.Information,
            TrayState.Active => SystemIcons.Shield,
            _ => SystemIcons.Application
        };
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}
