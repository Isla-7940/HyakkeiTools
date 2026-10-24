using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Hyakkei.Core;

namespace Hyakkei.Tool.AfkHold;

public partial class AfkIslandView : UserControl
{
    private const int MaxKeys = 4;

    private readonly AfkHoldTool _tool;
    private bool _loading = true;
    private bool _capturing;
    private Window? _capWindow;
    private Button? _plusChip;

    public AfkIslandView(AfkHoldTool tool)
    {
        _tool = tool;
        InitializeComponent();

        LeftBtn.IsChecked = _tool.Settings.Mouse == "Left";
        RightBtn.IsChecked = _tool.Settings.Mouse == "Right";
        NoneBtn.IsChecked = _tool.Settings.Mouse == "None";
        HotkeyHint.Text = ToolContext.Config.Current.ModuleHotkey;
        RenderKeys();

        _tool.HoldingChanged += holding => Dispatcher.Invoke(() => UpdateHolding(holding));
        IsVisibleChanged += (_, _) => { if (!IsVisible) EndCapture(); };
        _loading = false;
    }

    private void UpdateHolding(bool holding)
    {
        StartBtn.Content = holding ? "停止" : "开始";
        StatusDot.Fill = holding
            ? (Brush)FindResource("Island.Accent")
            : (Brush)FindResource("Island.TextTertiary");
    }

    private void OnStartClick(object sender, RoutedEventArgs e) => _tool.Toggle();

    private void OnMouseChanged(object sender, RoutedEventArgs e)
    {
        if (_loading) return;
        _tool.Settings.Mouse = LeftBtn.IsChecked == true ? "Left"
            : RightBtn.IsChecked == true ? "Right" : "None";
        _tool.SaveSettings();
    }

    // ---- 键位芯片 ----

    private void RenderKeys()
    {
        KeyRow.Children.Clear();
        foreach (var key in _tool.Settings.Keys)
        {
            var chip = new Button { Content = key.Name, Margin = new Thickness(0, 0, 6, 0), ToolTip = "点击移除" };
            chip.SetResourceReference(StyleProperty, "Island.Chip");
            var captured = key;
            chip.Click += (_, _) =>
            {
                if (_capturing) return;
                _tool.Settings.Keys.Remove(captured);
                _tool.SaveSettings();
                RenderKeys();
            };
            KeyRow.Children.Add(chip);
        }

        if (_tool.Settings.Keys.Count < MaxKeys)
        {
            _plusChip = new Button { Content = "+", ToolTip = "添加按键" };
            _plusChip.SetResourceReference(StyleProperty, "Island.Chip");
            _plusChip.Click += (_, _) => StartCapture();
            KeyRow.Children.Add(_plusChip);
        }
    }

    private void StartCapture()
    {
        if (_capturing || _plusChip is null) return;
        _capturing = true;
        _plusChip.Content = "按键…";
        _capWindow = Window.GetWindow(this);
        _capWindow?.AddHandler(PreviewKeyDownEvent, (KeyEventHandler)OnCaptureKey, handledEventsToo: true);
    }

    private void OnCaptureKey(object sender, KeyEventArgs e)
    {
        if (!_capturing) return;
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key is not Key.Escape)
        {
            var vk = KeyInterop.VirtualKeyFromKey(key);
            if (_tool.Settings.Keys.All(k => k.Vk != vk))
            {
                _tool.Settings.Keys.Add(new AfkKey { Vk = vk, Name = PrettyKey(key) });
                _tool.SaveSettings();
            }
            e.Handled = true;
        }
        EndCapture();
    }

    private void EndCapture()
    {
        if (!_capturing) return;
        _capturing = false;
        _capWindow?.RemoveHandler(PreviewKeyDownEvent, (KeyEventHandler)OnCaptureKey);
        _capWindow = null;
        RenderKeys();
    }

    private static string PrettyKey(Key key) => key switch
    {
        >= Key.D0 and <= Key.D9 => ((char)('0' + (key - Key.D0))).ToString(),
        Key.Oem3 => "~",
        Key.OemMinus => "-",
        Key.OemPlus => "+",
        Key.Space => "Space",
        _ => key.ToString(),
    };
}
