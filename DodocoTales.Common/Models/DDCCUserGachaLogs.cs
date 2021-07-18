using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Common.Models
{
    public partial class DDCCUserGachaLogs
    {
        public long uid { get; set; }
        public List<DDCCVersionLogs> V { get; set; }
        public long lastUpdateBannerLibraryVersion { get; set; }
        public long lastUpdateUnitLibraryVersion { get; set; }
    }
}
