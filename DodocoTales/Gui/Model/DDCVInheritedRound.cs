using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Library.Models;
using DodocoTales.Common.Models;

namespace DodocoTales.Gui.Model
{
    public class DDCVInheritedRound
    {
        public DDCLVersionInfo version { get; set; }
        public DDCLBannerInfo banner { get; set; }
        public int RoundIndex { get; set; }
        public DDCCRoundLogs Logs { get; set; }
    }
}
