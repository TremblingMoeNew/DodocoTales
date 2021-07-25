using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Common.Models
{
    public class DDCCUncategorizedLogs
    {
        public List<DDCCGachaLogItem> Beginner { get; set; }
        public List<DDCCGachaLogItem> Permanent { get; set; }
        public List<DDCCGachaLogItem> EventCharacter { get; set; }
        public List<DDCCGachaLogItem> EventWeapon { get; set; }
    }
}
