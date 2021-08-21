using DodocoTales.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Compatibility.GenWishExport.Models
{
    public class DDCIGenWishExpImportResult
    {
        public long Uid { get; set; }
        public List<DDCCGachaLogItem> Beginner { get; set; }
        public List<DDCCGachaLogItem> Permanent { get; set; }
        public List<DDCCGachaLogItem> EventCharacter { get; set; }
        public List<DDCCGachaLogItem> EventWeapon { get; set; }
    }
}
