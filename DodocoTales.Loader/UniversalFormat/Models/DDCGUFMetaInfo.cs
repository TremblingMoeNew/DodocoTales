using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.UniversalFormat.Models
{
    public class DDCGUFMetaInfo
    {

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string uid { get; set; }

        [DefaultValue("zh-cn")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string lang { get; set; }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string export_time { get; set; }

        [DefaultValue("Unknown")]
        [JsonProperty(DefaultValueHandling =DefaultValueHandling.Populate)]
        public string export_app { get; set; }

        [DefaultValue("0")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string export_app_version { get; set; }

        [DefaultValue("v2.1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string uigf_version { get; set; }
    }
}
