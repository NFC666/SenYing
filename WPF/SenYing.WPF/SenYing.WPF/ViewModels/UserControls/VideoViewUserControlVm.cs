using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using SenYing.Common.Message;
using SenYing.Common.Model;
using SenYing.WPF.Views;

namespace SenYing.WPF.ViewModels.UserControls
{
    public partial class VideoViewUserControlVm : ObservableObject
    {
        [ObservableProperty]
        private VideoInfo _Info;
        [ObservableProperty]
        private List<EpisodeInfo> _episodes = new();

        public VideoViewUserControlVm(VideoInfo videoInfo)
        {
            if (videoInfo == null)
            {
                return;
            }
            Info = videoInfo;
            for (int i = 1; i <= Info.Episodes.Count; i++)
            {
                var Title = $"第{i}集";
                _episodes.Add(new EpisodeInfo(Title, Info.Episodes[i - 1]));
            }
        }

        [RelayCommand]
        private void ChangeEpisode(string url)
        {
            try
            {
                WeakReferenceMessenger.Default.Send(new ChangeEpisodeMessage(url));

                var videoWinow = new VideoWindow(url);
                videoWinow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public record EpisodeInfo(string Title, string Url);
    }
}
