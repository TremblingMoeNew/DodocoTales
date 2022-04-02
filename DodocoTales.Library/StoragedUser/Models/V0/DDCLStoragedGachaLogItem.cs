using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models.V0
{
    public class DDCLStoragedGachaLogItem
    {

        public ulong id { get; set; }
        public ulong unitclass { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }

        // as backup
        public string code { get; set; }
        public ulong rank { get; set; }
        public DDCCUnitType unittype { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool idLost { get; set; }

        [DefaultValue(DDCCPoolType.Null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DDCCPoolType gachatype { get; set; }
    }
}
