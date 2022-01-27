using DodocoTales.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.BannerLibrary.Models
{
    public class DDCLBannerInfo
    {
        public ulong id { get; set; }
        public DDCCPoolType type { get; set; }
        public string name { get; set; }
        public string hint { get; set; }
        public bool epitomizedPathEnabled { get; set; }
        public List<ulong> rank5Up { get; set; }
        public List<ulong> rank4Up { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool beginTimeSync { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool endTimeSync { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool forceGachaTypeStorage { get; set; }

        [JsonIgnore]
        public ulong InternalId { get; set; }
        [JsonIgnore]
        public ulong VersionId { get; set; }

        public DDCLBannerInfo Copy()
        {
            return new DDCLBannerInfo
            {
                id = this.id,
                type = this.type,
                name = this.name,
                hint = this.hint,
                epitomizedPathEnabled = this.epitomizedPathEnabled,
                rank5Up = this.rank5Up.FindAll(x => true),
                rank4Up = this.rank4Up.FindAll(x => true),
                beginTime = this.beginTime,
                endTime = this.endTime,
                beginTimeSync = this.beginTimeSync,
                endTimeSync = this.endTimeSync,
                forceGachaTypeStorage = this.forceGachaTypeStorage,
                internalId = this.internalId
            };
        }
    }
}
