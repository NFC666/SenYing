using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using SenYing.Common.Enum;
using SenYing.Common.Message;
using SenYing.Common.Model;
using SenYing.Services;
using SenYing.Services.IServices;

namespace SenYing.WPF.ViewModels.UserControls
{
    public partial class SearchViewUserControlVm : ObservableObject
    {
        private readonly IM3u8Service _m3u8Service = App.ServiceProvider.GetRequiredService<IM3u8Service>();
        [ObservableProperty]
        private ObservableCollection<VideoInfo> _videoInfos
            = new ObservableCollection<VideoInfo>();
        [ObservableProperty]
        private VideoInfo _selectedVideoInfo;
        partial void OnSelectedVideoInfoChanged(VideoInfo? oldValue, VideoInfo newValue)
        {
            if (SelectedVideoInfo == null)
            {
                return;
            }
            WeakReferenceMessenger.Default.Send(new GoToVideoMessage(SelectedVideoInfo));
        }

        [ObservableProperty]
        private bool _isBusy;

        private string _currentKeyword = "";
        private int _currentPage = 1;

        public SearchViewUserControlVm()
        {

            WeakReferenceMessenger.Default.Register<SearchByKeywordMessage>(this, async (w, m) =>
            {
                _currentKeyword = m.Keyword;
                await LoadDataAsync(m.Keyword);

            });
        }

        public async Task LoadDataAsync(string keyword)
        { 
            IsBusy = true;
            VideoInfos = await LoadInfos(keyword, _currentPage);
            IsBusy = false;
        }

        [RelayCommand]
        private async Task LoadMore()
        {
            if (IsBusy) return; // 防止重复点击触发

            IsBusy = true;
            try
            {
                _currentPage++;

                // 获取更多基础视频信息
                var moreInfos = await _m3u8Service.GetSearchItemsAsync(_currentKeyword, _currentPage);

                if (moreInfos != null && moreInfos.Count > 0)
                {
                    foreach (var info in moreInfos)
                    {
                        VideoInfos.Add(info);
                    }
                }
                else
                {
                    _currentPage--;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<ObservableCollection<VideoInfo>>
                LoadInfos(string keyword, int page)
        {
            return new ObservableCollection<VideoInfo>(await _m3u8Service
                        .GetSearchItemsAsync(keyword, page));
        }
    }
}
