using SenYing.Common.Enum;
using SenYing.Common.Model;

namespace SenYing.Services.IServices
{
    public interface IM3u8Service
    {
        Task<List<VideoInfo>> GetSearchItemsAsync(string keyword,int page);

        Task<VideoInfo> GetVideoInfosAsync(string url);

        Task<List<VideoInfo>> GetVideoInfoFromSectionAsync(VideoType type,int page);

        Task<List<VideoInfo>>
            GetBaseSectionItemsAsync(VideoType type, int page);

        Task<VideoInfo>
            GetDetailSectionItemsAsyncBySelfUrlAsync(string selfUrl);
    }
}
