using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models
{
    public class DDCLUserGachaLog
    {
        public long uid { get; set; }
        public List<DDCLStoragedVersionLogs> V { get; set; }
        public long lastUpdateBannerLibraryVersion { get; set; }
        public long lastUpdateUnitLibraryVersion { get; set; }
        public DDCCTimeZone zone { get; set; }
    }
}
