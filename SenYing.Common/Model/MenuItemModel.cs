using SenYing.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenYing.Common.Model
{
    public class MenuItemModel
    {
        public string? Title { get; set; }
        public VideoType Type { get; set; }
        public List<MenuItemModel>? Items { get; set; }
    }
}
