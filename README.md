<div align="center">

<img src="src/Hyakkei.App/Assets/logo.png" width="96" alt="百景工具箱" />

# 百景工具箱 · HyakkeiTools

**双击 Ctrl，唤起你的个人功能中心。**

一个极简的 Windows 命令面板：连点、翻译、截屏识别、挂机……零散小工具不再各装一个软件。

[![Release](https://img.shields.io/github/v/release/Isla-7940/HyakkeiTools)](https://github.com/Isla-7940/HyakkeiTools/releases)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-0078d4)
![.NET](https://img.shields.io/badge/.NET-10-512bd4)

</div>

---

## 它是什么

日常高频的小功能，往往每个都要单独找一个软件装。百景工具箱把它们收进一个入口：**双击 Ctrl** 弹出一块轻巧的面板，打字即搜，用完即走——

- 输入模块名 → 过滤直达；输入**算式** → 立即出结果；粘贴**链接** → 回车打开；输入**外语** → 回车出译文
- 每个功能是独立模块，管理中心一键开关；关掉的绝不打扰你
- **一个动作键 `F6`**：进了哪个模块就归谁——连点启停、划词翻译、组合挂机，退出即释放，平时不占你的键位
- 启动静默进托盘；面板隐藏时常驻内存约 **18MB**，无 Electron

## 模块一览

| 模块 | 能做什么 |
|---|---|
| **连点器** | 鼠标左/右键或键盘任意键，连点（间隔可调、频率预设）或长按；`F6` 启停 |
| **翻译** | 输入即翻；任意应用选中文字按 `F6` 划词翻译；不选中按 `F6` 框选屏幕 → OCR → 译文就地浮现（识别原文可直接复制，兼作文字提取）；中 / 英 / 法，谷歌 → 腾讯自动回退 |
| **挂机** | 组合长按：一键同时按住若干键 + 鼠标键（如 `~`+`W`+左键），再按全部松开，适合游戏重复动作 |

<!-- 截图位：docs/assets/island-list.png / island-clicker.png -->

## 快速上手

1. 从 [Releases](https://github.com/Isla-7940/HyakkeiTools/releases) 下载：
   - `win-x64.zip`（约 62MB）——自带运行时，**解压即用**
   - `win-x64-lite.zip`（约 9MB）——需已安装 [.NET 10 桌面运行时](https://dotnet.microsoft.com/download/dotnet/10.0)
2. 运行 `HyakkeiTools.exe`，程序静默进入托盘
3. **双击 Ctrl** 唤起面板，开始使用

> 程序未签名，首次运行若被 SmartScreen 拦下，点「更多信息 → 仍要运行」。

常用按键：`Ctrl+数字` 直达模块 · `回车` 进入 · `Esc` 逐级返回 · 轻点 `Alt` 在面板与列表间往返 · 失焦自动隐藏。全部操作见 **[使用手册](docs/usage.md)**。

## 构建

```powershell
dotnet build HyakkeiTools.slnx -c Release   # 构建
scripts\publish.ps1                          # 单文件发布到 dist\
```

环境：Windows 10 19041+ / Windows 11，.NET 10 SDK。截屏翻译的识别精度取决于系统 OCR 语言包（设置 → 语言 → 勾选「光学字符识别」）。

## 写一个自己的模块

实现 `ITool`（岛内极简视图 + 管理中心页），在 `App.xaml.cs` 注册一行即可；可选实现 `IToolSession`（接管 `F6`）与 `IToolQuickInput`（接入万能输入）。细节见 [架构文档](docs/architecture.md)，界面请遵循 [设计规范](docs/design/DESIGN.md)。

## 文档

- [使用手册](docs/usage.md) — 所有操作方式
- [架构与实施记录](docs/architecture.md)
- [界面设计规范](docs/design/DESIGN.md) — 天青配色 · 字号 · 间距 · 控件

## 许可

[MIT](LICENSE)
