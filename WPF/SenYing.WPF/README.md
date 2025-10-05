# SenYing.WPF

本项目是基于 WPF + MVVM 的视频聚合桌面应用，提供分类浏览、关键字搜索、视频详情与剧集切换等功能。界面采用 Material Design 风格，使用 .NET Generic Host 完成依赖注入与应用启动，消息通信基于 CommunityToolkit.Mvvm 的 IMessenger。

## 功能概览
- **首页与分类浏览**：
  - 左侧树形菜单展示视频大类与子类（电视剧、电影、动漫、综艺、短剧等）。
  - 点击任一分类，右侧内容区切换到对应列表页（`IndexUserControl`），并自动加载该分类数据。
- **关键字搜索**：
  - 顶部搜索框输入关键词并回车，自动跳转到搜索页（`SearchViewUserControl`），展示搜索结果。
- **视频详情与播放入口**：
  - 在列表/搜索结果中选择某个视频，进入视频详情页（`VideoViewUserControl`），展示封面、主演、导演、地区、类型、更新状态、简介等信息，并列出剧集列表。
  - 支持选择不同剧集并进入播放窗口（`VideoWindow`）。
- **剧集切换**：
  - 在详情页中选择剧集时，通过消息通信切换到对应的播放资源。
- **主题与样式**：
  - 使用 Material Design in XAML 主题，统一控件样式与交互视觉。

## 交互流程
- **分类切换**：
  - `MainWindow` 左侧 `TreeView` 绑定 `MenuItems`；用户选择项后，`MainWindowViewModel` 将 `CurrentPage` 切换为 `IndexUserControl`，并通过 `ChangeMenuItemMessage` 通知页面加载对应 `VideoType` 数据。
- **搜索**：
  - 用户在搜索框回车，`MainWindow` 发送 `GoToSearchMessage` 与 `SearchByKeywordMessage`，`MainWindowViewModel` 将 `CurrentPage` 切换为 `SearchViewUserControl`，页面据此执行搜索并展示结果。
- **进入详情/播放**：
  - 在列表或搜索结果中选择视频，触发 `GoToVideoMessage`，`MainWindowViewModel` 将 `CurrentPage` 切换为 `VideoViewUserControl`；选择剧集后可打开 `VideoWindow` 进行播放。

## 核心架构
- **MVVM**：
  - 视图：`Views`（`MainWindow`、`UserControls`、`VideoWindow`）。
  - 视图模型：`ViewModels`（`MainWindowViewModel`、`IndexUserControlVm`、`SearchViewUserControlVm`、`VideoWindowViewModel`）。
  - 数据模型：`SenYing.Common.Model`（例如 `VideoInfo`）。
  - 枚举与类型：`SenYing.Common.Enum`（例如 `VideoType`）。
- **依赖注入（DI）**：
  - 在 `App.xaml.cs` 中通过 `Host.CreateDefaultBuilder` 注册窗口、视图、视图模型与服务（`IM3u8Service`）。
  - `MainWindow` 与各控件/VM 通过构造注入或 `App.ServiceProvider` 获取依赖。
- **消息通信（IMessenger）**：
  - 使用 `WeakReferenceMessenger` 进行跨视图/VM 通知（如 `GoToSearchMessage`、`GoToVideoMessage`、`ChangeMenuItemMessage` 等）。
- **服务层**：
  - `SenYing.Services` 提供 `IM3u8Service` 接口与实现（`M3u8Service`），负责：
    - 关键字搜索（`GetSearchItemsAsync`）
    - 分类数据获取（`GetVideoInfoFromSectionAsync` / `GetBaseSectionItemsAsync`）
    - 视频详情与剧集解析（`GetVideoInfosAsync` / `GetDetailSectionItemsAsyncBySelfUrlAsync`）

## 目录结构（节选）
- `SenYing.WPF/`
  - `SenYing.WPF/`
    - `App.xaml` / `App.xaml.cs`：应用入口与主机构建、DI 注册、主题资源。
    - `Views/`：`MainWindow`、`UserControls`（`IndexUserControl`、`SearchViewUserControl`、`VideoViewUserControl`）、`VideoWindow`。
    - `ViewModels/`：`MainWindowViewModel`、`UserControls` 对应的 VM、`VideoWindowViewModel`。
    - `Behaviors/`：界面行为（如 `TreeViewSelectedItemBehavior`）。
    - `Converter/`：常用值转换器。
    - `Resources/`：图标与资源文件。
    - `Properties/PublishProfiles/`：ClickOnce 发布配置。

## 运行与调试
- 在解决方案根目录还原与构建：
```bash
dotnet restore
dotnet build -c Debug
```
- 运行 WPF 主程序：
```bash
dotnet run --project .\WPF\SenYing.WPF\SenYing.WPF
```
- 也可在 IDE（Visual Studio / Rider）中将 `SenYing.WPF` 设为启动项目后运行。

## 发布
- 命令行发布（框架依赖）：
```bash
dotnet publish .\WPF\SenYing.WPF\SenYing.WPF -c Release -r win-x64 --self-contained false
```
- 自包含发布（免安装运行时）：
```bash
dotnet publish .\WPF\SenYing.WPF\SenYing.WPF -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```
- ClickOnce：使用 `Properties/PublishProfiles/ClickOnceProfile.pubxml`，在 IDE 中“发布”，或参考配置执行命令行发布。

## 使用说明与提示
- 首次启动会显示免责声明，请遵循提示，不得用于任何商业用途。
- 左侧菜单可展开子类（如电视剧下的日剧、韩剧、欧美剧等）。
- 搜索时回车触发搜索并跳转到搜索页。
- 视频详情页可查看简介与剧集列表，选择剧集即可进入播放窗口。

## 免责声明
本软件所有内容均收集于互联网视频站点，本软件不会保存、复制、传播任何资源。作者不承担由使用不当引发的任何法律责任。仅供学习交流使用，禁止一切商业用途。若侵犯您的权益，请联系作者以便及时处理。

## 许可
- 本仓库根目录 `README.md` 中已包含 **MIT** 许可全文。本子项目遵循相同许可协议。