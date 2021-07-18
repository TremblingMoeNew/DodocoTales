using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Models
{
    public class DDCGGachaLogResponseItem
    {
        public long uid { get; set; }
        public DDCCPoolType gacha_type { get; set; }
        public int count { get; set; }
        public DateTime time { get; set; }
        public string name { get; set; }
        public DDCCUnitType item_type { get; set; }
        public int rank_type { get; set; }
        public ulong id { get; set; }
    }
}
