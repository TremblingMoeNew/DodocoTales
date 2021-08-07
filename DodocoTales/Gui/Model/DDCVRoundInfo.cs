using DodocoTales.Common.Models;
using DodocoTales.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Model
{
    public class DDCVRoundInfo
    {
        public DDCLVersionInfo VersionInfo { get; set; }
        public DDCLBannerInfo BannerInfo { get; set; }
        public List<DDCCRoundLogs> Logs { get; set; }
        public List<DDCVInheritedRound> Inherited { get; set; }
        public int Index { get; set; }
    }
}
