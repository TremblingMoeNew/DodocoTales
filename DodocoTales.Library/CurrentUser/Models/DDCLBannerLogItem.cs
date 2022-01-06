using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.CurrentUser.Models
{
    public class DDCLBannerLogItem
    {
        public int BannerId { get; set; }

        public int VersionId { get; set; }
        public DDCCPoolType CategorizedGachaType { get; set; }

        public List<DDCLRoundLogItem> BasicRounds { get; set; }
        public List<DDCLRoundLogItem> GreaterRounds { get; set; }
        public List<DDCLGachaLogItem> Logs { get; set; }
    }
}
