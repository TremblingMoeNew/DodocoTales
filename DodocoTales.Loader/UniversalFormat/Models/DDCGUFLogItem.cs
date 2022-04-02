using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.UniversalFormat.Models
{
    public class DDCGUFLogItem
    {
        public string gacha_type { get; set; }
        public string time { get; set; }
        public string name { get; set; }
        public string item_type { get; set; }


        [DefaultValue("0")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string id { get; set; }

        [DefaultValue("0")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string uid { get; set; }
        [DefaultValue("0")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string rank_type { get; set; }

        public string uigf_gacha_type { get; set; }
    }
}
