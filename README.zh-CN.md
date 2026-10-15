# 📸 CapturaLibre DNET 2.6.1 — C# / .NET 8

[Español](README.md) | [简体中文](README.zh-CN.md)

CapturaLibre 是一款面向 Windows 10 和 Windows 11 的**免费、多语言、开源**截图与图片编辑工具。程序完全在本地运行，不含广告、遥测，也不会自动上传截图。项目采用 [MIT 许可证](LICENSE)。

稳定版 2.6.1 新增“不切换焦点”截图：可使用 5 秒准备按钮，或在 CapturaLibre 运行时按全局 **Print Screen** 键，保留已展开的菜单并在截图后打开编辑器。同一个可执行文件内置 **62 种语言**。

## 主要功能

- 截取选定区域、主屏幕、全部屏幕或指定窗口。
- 使用放大镜、坐标和准星精确选择区域。
- 提供 0、3、5 和 10 秒延时。
- 使用箭头、矩形、椭圆、画笔、编号步骤和文字进行标注。
- 对敏感内容进行像素化、模糊或黑色遮挡。
- 裁剪、旋转 90°、翻转图片，并支持最多六步撤销。
- 打开本地 PNG/JPG 图片继续编辑。
- 保存为 PNG/JPG、快速保存、复制到剪贴板或打开保存文件夹。
- 使用快捷键：`F6` 区域截图、`F7` 全屏幕截图、`F8` 主屏幕截图。
- 通过 DNET 模式直接按顺序保存 PNG，适合制作教程和文档。

## 下载 Windows 版本

请前往 [GitHub Releases 最新版本](https://github.com/arturo1961/Capturador-Pantalla-Windows/releases/latest)。

- `Instalar_CapturaLibre_DNET_2.6.1.exe`：推荐的安装版，包含安装向导和卸载程序。
- `CapturaLibre_DNET.exe`：免安装便携版，可直接运行。

两个版本均为 Windows x64 自包含程序，无需预先安装 .NET 8。

> Windows SmartScreen 可能会显示警告，因为可执行文件尚未进行商业数字签名。请只从本项目的官方 GitHub 页面下载，不要为了运行程序而关闭 Windows 的整体安全保护。

## 基本使用方法

1. 启动程序，并在右上角的语言选择器中选择简体中文。
2. 选择区域、屏幕或窗口截图。
3. 在编辑器中添加文字、箭头、图形，或隐藏敏感信息。
4. 根据需要裁剪或旋转图片。
5. 保存图片、复制到剪贴板，或打开保存文件夹。

语言选择会保存在本机，下次启动时继续使用。完整语言列表及扩展方法请参阅 [IDIOMAS.md](IDIOMAS.md)。

## 隐私说明

截图、设置和历史记录都保存在用户自己的电脑上。程序不会自动上传文件，也不包含账户系统或遥测功能。使用半透明标注无法可靠隐藏敏感信息；需要保护隐私时，请使用黑色遮挡、像素化或模糊工具，并在分享前再次检查图片。

## 从源代码编译

需要 Windows 10/11、Visual Studio 2022 17.8 或更高版本，以及 .NET 8 SDK。也可以在项目根目录运行：

```powershell
dotnet publish CapturaLibre/CapturaLibre.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=none -p:DebugSymbols=false -o publicar
```

## 免费软件与自愿捐赠

**CapturaLibre 现在免费，将来也会保持免费。是否捐赠完全由每位用户自行决定。** ❤️

自愿支持项目：

- [PayPal](https://paypal.me/acarballoflandes)
- [Ko-fi](https://ko-fi.com/arturocarballo)
- Bizum：`+34 662 024 169`
- [GitHub Sponsors](https://github.com/sponsors/arturo1961)（个人赞助页面可用后）

捐赠不会解锁功能或带来特殊权限，也不会改变 MIT 许可证。你也可以通过报告问题、提出建议、改进翻译或分享项目来提供帮助。

## 许可证

本项目采用 [MIT 许可证](LICENSE)。允许在保留许可证和版权声明的前提下使用、研究、修改和分发源代码。
