using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Common.Models
{
    public partial class DDCCBannerLogs
    {
        public int id { get; set; }
        public List<DDCCRoundLogs> R { get; set; }

        // as backup
        public DDCCPoolType poolType { get; set; }
    }
}
