using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.Models
{
    public class DDCLBannerLibModel
    {
        [JsonProperty(PropertyName = "beginner")]
        public List<DDCLBannerInfo> beginnerPools { get; set; }
        [JsonProperty(PropertyName = "permanent")]
        public List<DDCLBannerInfo> permanentPools { get; set; }

        [JsonProperty(PropertyName = "event")]
        public List<DDCLVersionInfo> eventPools { get; set; }
    }
}
