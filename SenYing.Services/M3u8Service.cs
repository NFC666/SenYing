using HtmlAgilityPack;
using SenYing.Services.IServices;
using SenYing.Common.Enum;
using SenYing.Common.Model;

namespace SenYing.Services
{
    public class M3u8Service : IM3u8Service
    {
        private string[] BaseUrls =
            ["www.jisuzy.com", "www.jisuzy.tv", "www.jisuziyuan.com", "www.jszy666.com", "www.jszy333.com"];
        private string BaseUrl = "https://jszy333.com/";
        private HttpClient _httpClient;

        public M3u8Service()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _ = TestUrlsAsync();
        }

        private async Task TestUrlsAsync()
        { 
            foreach (var item in BaseUrls)
            {
                var test = await GetBaseSectionItemsAsync(0, 1);
                if (test.Count > 0)
                {
                    BaseUrl = item;
                    break;
                }
            }
            throw new Exception("没有可用的源, 软件已失效或没有网络链接.");
        }


        public async Task<List<VideoInfo>>
            GetSearchItemsAsync(string keyword, int page)
        {
            //var url = $"{BaseUrl}index.php/vod/search.html?wd={keyword}";
            var url = $"index.php/vod/search/page/{page}/wd/{keyword}.html?ac=detail";

            var html = await _httpClient.GetStringAsync(url);

            var list = await GetInfosFromDocAsync(html);
            
            return list;
        }

        public async Task<VideoInfo> GetVideoInfosAsync(string url)
        {
            var html = await _httpClient.GetStringAsync(url);
            var list = GetInfoFromItem(html);
            return list;
        }

        public async Task<List<VideoInfo>>
            GetVideoInfoFromSectionAsync(VideoType type, int page)
        {
            var res = new List<VideoInfo>();
            string url = type == 0 ? "/" : $"/index.php/vod/type/id/{(int)type}/page/{page}.html?ac=detail";
            var html = await _httpClient.GetStringAsync(url);
            var list = await GetInfosFromDocAsync(html);
            return list;
        }
        public async Task<List<VideoInfo>>
            GetBaseSectionItemsAsync(VideoType type, int page)
        {
            var res = new List<VideoInfo>();
            string url = $"/index.php/vod/type/id/{(int)type}/page/{page}.html?ac=detail";
            if (type == 0)
            {
                url = $"/index.php/index/index/page/{page}.html?ac=detail";
            }
            var html = await _httpClient.GetStringAsync(url);
            var list = GetItemsFromDoc(html);
            foreach (var item in list)
            {
                if(item.Type == "伦理片")
                {
                    continue;
                }
                var info = new VideoInfo()
                {
                    Name = item.Title.Replace(" ",""),
                    SelfUrl = item.Url,
                    Type = item.Type,
                    UpdateTime = item.Time
                };
                res.Add(info);
            }
            return res;
        }

        public async Task<VideoInfo>
            GetDetailSectionItemsAsyncBySelfUrlAsync(string selfUrl)
        {
            var html = await _httpClient.GetStringAsync(selfUrl);
            return GetInfoFromItem(html);
        }


        private List<SectionItemModel> GetItemsFromDoc(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var list = new List<SectionItemModel>();

            var nodes = doc.DocumentNode?.SelectNodes("//div[@class='list-item']");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var titleNode = node.SelectSingleNode(".//span[@class='list-title']/a");
                    var typeNode = node.SelectNodes(".//span[@class='list-type']");
                    var timeNode = node.SelectSingleNode(".//span[@class='list-time']");
                    if (titleNode == null || typeNode == null || timeNode == null)
                    {
                        continue;
                    }
                    if(typeNode.Count != 1 )
                    {
                        var item = new SectionItemModel
                        {
                            Title = titleNode?.InnerText.Trim(),
                            Url = titleNode?.GetAttributeValue("href", string.Empty),
                            Type = typeNode?[1].InnerText.Trim(),
                            Area = typeNode?[0].InnerText.Trim(),
                            Time = timeNode?.InnerText.Trim()
                        };

                        list.Add(item);
                    }
                    else
                    {
                        var item = new SectionItemModel
                        {
                            Title = titleNode?.InnerText.Trim(),
                            Url = titleNode?.GetAttributeValue("href", string.Empty),
                            Type = typeNode?[0].InnerText.Trim(),
                            Area = typeNode?[0].InnerText.Trim(),
                            Time = timeNode?.InnerText.Trim()
                        };

                        list.Add(item);
                    }


                }
            }
            return list;
        }

        private async Task<List<VideoInfo>> GetInfosFromDocAsync(string html)
        {
            var res = new List<VideoInfo>();
            var items = GetItemsFromDoc(html);
            foreach (var i in items)
            {
                var url = i.Url;
                var itemHtml = await _httpClient.GetStringAsync(url);
                var info = GetInfoFromItem(itemHtml);
                info.SelfUrl = url;
                res.Add(info);
            }

            return res;
        }

        private VideoInfo GetInfoFromItem(string html)
        {
            var videoInfo = new VideoInfo();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 获取头图
            var coverNode = doc.DocumentNode
                .SelectSingleNode("//div[@class='vod-info']");
            videoInfo.CoverUrl =
                coverNode?.SelectSingleNode(".//img")?
                    .GetAttributeValue("src", string.Empty);

            // 获得标题和状态
            var detailNode = doc.DocumentNode
                .SelectSingleNode("//div[@class='vod-detail']");
            videoInfo.Name = detailNode
                .SelectSingleNode(".//h2")?.InnerText.Trim();
            videoInfo.Status = detailNode
                .SelectSingleNode(".//span")?.InnerText.Trim();

            // 获得其他基本信息
            var liNodes = detailNode.SelectNodes(".//ul/li");
            if (liNodes != null)
            {
                foreach (var li in liNodes)
                {
                    var text = li.InnerText.Trim();
                    var span = li.SelectSingleNode(".//span")?.InnerText.Trim();

                    if (text.StartsWith("别名"))
                        videoInfo.Alias = span;
                    else if (text.StartsWith("导演"))
                        videoInfo.Director = span;
                    else if (text.StartsWith("主演"))
                        videoInfo.Actor = span;
                    else if (text.StartsWith("类型"))
                        videoInfo.Type = span;
                    else if (text.StartsWith("地区"))
                        videoInfo.Area = span;
                    else if (text.StartsWith("语言"))
                        videoInfo.Language = span;
                    else if (text.StartsWith("上映"))
                        videoInfo.UploadTime = span;
                    else if (text.StartsWith("更新时间"))
                        videoInfo.UpdateTime = span;
                }
            }

            // 解析视频信息
            var videoUrlNodes = doc.DocumentNode
                .SelectNodes("//div[@class='vod-list']/div[@class='list-item']");
            foreach (var videoUrlNode in videoUrlNodes)
            {
                var urlNode = videoUrlNode
                    .SelectSingleNode(".//a");
                var url = urlNode?.GetAttributeValue("href", string.Empty);
                if(url != null && url.EndsWith(".m3u8"))
                {
                    videoInfo.Episodes.Add(url);
                }

            }

            return videoInfo;
        }
    }
}