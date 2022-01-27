using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.UnitLibrary.Models
{
    public partial class DDCLUnitInfo
    {
        public ulong id { get; set; }
        public DDCCUnitType type { get; set; }
        public ulong rank { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string portrait { get; set; }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string code_alias { get; set; }
    }
}
