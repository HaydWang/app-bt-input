using System.Windows;

namespace BtInput.UI;

public partial class FirstRunWindow : Window
{
    public FirstRunWindow()
    {
        InitializeComponent();
    }

    public void SetPcDeviceName(string deviceName)
    {
        PcNameText.Text = deviceName;
    }
}
