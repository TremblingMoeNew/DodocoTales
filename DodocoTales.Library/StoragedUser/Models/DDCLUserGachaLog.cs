using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models
{
    public class DDCLUserGachaLog
    {
        public long uid { get; set; }
        public ulong lastUpdateBannerLibraryVersion { get; set; }
        public ulong lastUpdateUnitLibraryVersion { get; set; }
        public DDCCTimeZone zone { get; set; }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<DDCLGachaLogItem> Logs { get; set; }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<DDCLEpitomizedPathItem> EpitomizedPath { get; set; }
    }
}
