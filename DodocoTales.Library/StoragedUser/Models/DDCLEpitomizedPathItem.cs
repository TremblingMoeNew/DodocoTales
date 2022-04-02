using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models
{
    public class DDCLEpitomizedPathItem
    {
        public ulong version_id { get; set; }
        public ulong banner_id { get; set; }
        public ulong round_id { get; set; }
        public bool enabled { get; set; }
        public ulong unitclass { get; set; }
    }
}
