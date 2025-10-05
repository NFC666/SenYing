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
            VideoInfos = await LoadBaseVideoInfos(keyword, _currentPage);
            await LoadDetailAsync();
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
                    await LoadDetailAsync(moreInfos);
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
                LoadBaseVideoInfos(string keyword, int page)
        {
            return new ObservableCollection<VideoInfo>(await _m3u8Service
                        .GetSearchItemsAsync(keyword, page));
        }

        public async Task LoadDetailAsync(IEnumerable<VideoInfo>? targetInfos = null)
        {
            var infos = targetInfos ?? VideoInfos;

            var semaphore = new SemaphoreSlim(5);
            var tasks = infos.Select(async video =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var info = await _m3u8Service
                                .GetDetailSectionItemsAsyncBySelfUrlAsync(video.SelfUrl);

                    video.Status = info.Status;
                    video.Alias = info.Alias;
                    video.Area = info.Area;
                    video.Director = info.Director;
                    video.Type = info.Type;
                    video.UpdateTime = info.UpdateTime;
                    video.UploadTime = info.UploadTime;
                    video.Language = info.Language;
                    video.CoverUrl = info.CoverUrl;
                    video.Description = info.Description;
                    video.Actor = info.Actor;
                    video.Episodes = info.Episodes;
                }
                catch (Exception)
                {
                    video.Name = "获取详情失败";
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }
    }
}
