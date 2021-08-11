using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.Models
{
    public class DDCLBannerInfo
    {
        public int id { get; set; }
        public DDCCPoolType type { get; set; }
        public string name { get; set; }
        public string hint { get; set; }
        public bool epitomizedPathEnabled { get; set; }
        public List<int> rank5Up { get; set; }
        public List<int> rank4Up { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool beginTimeSync { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool endTimeSync { get; set; }
    }
}
