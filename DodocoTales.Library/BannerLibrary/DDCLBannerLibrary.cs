using DodocoTales.Library.BannerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.BannerLibrary
{
    public class DDCLBannerLibrary
    {
        DDCLBannerLibModel model;

        public List<DDCLVersionInfo> Versions { get; set; }
        public List<DDCLBannerInfo> Banners { get; set; }

        public DDCLVersionInfo GetVersion(int versionid)
            => Versions.Find(x => x.id == versionid);
        public DDCLBannerInfo GetBanner(int bannerid)
            => Banners.Find(x => x.id == bannerid);
    }
}
