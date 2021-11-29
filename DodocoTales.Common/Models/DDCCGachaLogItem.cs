using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool idLost { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int gachatype { get; set; }
    }
}
