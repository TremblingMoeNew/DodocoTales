using DodocoTales.Common.Enums;
using DodocoTales.Library.StoragedUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.CurrentUser.Models
{
    public class DDCLRoundLogItem
    {
        public int VersionId { get; set; }
        public int BannerId { get; set; }
        public DDCCPoolType CategorizedGachaType { get; set; }

        public int EpitomizedPathID { get; set; }

        public List<DDCLGachaLogItem> Logs { get; set; }

        public DDCLStoragedRoundLogs Original { get; set; }
    }
}
