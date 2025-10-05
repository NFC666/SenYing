using CommunityToolkit.Mvvm.ComponentModel;
using SenYing.Common.Modesl;
using SenYing.Services;
using SenYing.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenYing.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private List<IndexItemModel> _indexItems = new();

        private readonly IM3u8Service _m3u8Service 
            = App.ServiceProvider.GetRequiredService<IM3u8Service>();

        public MainPageViewModel()
        {
            _ = GetIndexItemsAsync();
        }

        public async Task GetIndexItemsAsync()
        {
            var items = await _m3u8Service.GetIndexItemsAsync();
            IndexItems = items;
        }

    }
}
