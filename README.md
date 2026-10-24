<div align="center">

<img src="src/Hyakkei.App/Assets/logo.png" width="96" alt="百景工具箱" />

# 百景工具箱 · HyakkeiTools

双击 Ctrl 唤起的 Windows 命令面板，模块化个人工具箱。

[![Release](https://img.shields.io/github/v/release/Isla-7940/HyakkeiTools)](https://github.com/Isla-7940/HyakkeiTools/releases)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-0078d4)
![.NET](https://img.shields.io/badge/.NET-10-512bd4)

</div>

---

## 特性

- **双击 Ctrl** 唤起，失焦即隐，启动静默进托盘
- **万能输入**：模块名过滤 · 算式求值 · 链接直开 · 外语即译
- **动作键 `F6`**：当前模块独享，退出即释放
- 模块化架构，管理中心一键开关
- WPF 原生，无 Electron；隐藏时常驻内存约 18MB

## 模块

| 模块 | 功能 |
|---|---|
| 连点器 | 鼠标 / 键盘 × 连点 / 长按，间隔与频率预设 |
| 翻译 | 输入即翻 · 划词（选中 + `F6`）· 截屏 OCR（`F6` 框选，译文就地显示）· 中 / 英 / 法 |
| 挂机 | 组合长按（如 `~`+`W`+左键），`F6` 一键按住 / 松开 |

<!-- 截图位：docs/assets/island-list.png / island-clicker.png -->

## 安装

[Releases](https://github.com/Isla-7940/HyakkeiTools/releases) 提供两种包：

| 包 | 体积 | 要求 |
|---|---|---|
| `win-x64.zip` | ~62MB | 无，解压即用 |
| `win-x64-lite.zip` | ~9MB | [.NET 10 桌面运行时](https://dotnet.microsoft.com/download/dotnet/10.0) |

运行 `HyakkeiTools.exe` → 双击 Ctrl。程序未签名，SmartScreen 拦截时选「仍要运行」。

**按键**：`Ctrl+数字` 直达 · `回车` 进入 · `Esc` 返回 · `Alt` 面板 ↔ 列表 · 详见[使用手册](docs/usage.md)

## 构建

```powershell
dotnet build HyakkeiTools.slnx -c Release
scripts\publish.ps1    # 单文件发布到 dist\
```

需要 .NET 10 SDK。截屏 OCR 精度依赖系统语言包（设置 → 语言 → 光学字符识别）。

## 扩展

实现 `ITool` 并注册一行即成新模块；可选 `IToolSession`（接管 F6）、`IToolQuickInput`（接入万能输入）。见[架构文档](docs/architecture.md)与[设计规范](docs/design/DESIGN.md)。

## 许可

[MIT](LICENSE)
