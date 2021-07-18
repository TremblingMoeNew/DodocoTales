using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Common.Models
{
    public partial class DDCCVersionLogs
    {
        public int id { get; set; }
        public List<DDCCBannerLogs>B { get; set; }
    }
}
