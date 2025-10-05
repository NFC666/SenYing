using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SenYing.Common.Model
{
    public partial class VideoInfo : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _status;

        [ObservableProperty]
        private string? _coverUrl;

        [ObservableProperty]
        private string? _selfUrl;

        [ObservableProperty]
        private string? _alias;

        [ObservableProperty]
        private string? _director;

        [ObservableProperty]
        private string? _actor;

        [ObservableProperty]
        private string? _type;

        [ObservableProperty]
        private string? _area;

        [ObservableProperty]
        private string? _language;

        [ObservableProperty]
        private string? _uploadTime;

        [ObservableProperty]
        private string? _updateTime;

        [ObservableProperty]
        private string? _description;

        // List 本身不会触发属性变更通知，但你一般是整个赋值，
        // 如果需要动态增删集，推荐用 ObservableCollection<string?>
        [ObservableProperty]
        private List<string?> _episodes = new List<string?>();
    }
}
