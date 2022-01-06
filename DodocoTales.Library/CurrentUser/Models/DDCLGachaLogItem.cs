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
        public ulong StoragedId { get; set; }
        public int UnitClass { get; set; }
        public DateTime Time { get; set; }

        public bool IdLost { get; set; }
        public int Rank { get; set; }
        public DDCCPoolType GachaType { get; set; }
        public DDCCUnitType UnitType { get; set; }

        // 用于Library进行分类、搜索的额外信息
        public int VersionId { get; set; }
        public int BannerId { get; set; }
        public DDCCPoolType CategorizedGachaType { get; set; }
    }
}
