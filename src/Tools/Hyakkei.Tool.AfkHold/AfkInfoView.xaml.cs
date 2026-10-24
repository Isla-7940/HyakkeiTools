using System.Windows.Controls;
using Hyakkei.Core;

namespace Hyakkei.Tool.AfkHold;

public partial class AfkInfoView : UserControl
{
    public AfkInfoView()
    {
        InitializeComponent();
        HotkeyRun.Text = $" {ToolContext.Config.Current.ModuleHotkey} ";
    }
}
