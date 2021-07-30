using DodocoTales.Common.Models;
using DodocoTales.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVBannerInfo
    {
        public DDCLVersionInfo VersionInfo { get; set; }
        public DDCLBannerInfo Info { get; set; }
        public DDCCBannerLogs Logs { get; set; }
        public List<DDCVInheritedRound> Inherited { get; set; }
    }
}
