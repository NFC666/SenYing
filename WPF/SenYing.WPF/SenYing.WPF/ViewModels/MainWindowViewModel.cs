using System.ComponentModel;
using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using MaterialDesignThemes.Wpf;

using Microsoft.Extensions.DependencyInjection;

using SenYing.Common.Enum;
using SenYing.Common.Message;
using SenYing.Common.Model;
using SenYing.WPF.Views.UserControls;

namespace SenYing.WPF
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserControl _currentPage;
        [ObservableProperty]
        private MenuItemModel? _selectedMenuItem;
        [ObservableProperty]
        private List<MenuItemModel> _menuItems;

        partial void OnSelectedMenuItemChanged(MenuItemModel? value)
        {
            if (value != null)
            {
                CurrentPage = App.ServiceProvider.GetRequiredService<IndexUserControl>();
                WeakReferenceMessenger.Default.Send(new ChangeMenuItemMessage(value.Type));
                SelectedMenuItem = null;
            }
        }


        public MainWindowViewModel()
        {
            ConfigMenuItems();
            WeakReferenceMessenger.Default.Register<GoToVideoMessage>(this, (w, m) =>
            {
                CurrentPage = new VideoViewUserControl(m.VideoInfo);
            });
            WeakReferenceMessenger.Default.Register<GoToSearchMessage>(this, (w, m) =>
            {
                CurrentPage = App.ServiceProvider.GetRequiredService<SearchViewUserControl>();
            });
        }

        private void ConfigMenuItems()
        {
            MenuItems = new List<MenuItemModel>
    {
        new MenuItemModel()
        {
            Title = "首页",
            Type = VideoType.默认
        },
        new MenuItemModel()
        {
            Title = "电视剧",
            Type = VideoType.电视剧,
            Items = new List<MenuItemModel>
            {
                new () { Title = "日剧", Type = VideoType.日剧 },
                new () { Title = "马泰剧", Type = VideoType.马泰剧 },
                new () { Title = "内地剧", Type = VideoType.内地剧 },
                new () { Title = "欧美剧", Type = VideoType.欧美剧 },
                new () { Title = "香港剧", Type = VideoType.香港剧 },
                new () { Title = "韩剧", Type = VideoType.韩剧 },
                new () { Title = "台湾剧", Type = VideoType.台湾剧 }
            }
        },
        new MenuItemModel()
        {
            Title = "电影",
            Type = VideoType.电影,
            Items = new List<MenuItemModel>
            {
                new () { Title = "恐怖片", Type = VideoType.恐怖片 },
                new () { Title = "动画片", Type = VideoType.动画片 },
                new () { Title = "剧情片", Type = VideoType.剧情片 },
                new () { Title = "战争片", Type = VideoType.战争片 },
                new () { Title = "动作片", Type = VideoType.动作片 },
                new () { Title = "记录片", Type = VideoType.记录片 },
                new () { Title = "爱情片", Type = VideoType.爱情片 },
                new () { Title = "喜剧片", Type = VideoType.喜剧片 },
                new () { Title = "科幻片", Type = VideoType.科幻片 },
                new () { Title = "灾难片", Type = VideoType.灾难片 },
                new () { Title = "悬疑片", Type = VideoType.悬疑片 },
                new () { Title = "犯罪片", Type = VideoType.犯罪片 }
            }
            },
            new MenuItemModel()
            {
                Title = "动漫",
                Type = VideoType.动漫,
                Items = new List<MenuItemModel>
                {
                    new () { Title = "中国动漫", Type = VideoType.中国动漫 },
                    new () { Title = "日本动漫", Type = VideoType.日本动漫 },
                    new () { Title = "欧美动漫", Type = VideoType.欧美动漫 }
                }
            },
            new MenuItemModel()
            {
                Title = "综艺",
                Type = VideoType.综艺,
                Items = new List<MenuItemModel>
                {
                    new () { Title = "大陆综艺", Type = VideoType.大陆综艺 },
                    new () { Title = "日韩综艺", Type = VideoType.日韩综艺 },
                    new () { Title = "港台综艺", Type = VideoType.港台综艺 },
                    new () { Title = "欧美综艺", Type = VideoType.欧美综艺 }
                }
            },
            new MenuItemModel()
            {
                Title = "短剧",
                Type = VideoType.短剧
            }
        };
        }

    }
}
