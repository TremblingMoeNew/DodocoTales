using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Compatibility.KeqingNiuza.Model
{
    public class DDCIKeqingNiuzaWishData
    {
        // 与DDCGGachaLogResponseItem相同部分
        public long uid { get; set; }
        public DDCCPoolType gacha_type { get; set; }
        public DateTime time { get; set; }
        public string name { get; set; }
        public int rank_type { get; set; }
        public ulong id { get; set; }
        // 与DDCGGachaLogResponseItem不同部分：改为string（因为牛杂店用的是中文，无法直接反序列化为DDCCUnitType）
        public string item_type { get; set; }
        // 新增部分
        public string lang { get; set; }
        public bool IsLostId { get; set; }
    }
}
