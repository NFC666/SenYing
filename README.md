# SenYing

这是一款学习性质软件, 主要目的是用于对WPF的网络抓取, 视频显示等功能的练手. 本软件的核心功能是获取https://jszy333.com/ 爬取的数据并展示,还有对m3u8资源的实时浏览.  

本软件所有内容均收集于互联网各种视频网站，本软件不会保存,复制,传播任何资源。本软件不负任何法律责任。
此外, 本软件只供学习目的, 禁止一切商用, 因使用者滥用导致的问题, 本软件作者概不负责. 
如果侵犯了您的权益，请通知我，我会第一时间及时删除侵权内容，请在下载测试后24小时内删除,谢谢合作！
请支持正版！

## 项目概览
- **主应用（WPF）**: 位于 `WPF/SenYing.WPF/SenYing.WPF`，采用 MVVM 模式与依赖注入，集成 Material Design 风格与常用开发工具链。
- **公共与服务**: `SenYing.Common`、`SenYing.Services` 提供模型、消息与服务能力。
- **单元测试**: `WPF/SenYing.WPF.Tests` 为 WPF 的单元测试项目。
- **根级配置**: `.gitignore` 已忽略 `.vs`、`.rider`、`bin`、`obj` 等常见产物与 IDE 文件。

## 技术栈与特性
- **MVVM**: 基于 CommunityToolkit MVVM（简化属性变更通知、命令绑定、依赖注入配合）。
- **依赖注入**: 使用 .NET Generic Host 构建与注入应用服务。
- **UI**: Material Design in XAML（现代化控件与主题）。
- **集中式包管理**: 通过 `Directory.Packages.props` 管理 NuGet 依赖版本。
- **构建自定义**: `Directory.Build.props`、`Directory.Build.targets` 用于统一配置。
- **代码风格**: `Settings.XamlStyler` 用于 XAML 自动格式化。
- **测试**: 内置单元测试项目，支持 `dotnet test` 与覆盖率生成（可按需配置）。

## 目录结构（节选）
- `WPF/`
  - `SenYing.WPF/`
    - `SenYing.WPF/`
      - `App.xaml`、`App.xaml.cs`（应用入口与启动）
      - `Views/`（页面与控件：`MainWindow.xaml`、`UserControls` 等）
      - `ViewModels/`（视图模型：`MainWindowViewModel.cs` 等）
      - `Behaviors/`、`Converter/`、`Resources/`（行为、转换器、资源）
      - `Properties/PublishProfiles/`（ClickOnce 发布配置）
      - `SenYing.WPF.csproj`（项目文件）
    - `README.md`（WPF 模板说明）
  - `SenYing.WPF.Tests/`（WPF 测试项目）
- 其他：`SenYing`（MAUI 示例）、`SenYing.Common`、`SenYing.Services`、`SenYing.UnitTest`（通用测试）

## 环境要求
- Windows 10/11（开发与运行）
- .NET SDK 9（或与项目 `TargetFramework` 对应的版本）
- IDE：Visual Studio 2022 / Rider / VS Code（任选其一）

## 快速开始
1. 还原依赖
```bash
dotnet restore
```
2. 编译（建议在 Release/Debug 任一配置下皆可）
```bash
dotnet build -c Debug
# 或
# dotnet build -c Release
```
3. 运行 WPF 主应用
```bash
dotnet run --project .\WPF\SenYing.WPF\SenYing.WPF
```

## 发布
- 方式一：命令行发布（框架依赖）
```bash
dotnet publish .\WPF\SenYing.WPF\SenYing.WPF -c Release -r win-x64 --self-contained false
```
- 方式二：自包含发布（无需安装 .NET 运行时）
```bash
dotnet publish .\WPF\SenYing.WPF\SenYing.WPF -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```
- 方式三：ClickOnce（推荐用于快速分发）
  - 使用 `Properties/PublishProfiles/ClickOnceProfile.pubxml`。
  - 可在 IDE 中右键项目选择“发布”，或命令行执行与配置文件匹配的发布命令。
  - 发布产物可在 `bin/Release/netX.Y-windows/publish` 或 `bin/publish` 相关目录下找到（仓库中亦能看到 `publish/` 示例产物结构）。

## 测试
- 运行 WPF 测试项目：
```bash
dotnet test .\WPF\SenYing.WPF.Tests
```
- 如需覆盖率与报告，可引入 Coverlet 与 ReportGenerator（WPF 模板 README 中有相关参考）。

## 开发说明
- **MVVM 约定**：
  - 视图（`Views/`）只处理展示与交互；
  - 视图模型（`ViewModels/`）处理状态与命令；
  - 服务（在 `SenYing.Services` 等）封装业务逻辑与外部依赖；
  - 模型与消息（在 `SenYing.Common`）封装数据与跨模块通信。
- **依赖注入**：在应用启动（`App.xaml.cs`）或组合根中注册服务与视图模型，确保构造函数注入即可使用。
- **样式与主题**：通过 Material Design in XAML 资源字典与 `Resources/` 统一管理主题与控件样式。

## 常见问题（FAQ）
- 运行时报缺少运行时：请确认目标机器已安装对应的 .NET Desktop Runtime；或使用自包含发布。
- XAML 样式不生效：检查 `App.xaml` 的资源字典引用顺序与键名，确保 Material 资源已加载。
- 生成产物过大：尝试 `PublishTrimmed`、`PublishSingleFile` 并评估对反射的影响。
