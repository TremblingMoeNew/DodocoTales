using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models.V0
{
    public class DDCLStoragedVersionLogs
    {
        public ulong id { get; set; }
        public List<DDCLStoragedBannerLogs> B { get; set; }
    }
}
