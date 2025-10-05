using SenYing.Services;
using SenYing.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenYing.Common.Enum;
using Xunit.Abstractions;

namespace SenYing.UnitTest.Services
{
    public class M3u8ServiceTest
    {
        private readonly IM3u8Service _m3u8Service = App.ServiceProvider.GetRequiredService<IM3u8Service>();
        private readonly ITestOutputHelper _output;
        public M3u8ServiceTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public async Task GetInfosFromSection_Should_Success()
        {
            var list = await _m3u8Service.GetVideoInfoFromSectionAsync(VideoType.日剧);
            _output.WriteLine(list.Count.ToString());
            foreach (var item in list)
            {
                _output.WriteLine(item.Name);
            }
            Assert.True(list.Count > 0);
        }
    }
}
