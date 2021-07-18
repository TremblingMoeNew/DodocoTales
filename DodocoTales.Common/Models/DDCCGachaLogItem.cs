using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Common.Models
{
    public class DDCCGachaLogItem
    {
        public ulong id { get; set; }
        public int unitclass { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }

        // as backup
        public string code { get; set; }
        public int rank { get; set; }
        public DDCCUnitType unittype { get; set; }
    }
}
