using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models.V0
{
    public class DDCLUserGachaLogV0
    {
        public long uid { get; set; }
        public List<DDCLStoragedVersionLogs> V { get; set; }
        public ulong lastUpdateBannerLibraryVersion { get; set; }
        public ulong lastUpdateUnitLibraryVersion { get; set; }
        public DDCCTimeZone zone { get; set; }
    }
}
