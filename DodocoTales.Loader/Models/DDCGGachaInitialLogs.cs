using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Models
{
    public class DDCGGachaInitialLogs
    {
        public List<DDCGGachaLogResponseItem> Beginner { get; set; }
        public List<DDCGGachaLogResponseItem> Permanent { get; set; }
        public List<DDCGGachaLogResponseItem> EventCharacter { get; set; }
        public List<DDCGGachaLogResponseItem> EventWeapon { get; set; }
    }
}
