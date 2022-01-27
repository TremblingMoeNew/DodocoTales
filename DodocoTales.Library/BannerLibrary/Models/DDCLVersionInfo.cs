using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.BannerLibrary.Models
{
    public class DDCLVersionInfo
    {
        public ulong id { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
        public List<DDCLBannerInfo> banners { get; set; }
    }
}
