using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models
{
    public class DDCLStoragedVersionLogs
    {
        public ulong id { get; set; }
        public List<DDCLStoragedBannerLogs> B { get; set; }
    }
}
