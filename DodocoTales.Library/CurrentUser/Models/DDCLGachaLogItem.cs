using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DodocoTales.Common.Enums;
using Newtonsoft.Json;

namespace DodocoTales.Library.CurrentUser.Models
{
    public class DDCLGachaLogItem
    {
        public ulong Id { get; set; }
        public ulong InternalId { get; set; }
        public ulong UnitClass { get; set; }
        public DateTime Time { get; set; }

        public bool IdLost { get; set; }
        public ulong Rank { get; set; }
        public DDCCPoolType GachaType { get; set; }
        public DDCCUnitType UnitType { get; set; }

        // 用于Library进行分类、搜索的额外信息
        public ulong VersionId { get; set; }
        public ulong BannerId { get; set; }
        public DDCCPoolType CategorizedGachaType { get; set; }

    }
}
