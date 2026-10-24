using System.Windows;
using Hyakkei.Core;

namespace Hyakkei.Tool.AfkHold;

public sealed class AfkKey
{
    public int Vk { get; set; }
    public string Name { get; set; } = "";
}

/// <summary>挂机设置（持久化到配置 Tools.afk-hold 节）。默认组合：~ + W + 鼠标左键。</summary>
public sealed class AfkSettings
{
    public List<AfkKey> Keys { get; set; } =
    [
        new() { Vk = 0xC0, Name = "~" },  // VK_OEM_3
        new() { Vk = 0x57, Name = "W" },
    ];

    /// <summary>Left | Right | None</summary>
    public string Mouse { get; set; } = "Left";
}

/// <summary>
/// 挂机：组合长按——热键按一次同时按住所有配置的键（键盘若干 + 鼠标），再按一次全部松开。
/// 走扫描码注入，读键盘状态的游戏能正确识别"按住"。Esc/Alt 退出面板、切模块、退出程序都会强制松开。
/// </summary>
public sealed class AfkHoldTool : ITool, IToolSession
{
    public const string ToolId = "afk-hold";

    private int _hotkeyId = -1;
    private AfkKey[] _heldKeys = [];   // 开始时的快照，保证松开的和按下的一致
    private string _heldMouse = "None";

    public AfkHoldTool()
    {
        Settings = ToolContext.Config.GetToolConfig<AfkSettings>(ToolId);
    }

    public AfkSettings Settings { get; }

    public bool IsHolding { get; private set; }

    /// <summary>按住状态变化（热键线程=UI 线程，可直接更新界面）。</summary>
    public event Action<bool>? HoldingChanged;

    public string Id => ToolId;
    public string Name => "挂机";
    public string Description => "组合长按 · 一键切换";
    public string IconGlyph => "\uE7FC"; // 手柄

    public FrameworkElement CreateView() => new AfkInfoView();
    public FrameworkElement CreateIslandView() => new AfkIslandView(this);

    public void Toggle()
    {
        if (IsHolding) StopHold();
        else StartHold();
    }

    private void StartHold()
    {
        if (IsHolding) return;
        _heldKeys = [.. Settings.Keys];
        _heldMouse = Settings.Mouse;

        foreach (var k in _heldKeys)
            InputSimulator.KeyDown((ushort)k.Vk);
        if (_heldMouse == "Left") InputSimulator.MouseDown(SimMouseButton.Left);
        else if (_heldMouse == "Right") InputSimulator.MouseDown(SimMouseButton.Right);

        IsHolding = true;
        HoldingChanged?.Invoke(true);
        Log.Info($"挂机开始：{string.Join("+", _heldKeys.Select(k => k.Name))}+{_heldMouse}");
    }

    public void StopHold()
    {
        if (!IsHolding) return;
        if (_heldMouse == "Left") InputSimulator.MouseUp(SimMouseButton.Left);
        else if (_heldMouse == "Right") InputSimulator.MouseUp(SimMouseButton.Right);
        for (var i = _heldKeys.Length - 1; i >= 0; i--)
            InputSimulator.KeyUp((ushort)_heldKeys[i].Vk);

        IsHolding = false;
        HoldingChanged?.Invoke(false);
        Log.Info("挂机停止");
    }

    public void SaveSettings() => ToolContext.Config.SetToolConfig(ToolId, Settings);

    public void OnSessionActivated()
    {
        var gesture = ToolContext.Config.Current.ModuleHotkey;
        _hotkeyId = ToolContext.ModuleHotkeys.Register(gesture, Toggle);
        Log.Info(_hotkeyId >= 0 ? $"挂机热键已挂载：{gesture}" : $"挂机热键挂载失败：{gesture}");
    }

    public void OnSessionDeactivated()
    {
        if (_hotkeyId >= 0)
        {
            ToolContext.ModuleHotkeys.Unregister(_hotkeyId);
            _hotkeyId = -1;
        }
        StopHold(); // 强制松开，防卡键
        Log.Info("挂机热键已解除");
    }
}
